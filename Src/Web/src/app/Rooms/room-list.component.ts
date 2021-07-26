import { Component, OnInit } from '@angular/core';
import { IRoom } from './room';
import { RoomService } from './room.service';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.scss']
})
export class RoomListComponent implements OnInit {

  rooms: IRoom[] = [];
  
  constructor(private roomService: RoomService) { }

  ngOnInit(): void {
    this.getRooms()
  }

  getRooms(): void {
    this.roomService.getRooms()
        .subscribe(rooms => this.rooms = rooms);
}
}
