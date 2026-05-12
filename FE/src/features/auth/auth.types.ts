// ============================================================
// Auth DTOs - Khớp với BE.Application.DTOs.Auth.LoginDto.cs
// ============================================================

export interface LoginRequest {
  username: string
  password: string
}

export interface LoginResponse {
  id: string
  username: string
  fullName: string
  email: string
  isAdmin: boolean
  token: string
}
