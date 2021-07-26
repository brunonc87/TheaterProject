import { Component, OnInit } from '@angular/core';
import { ISection } from './section';
import { SectionService } from './section.service';

@Component({
  selector: 'app-section-list',
  templateUrl: './section-list.component.html',
  styleUrls: ['./section-list.component.scss']
})
export class SectionListComponent implements OnInit {

  sections: ISection[] = [];
  
  constructor(private sectionService: SectionService) { }

  ngOnInit(): void {
    this.getSections();
  }

  getSections(): void {
    this.sectionService.getSections().subscribe((sections) => { this.sections = sections; }, (error) => {
      console.log(error)
   });
   
  }

  deleteSection(sectionId: number): void {
    if (confirm("Tem certeza que deseja excluir a seção desejada?"))
    {
      this.sectionService.deleteSection(sectionId).subscribe((response) => { this.reload()}, (error) => {
        alert(error)
     });
    }
    
  }  

  public parseAudioIdToValue (id: number) : string {
    switch (id) {
        case 2:
            return 'Dublado';
        case 1:
        default:
            return 'Original';
    }   
  }

  reload(): void {
    this.getSections();

}   
}
