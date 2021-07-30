import { NotFoundComponent } from './not-found/not-found.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthenticationComponent } from './Authentication/authentication.component';
import { MainFormComponent } from './main-form/main-form.component';

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
        loadChildren: () => import("./movies/movies.module").then(x => x.MoviesModule)
      },
      {
        path: 'rooms',
        loadChildren: () => import("./rooms/rooms.module").then(x => x.RoomsModule)
      },
      {
        path: 'sections',
        loadChildren: () => import("./sections/sections.module").then(x => x.SectionsModule)
      }
    ]
  },
  {
    path: '**',
    redirectTo: '404-not-found'
  },
  {
    path: '404-not-found',
    loadChildren: () => import("./not-found/not-found.module").then(x => x.NotFoundModule),
    pathMatch: 'full'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
