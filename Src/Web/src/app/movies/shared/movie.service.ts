import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, of, throwError } from 'rxjs';
import { IMovie, MovieAddCommand, MovieEditCommand } from './movie';3
import { catchError } from 'rxjs/operators'

@Injectable({
  providedIn: 'root'
})
export class MovieService {

  movieurl: string = "http://localhost:5000/api/Movie"

  constructor(private httpCLient: HttpClient) { }

  getMovies(token: string): Observable<IMovie[]> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpCLient.get<IMovie[]>(this.movieurl, { headers } );
  }

  getMovie(movieId: number, token: string): Observable<IMovie> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpCLient.get<IMovie>(`${this.movieurl}/${movieId}`, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  addMovie(movie: MovieAddCommand, token: string): Observable<any> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpCLient.post<MovieAddCommand>(this.movieurl, movie, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  updateMovie(movie: MovieEditCommand, token: string): Observable<any> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpCLient.put<MovieEditCommand>(this.movieurl, movie, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  deleteMovie(movieTittle: string, token: string): Observable<any> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpCLient.delete(`${this.movieurl}/${movieTittle}`, { headers }).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }
}
