import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
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

  getSections(token: string): Observable<ISection[]>{
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<ISection[]>(this.sectionUrl, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  addSection(sectionAddCommand: SectionAddCommand, token: string): Observable<any> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.post(this.sectionUrl, sectionAddCommand, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      }));
  }

  deleteSection(sectionId: number, token: string): Observable<any> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.delete(`${this.sectionUrl}/${sectionId}`, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      }));
  }
}
