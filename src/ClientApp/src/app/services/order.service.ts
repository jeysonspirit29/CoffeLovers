import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http'
import { environment } from 'src/environments/environment';
import { catchError, Observable, tap, throwError } from 'rxjs';
import { AuthService } from './auth.service';
import { ChangeOrderStatusRequest, CreateOrderRequest, Order, OrderPaginated } from '../shared/models/Order.model';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  url = environment.baseUrl;
  
  constructor(private http: HttpClient,
            private authService: AuthService) { }


  getOrders(pageNumber: number, pageSize: number) : Observable<OrderPaginated>{
    return this.http.get<OrderPaginated>(this.url+"/orders?PageNumber="+pageNumber+"&PageSize="+pageSize, this.getHeaders());
  }

  takeOrder(changeOrderStatusRequest:ChangeOrderStatusRequest){
    return this.http.patch(this.url+"/orders",changeOrderStatusRequest, this.getHeaders());
  }
  
  createOrder(createOrderRequest:CreateOrderRequest){
    return this.http.post(this.url+"/orders",createOrderRequest, this.getHeaders());
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
