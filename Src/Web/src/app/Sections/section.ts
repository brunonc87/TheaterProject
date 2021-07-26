export interface ISection {
    id: number;
    startDate: Date;
    value: number;
    animationType: string;
    audioType: number;
    movieTittle: string;
    duration: number;
    roomName: string;
    numberOfSeats: number;
}

export class SectionAddCommand {
    movieTittle: string = '';
    roomName: string = '';
    startDate: Date = new Date();
    value: number = 0;
    animationType: string = '';
    audioType: number = 0;

    constructor() {
    }
}

