import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRoom } from './room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  roomUrl: string = 'http://localhost:5000/api/Room';

  constructor(private httpClient: HttpClient) { }

  getRooms(token: string): Observable<IRoom[]> {
    let headers = new HttpHeaders().set('Authorization', `Bearer ${token}`);
    return this.httpClient.get<IRoom[]>(this.roomUrl, { headers });
  }
}
