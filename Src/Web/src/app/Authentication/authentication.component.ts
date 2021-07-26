import { Component, OnInit } from '@angular/core';
import { AuthenticationService } from './authentication.service';
import { UserCredential } from './credential';

@Component({
  selector: 'app-authentication',
  templateUrl: './authentication.component.html',
  styleUrls: ['./authentication.component.scss']
})
export class AuthenticationComponent {

  usercredential: UserCredential = new UserCredential();

  constructor(private authenticationService: AuthenticationService) { }

  authenticate(): void {
    this.authenticationService.authenticate(this.usercredential).subscribe(
      (response) => { 
        if (response == true) {
          console.log(response);
        } 
        else {
           alert("Senha inválida"); 
          }},
      (error) => { alert(error) }
      );
  }
}
