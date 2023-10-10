import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, CanActivateChild, CanDeactivate, CanLoad, Route, Router, RouterStateSnapshot, UrlSegment, UrlTree } from '@angular/router';
import { Observable } from 'rxjs';
import { AuthService } from '../../services/auth.service';
import { Constants } from '../Contants';

@Injectable({
  providedIn: 'root'
})
export class CreateOrderGuard implements CanActivate {
  constructor(private auth: AuthService, private router: Router) {}
  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    const isAuthenticated = this.auth.isAuthenticated();
    const hasRole= this.auth.hasRole(Constants.ROL_USER);
    if (!isAuthenticated) {
      this.router.navigate(['/login']);
      return false;
    } 

    if (!hasRole) {
      this.router.navigate(['/orders']);
      return false;
    }  

    return true
  }
  
}
