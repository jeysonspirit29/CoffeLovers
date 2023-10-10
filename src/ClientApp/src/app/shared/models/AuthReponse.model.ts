export interface AuthResponse {
    success: boolean;
    id: string;
    userName: string;
    fullName: string;
    message: string;
    rol: string;
    token: string;
  }

  export interface UserData {
    id: string;
    userName: string;
    fullName: string;
    rol: string;
  }