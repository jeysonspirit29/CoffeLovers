export interface Product {
    id: number;
    name: string;
    photoURL: string;
    price: number;
    stock: number;
  }

  export interface ProductPaginated {
    pageNumber: number;
    totalPages: number;
    totalCount: number;
    hasPreviousPage: boolean;
    hasNextPage: boolean;
    items: Product[];
  }

