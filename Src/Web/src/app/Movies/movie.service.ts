import { HttpClient, HttpErrorResponse } from '@angular/common/http';
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

  getMovies(): Observable<IMovie[]> {
    return this.httpCLient.get<IMovie[]>(this.movieurl);
  }

  getMovie(movieId: number): Observable<IMovie> {
    return this.httpCLient.get<IMovie>(`${this.movieurl}/${movieId}`).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  addMovie(movie: MovieAddCommand): Observable<any> {
    return this.httpCLient.post<MovieAddCommand>(this.movieurl, movie).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  updateMovie(movie: MovieEditCommand): Observable<any> {
    return this.httpCLient.put<MovieEditCommand>(this.movieurl, movie).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }

  deleteMovie(movieTittle: string): Observable<any> {
    return this.httpCLient.delete(`${this.movieurl}/${movieTittle}`).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }
}
