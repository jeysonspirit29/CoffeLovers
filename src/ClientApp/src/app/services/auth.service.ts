import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Observable } from 'rxjs';
import { AuthResponse, UserData } from '../shared/models/AuthReponse.model';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  username?: string;
  authenticated: boolean = false;
  authorities: any = [];
  errorMessage!: string;

  constructor(private http: HttpClient) { }



  authenticate(credentials: any)  :Observable<AuthResponse> {
    return this.http.post<AuthResponse>(environment.baseUrl + '/auth', credentials);
  }

 

  saveUserLocal(auth: AuthResponse){
    localStorage.setItem("id", auth.id);
    localStorage.setItem("userName", auth.userName);
    localStorage.setItem("fullName", auth.fullName);
    localStorage.setItem("rol", auth.rol);
    localStorage.setItem("token", auth.token);
  }

  getUserData() : UserData{
    const userData: UserData = {
      id:   localStorage.getItem("id") ?? "",
      userName:   localStorage.getItem("userName")?? "",
      fullName:   localStorage.getItem("fullName")?? "",
      rol:   localStorage.getItem("rol")?? ""
    };
    return userData;
  }
  getToken():string{
    return localStorage.getItem("token") ?? "";
  }
 
  isAuthenticated():boolean{
    return (localStorage.getItem("token") ?? "") != "";
  }

  hasRole(role: string): boolean{
    const assignedRole = localStorage.getItem("rol")?? "";
    return role == assignedRole;
  }

  cleanUserData(){
    window.localStorage.clear();
  }

  listContainsRole(role: string) {
    return this.authorities.some((auth: { authority: string; }) => auth.authority === role);
    
  }

}
