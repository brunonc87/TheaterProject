import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MovieService } from 'src/app/Movies/movie.service';
import { RoomService } from 'src/app/Rooms/room.service';
import { SectionAddCommand } from '../section';
import { SectionService } from '../section.service';

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
    private roomService: RoomService, private router: Router) { }

  ngOnInit(): void {
    this.movieService.getMovies().subscribe((movies) => { this.movies = movies.map(movie => {return movie.tittle})}, (error) => { console.log(error)});
    this.roomService.getRooms().subscribe((rooms) => { this.rooms = rooms.map(room => {return room.name})}, (error) => { console.log(error)});
  }

  addSection(): void{
    this.parseAudioValueToId();
    console.log(this.sectionAddCommand);
    this.sectionService.addSection(this.sectionAddCommand).subscribe((response) => { this.router.navigate(['/sections']); }, (error) => {alert(error)});
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
