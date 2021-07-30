using AutoMapper;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Theater.Application.Sections.Commands;
using Theater.Application.Sections.Models;
using Theater.Domain.Sections;

namespace Theater.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SectionController : ControllerBase
    {
        private readonly ISectionsService _sectionService;
        private readonly IMapper _mapper;

        public SectionController(ISectionsService sectionService, IMapper mapper)
        {
            _sectionService = sectionService;
            _mapper = mapper;
        }

        // GET: api/<MovieController>
        [HttpGet]
        public IActionResult Get()
        {
            try
            {
                IEnumerable<Section> sections = _sectionService.GetSections();

                return Ok(_mapper.Map<IEnumerable<SectionModel>>(sections));
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
                ValidationResult result = sectionAddCommand.Validate();
                if (!result.IsValid)
                    return BadRequest(result.Errors.First().ErrorMessage);

                _sectionService.AddSection(_mapper.Map<Section>(sectionAddCommand));
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
