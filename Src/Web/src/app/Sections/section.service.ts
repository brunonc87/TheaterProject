import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ISection, SectionAddCommand } from './section';

@Injectable({
  providedIn: 'root'
})
export class SectionService {

  sectionUrl: string = 'http://localhost:5000/api/Section';

  constructor(private httpClient: HttpClient) { }

  getSections(): Observable<ISection[]>{
    return this.httpClient.get<ISection[]>(this.sectionUrl).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  addSection(sectionAddCommand: SectionAddCommand): Observable<any> {
    return this.httpClient.post(this.sectionUrl, sectionAddCommand).pipe(
      catchError((err: HttpErrorResponse) => { 
        return throwError(err.error);
      }));
  }

  deleteSection(sectionId: number): Observable<any> {
    return this.httpClient.delete(`${this.sectionUrl}/${sectionId}`).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      }));
  }
}
