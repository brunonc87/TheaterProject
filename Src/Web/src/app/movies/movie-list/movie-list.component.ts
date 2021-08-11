import { GuardService } from './../../shared/guard.service';
import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { IMovie } from "../shared/movie";
import { MovieService } from "../shared/movie.service";

@Component({
    selector: 'movie-list',
    templateUrl: './movie-list.component.html',
    styleUrls: ['./movie-list.component.scss']
  })
  export class MovieListComponent implements OnInit{

    constructor(private movieService: MovieService, private router: Router, private guardService: GuardService) {
    }
    movies: IMovie[] = [];

    ngOnInit(): void {
        this.getMovies();
    }

    getMovies(): void {
        this.movieService.getMovies(this.guardService.getAuthToken())
            .subscribe(movies => this.movies = movies);
    }

    deleteMovie(movietittle: string): void {
        if(confirm("Tem certeza que deseja excluir o filme \""+movietittle+"\"?")) {
        this.movieService.deleteMovie(movietittle, this.guardService.getAuthToken()).subscribe((response) => { this.reload() }, (error) => {
            alert(error)
        })};
    }

    editMovie(movieId: number): void {
        this.router.navigate(["/movies/edit", movieId]);
    }

     reload(): void {
         this.getMovies();
     }

     addMovie(): void {
       this.router.navigate(["movies/add"]);
     }
  }
