import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MovieAddCommand } from '../movie';
import { MovieService } from '../movie.service';

@Component({
  selector: 'app-movie-add',
  templateUrl: './movie-add.component.html',
  styleUrls: ['./movie-add.component.scss']
})
export class MovieAddComponent implements OnInit {

  movie = new MovieAddCommand();
  constructor(private movieService: MovieService, private router: Router) { }
  
  ngOnInit(): void {
  }

  addMovie(): void {
    this.movieService.addMovie(this.movie).subscribe(
      () => { 
        this.router.navigate(['/movies']); 
      }, 
      (error) => { 
        alert(error)
      });    
  }
}
