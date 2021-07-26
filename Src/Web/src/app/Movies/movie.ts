
export interface IMovie{
    id: number;
    tittle: string;
    description: string;
    duration: number;
}

export class MovieAddCommand {
    tittle: string = '';
    description: string = '';
    duration: number = 0;    

    constructor() {
    }
}

export class MovieEditCommand {
    id: number = 0;
    tittle: string = '';
    description: string = '';
    duration: number = 0;
}