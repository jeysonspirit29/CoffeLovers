import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { PageNotFoundComponent } from './page-not-found/page-not-found.component';
import { OrdersComponent } from './orders/orders.component';
import { CreateOrderComponent } from './create-order/create-order.component';
import { OrdersGuard } from './shared/guards/orders.guard';
import { LoginGuard } from './shared/guards/login.guard';
import { CreateOrderGuard } from './shared/guards/create-order.guard';

const routes: Routes = [
  { path: 'login', component: LoginComponent,  canActivate: [LoginGuard], canLoad: [LoginGuard] },
  { path: 'orders', component: OrdersComponent, canActivate: [OrdersGuard], canLoad:[OrdersGuard]},
  { path: 'create-order', component: CreateOrderComponent, canActivate: [CreateOrderGuard], canLoad:[CreateOrderGuard] },
  { path: 'error', component: PageNotFoundComponent},
  { path: '', redirectTo: 'login', pathMatch: 'full'},
  { path: '**', redirectTo: 'error', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
