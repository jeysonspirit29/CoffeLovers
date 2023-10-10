import { Component, OnInit } from '@angular/core';
import { AuthService } from '../services/auth.service';
import { Product, ProductPaginated } from '../shared/models/Product.model';
import { ProductService } from '../services/product.service';
import { CreateOrderRequest, OrderDetail } from '../shared/models/Order.model';
import { CreateOrderGuard } from '../shared/guards/create-order.guard';
import { OrderService } from '../services/order.service';
import { Route, Router } from '@angular/router';


@Component({
  selector: 'app-create-order',
  templateUrl: './create-order.component.html',
  styleUrls: ['./create-order.component.css']
})
export class CreateOrderComponent implements OnInit {

  products : Product[] = [];
  orderDetail : OrderDetail[] = [];
  totalAmount: number = 0;
  constructor(private authService: AuthService,
    private productService: ProductService,
    private orderService: OrderService,
    private router: Router 
    ) { }

  ngOnInit(): void {
    this.loadProducts();
 
   }
 
   loadProducts(){
     this.productService.getProducts(1,100)
     .subscribe({
        next: (response : ProductPaginated ) => {
         this.products = response.items;
        }
     });
   }
  

   addProduct(productId:number){
    if(productId > 0){
      let orderDetail = this.orderDetail.find(x => x.productId == productId);
      if(orderDetail == null){
        let product = this.products.find(x => x.id == productId);
        this.orderDetail.push({
          price : product?.price ?? 0,
          quantity : 1,
          amountSubtotal : product?.price ?? 0,
          product : product?.name ?? "",
          productPhotoURL : product?.photoURL ?? "",
          productId: product?.id ?? 0
        });
        this.calculateAmounts();
      }
    }
   }

   increase(productId:number){
     const isValidStock = this.checkStock(productId,true);
     if(isValidStock){
      this.changeQuantity(productId, 1);
     }
   }


   decrease(productId:number){
    const isValidStock = this.checkStock(productId,false);
    if(isValidStock){
      this.changeQuantity(productId, -1);
    }
   }

   changeQuantity(productId: number, addQuantity:number) {
    let orderDetail = this.orderDetail.find(x => x.productId == productId);
    if (orderDetail != null) {
      orderDetail.quantity = orderDetail.quantity + addQuantity;
      orderDetail.amountSubtotal = orderDetail.quantity * orderDetail.price;
      this.updateOrderDetail(orderDetail);
    }
  }

   updateOrderDetail(orderDetail: OrderDetail){
    let itemIndex = this.orderDetail.findIndex(x => x.productId == orderDetail.productId);
    this.orderDetail[itemIndex] = orderDetail;
    this.calculateAmounts();
   }

   checkStock(productId:number, increase: boolean){
    let currentQuantity = this.getAddedQuantity(productId);
    let product = this.products.find(x => x.id == productId);

    if(increase){
      let remaingStock = (product?.stock ?? 0) - currentQuantity;
      console.log("STOCK",product?.stock ?? 0)
      console.log("remaingStock",remaingStock)
      if(remaingStock <= 0){
        alert("Se agotó el stock del producto "+product?.name+".");
        return false;
      }
    }
    else{
      if(currentQuantity <= 1){
        return false;
      }
    }
    return true;
   }

   deleteOrderDetail(productId:number){
    this.orderDetail = this.orderDetail.filter(x => x.productId != productId);
    this.calculateAmounts();
   }

   calculateAmounts(){
    this.totalAmount = this.orderDetail.reduce((n, {amountSubtotal}) => n + amountSubtotal, 0)
   }

   getAddedQuantity(productId:number):number{
     let orderDetail = this.orderDetail.find(x => x.productId == productId);
     console.log("CURRENT Q",orderDetail?.quantity ?? 0)
    return orderDetail?.quantity ?? 0;
   }

   sendOrder(){
    if(this.orderDetail.length <= 0){
      alert("Debe agregar al menos un producto.")
      return;
    }
    const request = this.getCreateOrderRequest();
    this.createOrder(request);
   }

   getCreateOrderRequest() : CreateOrderRequest{
    const request: CreateOrderRequest = {
      taxId:  1,
      items:  this.orderDetail
    };
    return request;
  }

   private createOrder(request: CreateOrderRequest) {
    this.orderService.createOrder(request)
      .subscribe({
        next: (response) => {
          console.log("response",response)
          if (response) {
            alert("La orden # "+response+" se creó con éxito.");
            this.router.navigate(["/orders"]);
          }
          else {
            alert("No se pudo realizar la operación.");
          }
        }
      });
  }
}
