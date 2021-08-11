import { GuardService } from './../../shared/guard.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { IMovie, MovieAddCommand, MovieEditCommand } from '../shared/movie';
import { MovieService } from '../shared/movie.service';

@Component({
  selector: 'app-movie-edit',
  templateUrl: './movie-edit.component.html',
  styleUrls: ['./movie-edit.component.scss']
})
export class MovieEditComponent implements OnInit {

  movie: MovieEditCommand = new MovieEditCommand();

  constructor(private route: ActivatedRoute, private movieService: MovieService, private router: Router, private guardService: GuardService) { }

  ngOnInit(): void {
    this.route.paramMap.subscribe(parameterMap => {
      const value = parameterMap.get('id');
      if (value != null)
      {
        const id = +value;
        this.getMovieById(id);
      }
      else
      {
        alert("Erro ao localizar o filme.")
        this.router.navigate(["/movies"]);
      }
    });
  }

  getMovieById(movieId: number) : void {
    this.movieService.getMovie(movieId, this.guardService.getAuthToken()).subscribe(
      (response) => {this.movie = response;},
      (error) => {alert(error);}
       )
  }
  editMovie(): void {
    this.movieService.updateMovie(this.movie, this.guardService.getAuthToken()).subscribe(
      () => {
        this.router.navigate(['/movies']);
      },
      (error) => {
        alert(error)
      });
  }
}
