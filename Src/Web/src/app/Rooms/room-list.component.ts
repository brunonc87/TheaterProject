import { GuardService } from './../shared/guard.service';
import { RoomService } from './shared/room.service';
import { Component, OnInit } from '@angular/core';
import { IRoom } from './shared/room';

@Component({
  selector: 'app-room-list',
  templateUrl: './room-list.component.html',
  styleUrls: ['./room-list.component.scss']
})
export class RoomListComponent implements OnInit {

  rooms: IRoom[] = [];

  constructor(private roomService: RoomService, private guardService: GuardService) { }

  ngOnInit(): void {
    this.getRooms()
  }

  getRooms(): void {
    this.roomService.getRooms(this.guardService.getAuthToken())
        .subscribe(rooms => this.rooms = rooms);
}
}
