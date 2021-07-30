import { SectionListComponent } from './section-list.component';
import { SectionAddComponent } from './section-add/section-add.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    component: SectionListComponent
  },
  {
    path: 'add',
    component: SectionAddComponent
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SectionsRoutingModule { }
