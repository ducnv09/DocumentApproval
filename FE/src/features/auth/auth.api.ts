import { apiPost } from '../../api/axiosClient'
import type { LoginRequest, LoginResponse } from './auth.types'

// ============================================================
// AUTH API - Các endpoint liên quan đến xác thực
// ============================================================

const BASE = '/auth'

/**
 * Đăng nhập với username/password.
 * Backend: POST /api/auth/login
 */
export function login(request: LoginRequest): Promise<LoginResponse> {
  return apiPost<LoginResponse>(`${BASE}/login`, request)
}
