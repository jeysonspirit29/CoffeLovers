import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { ChangeOrderStatusRequest, Order, OrderPaginated } from '../shared/models/Order.model';
import { ProductPaginated } from '../shared/models/Product.model';

@Injectable({
  providedIn: 'root'
})
export class ProductService {

  url = environment.baseUrl;
  
  constructor(private http: HttpClient,
            private authService: AuthService) { }


  getProducts(pageNumber: number, pageSize: number) : Observable<ProductPaginated>{
    return this.http.get<ProductPaginated>(this.url+"/products?PageNumber="+pageNumber+"&PageSize="+pageSize, this.getHeaders());
  }

  getHeaders() : Object {
    return  {
      headers: new HttpHeaders({
        'Bearer-Token':  this.authService.getToken(),
       'Authorization': `bearer ${this.authService.getToken()}`
      })
    };

  }
  
}
