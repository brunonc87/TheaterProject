import { Component, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { IMovie } from "./movie";
import { MovieService } from "./movie.service";

@Component({
    selector: 'movie-list',
    templateUrl: './movie-list.component.html',
    styleUrls: ['./movie-list.component.scss']
  })
  export class MovieListComponent implements OnInit{
    
    constructor(private movieService: MovieService, private router: Router) {
    

    }
    movies: IMovie[] = [];

    ngOnInit(): void {
        this.getMovies();
    }

    getMovies(): void {
        this.movieService.getMovies()
            .subscribe(movies => this.movies = movies);
    }

    deleteMovie(movietittle: string): void {
        if(confirm("Tem certeza que deseja excluir o filme \""+movietittle+"\"?")) {
        this.movieService.deleteMovie(movietittle).subscribe((response) => { this.reload() }, (error) => {
            alert(error)
        })};
    }

    editMovie(movieId: number): void {
        this.router.navigate(["/movie-edit", movieId]);
    }

     reload(): void {
         this.getMovies();

     }   
  }