using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Api.DTO.Sections;
using Theater.Domain.Sections;

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionsService _sectionService;

        public SectionController(ISectionsService sectionService)
        {
            _sectionService = sectionService;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                List<SectionModel> sectionModels = new List<SectionModel>();
                IEnumerable<Section> sections = _sectionService.GetSections();

                foreach (Section section in sections)
                {
                    SectionModel sectionModel = new SectionModel();
                    sectionModel.ConsertSectionToModel(section);
                    sectionModels.Add(sectionModel);
                }

                return Ok(sectionModels);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // POST api/<MovieController>
        [HttpPost]
        public IActionResult Add([FromBody] SectionAddCommand sectionAddCommand)
        {
            try
            {
                _sectionService.AddSection(sectionAddCommand.ConvertToSection());
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // DELETE api/<MovieController>/5
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _sectionService.RemoveSection(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
