import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserCredential, LoginInfoModel } from './credential';

@Injectable({
  providedIn: 'root'
})
export class AuthenticationService {

  authenticationUrl: string = 'http://localhost:5000/api/Authentication';

  constructor(private httpClient: HttpClient) { }

  authenticate(credential: UserCredential): Observable<LoginInfoModel> {
    return this.httpClient.post<LoginInfoModel>(this.authenticationUrl, credential).pipe(
      catchError((err: HttpErrorResponse) => {
        return throwError(err.error);
      })
    );
  }
}
