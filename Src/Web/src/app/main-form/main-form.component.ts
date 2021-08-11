import { GuardService } from './../shared/guard.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-main-form',
  templateUrl: './main-form.component.html',
  styleUrls: ['./main-form.component.scss']
})
export class MainFormComponent implements OnInit {

  constructor(private router: Router, private guardService: GuardService) { }

  ngOnInit(): void {
    if(!this.guardService.isLoggedIn()) {
      this.onExitClick();
    }
    else {
      this.router.navigate(['/movies']);
    }

  }

  onExitClick(): void {
    this.guardService.deleteAuthToken();
    this.router.navigate(['/login']);
  }
}
