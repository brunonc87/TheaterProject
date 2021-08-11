import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { GuardService } from '../shared/guard.service';
import { AuthenticationService } from './shared/authentication.service';
import { LoginInfoModel, UserCredential } from './shared/credential';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent {

  usercredential: UserCredential = new UserCredential();

  loginInfo: LoginInfoModel = new LoginInfoModel();

  constructor(private authenticationService: AuthenticationService,
              private router: Router,
              private guardService: GuardService) { }

  authenticate(): void {
    this.authenticationService.authenticate(this.usercredential).subscribe((response) =>
      {
        this.loginInfo = response;
        this.guardService.addAuthToken(this.loginInfo.token);
        this.router.navigate(["/"]);
      }, (error) =>
      {
        alert(error)
      });
  }
}
