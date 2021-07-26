import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { IRoom } from './room';

@Injectable({
  providedIn: 'root'
})
export class RoomService {

  roomUrl: string = 'http://localhost:5000/api/Room';

  constructor(private httpClient: HttpClient) { }

  getRooms(): Observable<IRoom[]> {
    return this.httpClient.get<IRoom[]>(this.roomUrl);
  }
}
