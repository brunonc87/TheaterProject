import { MovieAddComponent } from './movies/movie-add/movie-add.component';
import { MovieEditComponent } from './movies/movie-edit/movie-edit.component';
import { MovieListComponent } from './movies/movie-list/movie-list.component';
import { MoviesModule } from './movies/movies.module';
import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomListComponent } from './rooms/room-list.component';
import { SectionListComponent } from './sections/section-list.component';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { AuthenticationComponent } from './Authentication/authentication.component';
import { SectionAddComponent } from './sections/section-add/section-add.component';
import { CurrencyMaskConfig, CurrencyMaskModule, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask';
import { MainFormComponent } from './main-form/main-form.component';
import { HttpClientModule } from '@angular/common/http';
import { RoomsModule } from './rooms/rooms.module';
import { SectionsModule } from './sections/sections.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { NotFoundModule } from './not-found/not-found.module';


registerLocaleData(ptBr);

export const CustomCurrencyMaskConfig: CurrencyMaskConfig = {
  align: "right",
  allowNegative: false,
  decimal: ",",
  precision: 2,
  prefix: "R$ ",
  suffix: "",
  thousands: ""
};

@NgModule({
  declarations: [
    AppComponent,
    MovieListComponent,
    MovieEditComponent,
    MovieAddComponent,
    RoomListComponent,
    SectionListComponent,
    AuthenticationComponent,
    SectionAddComponent,
    MainFormComponent,
    NotFoundComponent
   ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    CurrencyMaskModule,
    MoviesModule,
    RoomsModule,
    SectionsModule,
    NotFoundModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
