// ============================================================
// Chuẩn Response từ Backend (ApiResponse<T>)
// Khớp 1:1 với BE.Application.DTOs.ApiResponse.cs
//
// Đây là shared types dùng chung cho toàn bộ API Client.
// Domain-specific types nằm trong từng feature:
//   - @/features/auth/auth.types.ts
//   - @/features/admin/admin.types.ts
//   - @/features/document/document.types.ts
//   - @/features/approval/approval.types.ts
// ============================================================

/**
 * Response chuẩn từ backend (không có data).
 * Dùng cho các API trả về trạng thái đơn giản (delete, update).
 */
export interface ApiResponse {
  isSuccess: boolean
  message: string | null
  traceId: string | null
  errors: Record<string, string[]> | null
}

/**
 * Response chuẩn từ backend (có data).
 * Dùng cho các API trả về dữ liệu (get, create).
 */
export interface ApiResponseData<T> extends ApiResponse {
  data: T | null
}
