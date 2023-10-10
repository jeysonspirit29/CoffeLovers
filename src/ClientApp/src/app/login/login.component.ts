import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

import { AuthService } from '../services/auth.service';
import { AuthResponse } from '../shared/models/AuthReponse.model';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  credentials = { 'username': '', 'password': '' };
  errorLogin: boolean = false;
  errorLoginMessage?: string;
  successLogin?: boolean;

  constructor(private authService: AuthService, private router: Router) { }

  ngOnInit(): void {
    this.successLogin = this.authService.authenticated;
    this.errorLoginMessage = this.authService.errorMessage;
  }

  login() {
    this.authService.authenticate(this.credentials)
    .subscribe(data => {
        if(data.success){
          this.authService.cleanUserData();
          this.authService.saveUserLocal(data);
          this.router.navigateByUrl('/orders').then(() => {
            window.location.reload();
          });;
        }
        else{
          this.errorLoginMessage = data.message; 
        }

      }) ;


    
  }
  


}
