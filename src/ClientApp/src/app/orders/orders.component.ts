import { Component, OnInit } from '@angular/core';
import { OrderService } from '../services/order.service';
import { ChangeOrderStatusRequest, Order, OrderDetail, OrderPaginated } from '../shared/models/Order.model';
import { AuthService } from '../services/auth.service';
import { Constants } from '../shared/Contants';

@Component({
  selector: 'app-orders',
  templateUrl: './orders.component.html',
  styleUrls: ['./orders.component.css']
})
export class OrdersComponent implements OnInit {

  orders : Order[] = [];
  orderDetails : OrderDetail[] = [];
  constructor(private orderService : OrderService,
    private authService: AuthService) {
   }

  ngOnInit(): void {
   this.loadOrders();

  }

  loadOrders(){
    this.orderService.getOrders(1,100)
    .subscribe({
       next: (response : OrderPaginated ) => {
        this.orders = response.items;
       }
    }) ;
  }

  showDetail(orderId:number){
    let order = this.orders.find(x => x.id == orderId);
    this.orderDetails = order?.orderDetails ?? [];
  }

  visibleAttention(orderStatus:string){
  const user = this.authService.getUserData();
  return user.rol == Constants.ROL_EMPLOYEE && orderStatus == Constants.ORDERSTATUS_PENDING;
  }

  visibleDelivery(orderStatus:string){
    const user = this.authService.getUserData();
    return user.rol == Constants.ROL_EMPLOYEE && orderStatus == Constants.ORDERSTATUS_INPROGRESS;
    }

    visibleComplete(orderStatus:string){
      const user = this.authService.getUserData();
      return (user.rol == Constants.ROL_SUPERVISOR || user.rol == Constants.ROL_ADMINISTRATOR) && 
            orderStatus == Constants.ORDERSTATUS_DELIVERED;
    }

  changeOrderStatusToInProgress(orderId:number){
    const request: ChangeOrderStatusRequest = {
      orderId: orderId,
      orderStatusId: Constants.ORDERSTATUS_INPROGRESS_ID,
    };
    this.changeOrderStatus(request);
  }

  changeOrderStatusToDelivered(orderId:number){
    const request: ChangeOrderStatusRequest = {
      orderId: orderId,
      orderStatusId: Constants.ORDERSTATUS_DELIVERED_ID,
    };
    this.changeOrderStatus(request);
  }

  changeOrderStatusToComplete(orderId:number){
    const request: ChangeOrderStatusRequest = {
      orderId: orderId,
      orderStatusId: Constants.ORDERSTATUS_COMPLETED_ID,
    };
    this.changeOrderStatus(request);
  }

  private changeOrderStatus(request: ChangeOrderStatusRequest) {
    this.orderService.takeOrder(request)
      .subscribe({
        next: (response) => {
          if (response == true) {
            this.loadOrders();
            alert("La operación se realizó con éxito.");
          }
          else {
            alert("No se pudo realizar la operación.");
          }
        }
      });
  }
}
