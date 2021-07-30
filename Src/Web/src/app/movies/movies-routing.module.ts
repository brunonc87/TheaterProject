import { MovieAddComponent } from './movie-add/movie-add.component';
import { MovieListComponent } from './movie-list/movie-list.component';
import { MovieEditComponent } from './movie-edit/movie-edit.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';


const routes: Routes = [
  {
    path: '',
    component: MovieListComponent
  },
  {
    path: 'add',
    component: MovieAddComponent
  },
  {
    path: 'edit/:id',
    component: MovieEditComponent
  },

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MoviesRoutingModule { }
