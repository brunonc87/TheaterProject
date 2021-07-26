import { DEFAULT_CURRENCY_CODE, LOCALE_ID, NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';


import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { MovieListComponent } from './Movies/movie-list.component';
import { MovieEditComponent } from './Movies/movie-edit/movie-edit.component';
import { MovieAddComponent } from './Movies/movie-add/movie-add.component';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { RoomListComponent } from './Rooms/room-list.component';
import { SectionListComponent } from './Sections/section-list.component';

import ptBr from '@angular/common/locales/pt';
import { registerLocaleData } from '@angular/common';
import { AuthenticationComponent } from './Authentication/authentication.component';
import { SectionAddComponent } from './Sections/section-add/section-add.component';
import { CurrencyMaskConfig, CurrencyMaskModule, CURRENCY_MASK_CONFIG } from 'ng2-currency-mask';
import { MainFormComponent } from './main-form/main-form.component';
import { HttpClientModule } from '@angular/common/http';

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
    MainFormComponent    
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    CommonModule,
    FormsModule,
    CurrencyMaskModule
  ],
  providers: [
    { provide: LOCALE_ID, useValue: 'pt' },
    { provide: DEFAULT_CURRENCY_CODE, useValue: 'BRL' },
    { provide: CURRENCY_MASK_CONFIG, useValue: CustomCurrencyMaskConfig }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
