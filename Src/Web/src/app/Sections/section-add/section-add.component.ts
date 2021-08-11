import { GuardService } from './../../shared/guard.service';
import { RoomService } from './../../rooms/shared/room.service';
import { MovieService } from './../../movies/shared/movie.service';
import { SectionService } from './../section.service';
import { SectionAddCommand } from './../section';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';


@Component({
  selector: 'app-section-add',
  templateUrl: './section-add.component.html',
  styleUrls: ['./section-add.component.scss']
})
export class SectionAddComponent implements OnInit {

  animationTypes = ['2D', '3D'];
  audioTypes =  ['Dublado', 'Original'];
  movies: string[] = [];
  rooms: string[] = [];
  sectionAddCommand: SectionAddCommand = new SectionAddCommand();
  selectedAudio: string = '';

  constructor(private sectionService: SectionService, private movieService: MovieService,
    private roomService: RoomService, private router: Router, private guardService: GuardService) { }

  ngOnInit(): void {
    this.movieService.getMovies(this.guardService.getAuthToken()).subscribe((movies) => { this.movies = movies.map(movie => {return movie.tittle})}, (error) => { console.log(error)});
    this.roomService.getRooms(this.guardService.getAuthToken()).subscribe((rooms) => { this.rooms = rooms.map(room => {return room.name})}, (error) => { console.log(error)});
  }

  addSection(): void{
    this.parseAudioValueToId();
    console.log(this.sectionAddCommand);
    this.sectionService.addSection(this.sectionAddCommand, this.guardService.getAuthToken()).subscribe((response) => { this.router.navigate(['/sections']); }, (error) => {alert(error)});
  }

  public parseAudioValueToId () : void {
    switch (this.selectedAudio) {
        case 'Dublado':
            this.sectionAddCommand.audioType = 2;
            break;
        case 'Original':
        default:
            this.sectionAddCommand.audioType = 1;
            break;
    }
  }
}
