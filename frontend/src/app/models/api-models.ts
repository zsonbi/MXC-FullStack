
export interface EventDto {
  id?: string; 
  createdAt?: string;
  name: string;
  location?: string;
  country?: string;
  capacity: number;
}

export interface LoginRequest {
  userName?: string;
  password?: string;
  rememberMe?: boolean;
}

export interface UserProfileDto {
  userName: string;
  email?: string;
}