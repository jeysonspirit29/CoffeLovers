export interface Order {
    id: number;
    requestingUser: string;
    attentionUser: string;
    orderStatus: string;
    dateCompleted: Date;
    dateCreated: Date;
    taxPercentage: number;
    taxAmount: number;
    totalAmountBeforeTax: number;
    totalOrderAmount: number;
    orderDetails: OrderDetail[];
  }

  export interface OrderDetail {
    productId: number;
    product: string;
    productPhotoURL: string;
    quantity: number;
    price: number;
    amountSubtotal: number;
  }


  export interface OrderPaginated {
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    items: Order[];
  }

  export interface ChangeOrderStatusRequest{
    orderStatusId: number;
    orderId: number;
  }


  export interface CreateOrderRequest {
    taxId: number;
    items: CreateOrderDetailRequest[];
  }

  export interface CreateOrderDetailRequest {
    productId: number;
    quantity: number;
  }