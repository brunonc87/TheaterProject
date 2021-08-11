import { GuardService } from './../../shared/guard.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MovieAddCommand } from '../shared/movie';
import { MovieService } from '../shared/movie.service';


@Component({
  selector: 'app-movie-add',
  templateUrl: './movie-add.component.html',
  styleUrls: ['./movie-add.component.scss']
})
export class MovieAddComponent implements OnInit {

  movie = new MovieAddCommand();
  constructor(private movieService: MovieService, private router: Router, private guardService: GuardService) { }

  ngOnInit(): void {
  }

  addMovie(): void {
    this.movieService.addMovie(this.movie, this.guardService.getAuthToken()).subscribe(
      () => {
        this.router.navigate(['/movies']);
      },
      (error) => {
        alert(error)
      });
  }
}
