import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './Authentication/authentication.component';
import { MainFormComponent } from './main-form/main-form.component';
import { MovieAddComponent } from './Movies/movie-add/movie-add.component';
import { MovieEditComponent } from './Movies/movie-edit/movie-edit.component';
import { MovieListComponent } from './Movies/movie-list.component';
import { RoomListComponent } from './Rooms/room-list.component';
import { SectionAddComponent } from './Sections/section-add/section-add.component';
import { SectionListComponent } from './Sections/section-list.component';

const routes: Routes = [
  {
    path: 'login',
    component: AuthenticationComponent
  },
  {
    path: '',
    component: MainFormComponent,
    children: [ 
      {
        path: 'movies',
        component: MovieListComponent        
      },
      {
        path: 'movie-add',
        component: MovieAddComponent
      },
      {
        path: 'movie-edit/:id',
        component: MovieEditComponent
      },
      {
        path: 'rooms',
        component: RoomListComponent
      },
      {
        path: 'sections',
        component: SectionListComponent
      },
      {
        path: 'section-add',
        component: SectionAddComponent
      }
    ]
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
