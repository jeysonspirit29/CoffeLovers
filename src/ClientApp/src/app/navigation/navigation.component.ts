import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { finalize } from 'rxjs';
import { environment } from 'src/environments/environment';
import { AuthService } from '../services/auth.service';
import { Constants } from '../shared/Contants';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.css']
})
export class NavigationComponent implements OnInit {

  description = "";

  constructor(private auth: AuthService, private http: HttpClient, private router: Router) {

   }

  ngOnInit(): void {
    this.loadDescription();
  }

  loadDescription(){
    let user = this.auth.getUserData();
    this.description =  user.userName+" | " +user.fullName+" | Rol: "+user.rol;
  }

  isAuthenticated() {
    return this.auth.isAuthenticated();
  }

  isAuthenticatedAndUserRole() {
    return this.auth.isAuthenticated() && this.auth.hasRole(Constants.ROL_USER);
  }

  logout() {
    this.auth.cleanUserData();
    this.router.navigateByUrl('/login');
  }

}
