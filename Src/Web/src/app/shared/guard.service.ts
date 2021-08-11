import { LocalStorageService } from './local-storage.service';
import { Inject, Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class GuardService {

  private static isLogged: boolean = false;

  constructor(private localStorageService: LocalStorageService) { }

  addAuthToken(token: string): void {
    GuardService.isLogged = true;
    this.localStorageService.setValue('token', token);
  }

  getAuthToken(): string {
    return this.localStorageService.getValue('token');
    return '';
  }

  deleteAuthToken(): void {
    GuardService.isLogged = false;
    this.localStorageService.deleteValue('token');
  }

  isLoggedIn(): boolean {
    return GuardService.isLogged;
  }
}
