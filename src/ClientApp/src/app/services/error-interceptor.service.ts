import { HttpErrorResponse, HttpEvent, HttpHandler, HttpInterceptor, HttpRequest } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, throwError } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ErrorInterceptorService implements HttpInterceptor{

  constructor() { }
  handleError(error: HttpErrorResponse) {
    let errorMessage = 'Ocurrió un error: '
    
    if (error.status === 0 ) {
      console.error('Ocurrió un error:', error.error);
    } else {
      if (error.status === 401) {
        console.error("Error 401.");
      }
      errorMessage += error.error;
      console.error(
        `Backend retornó error ${error.status}, Error: `, error.error);
    }

    console.error(errorMessage);

    return throwError(() =>
      errorMessage);

  }
  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(req)
      .pipe(
        catchError(this.handleError))

  };
}
