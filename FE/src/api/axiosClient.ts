import axios, { type AxiosError, type AxiosInstance, type AxiosResponse, type InternalAxiosRequestConfig } from 'axios'
import type { ApiResponse, ApiResponseData } from '../types/api.types'
import { useAuthStore } from '../stores/authStore'
import { toastService } from '../utils/toastService'
import router from '../router'
import { Capacitor } from '@capacitor/core'

// ============================================================
// 1. TẠO AXIOS INSTANCE
// ============================================================

// Trên điện thoại, "localhost" trỏ về chính điện thoại → dùng IP LAN của máy tính
const baseURL = Capacitor.isNativePlatform()
  ? (import.meta.env.VITE_API_BASE_URL_MOBILE || 'https://10.6.44.218:7092/api')
  : (import.meta.env.VITE_API_BASE_URL || 'https://localhost:7092/api')

const apiClient: AxiosInstance = axios.create({
  baseURL,
  timeout: Number(import.meta.env.VITE_API_TIMEOUT) || 15000,
  headers: {
    'Content-Type': 'application/json',
    'Accept': 'application/json',
  },
})

// ============================================================
// 2. REQUEST INTERCEPTOR - Gắn Token tự động
// ============================================================

apiClient.interceptors.request.use(
  (config: InternalAxiosRequestConfig) => {
    const authStore = useAuthStore()
    const token = authStore.token

    if (token && config.headers) {
      config.headers.Authorization = `Bearer ${token}`
    }

    return config
  },
  (error: AxiosError) => {
    return Promise.reject(error)
  }
)

// ============================================================
// 3. RESPONSE INTERCEPTOR - Xử lý chuẩn response từ Backend
// ============================================================

apiClient.interceptors.response.use(
  // ✅ Response thành công (HTTP 2xx)
  (response: AxiosResponse) => {
    return response
  },

  // ❌ Response lỗi (HTTP 4xx, 5xx)
  (error: AxiosError<ApiResponse>) => {
    const status = error.response?.status
    const apiError = error.response?.data

    switch (status) {
      case 401:
        // Token hết hạn hoặc không hợp lệ → Đăng xuất
        toastService.warn('Phiên đăng nhập hết hạn. Vui lòng đăng nhập lại.')
        handleUnauthorized()
        break

      case 403:
        // Không có quyền truy cập
        toastService.error(apiError?.message ?? 'Bạn không có quyền thực hiện thao tác này.', 'Truy cập bị từ chối')
        break

      case 404:
        // Không tìm thấy resource
        toastService.warn(apiError?.message ?? 'Không tìm thấy dữ liệu yêu cầu.')
        break

      case 400:
        // Lỗi validation
        if (apiError?.errors) {
          const firstError = Object.values(apiError.errors)[0]?.[0]
          toastService.warn(firstError ?? apiError.message ?? 'Dữ liệu không hợp lệ.', 'Lỗi nhập liệu')
        } else {
          toastService.warn(apiError?.message ?? 'Dữ liệu không hợp lệ.', 'Lỗi nhập liệu')
        }
        break

      case 409:
        // Lỗi xung đột dữ liệu (Optimistic Concurrency)
        toastService.warn(
          apiError?.message ?? 'Dữ liệu đã được thay đổi bởi người khác. Vui lòng tải lại!',
          'Xung đột dữ liệu'
        )
        break

      case 422:
        // Lỗi nghiệp vụ (DomainException)
        toastService.error(apiError?.message ?? 'Thao tác không hợp lệ.', 'Lỗi nghiệp vụ')
        break

      case 500:
        // Lỗi server
        console.error('[API] Server Error:', apiError?.message, 'TraceId:', apiError?.traceId)
        toastService.error('Lỗi hệ thống. Vui lòng thử lại sau.')
        break

      default:
        // Lỗi mạng hoặc không xác định
        if (!error.response) {
          toastService.error('Không thể kết nối đến máy chủ. Vui lòng kiểm tra mạng.', 'Lỗi kết nối')
        }
        break
    }

    return Promise.reject(error)
  }
)

// ============================================================
// 4. HELPER: Xử lý khi token hết hạn
// ============================================================

function handleUnauthorized(): void {
  const authStore = useAuthStore()
  authStore.logout()
  router.replace({ name: 'Login' })
}

// ============================================================
// 5. HELPER FUNCTIONS - Trích xuất data từ ApiResponse
// ============================================================

/**
 * Gọi API và trả về data đã unwrap từ ApiResponse<T>.
 * Ném lỗi nếu isSuccess === false.
 *
 * @example
 * const users = await apiGet<UserDto[]>('/admin/users')
 */
export async function apiGet<T>(url: string, params?: Record<string, unknown>): Promise<T> {
  const response = await apiClient.get<ApiResponseData<T>>(url, { params })
  return unwrapResponse(response)
}

export async function apiPost<T>(url: string, data?: unknown): Promise<T> {
  const response = await apiClient.post<ApiResponseData<T>>(url, data)
  return unwrapResponse(response)
}

export async function apiPut<T = void>(url: string, data?: unknown): Promise<T> {
  const response = await apiClient.put<ApiResponseData<T>>(url, data)
  return unwrapResponse(response)
}

export async function apiDelete<T = void>(url: string): Promise<T> {
  const response = await apiClient.delete<ApiResponseData<T>>(url)
  return unwrapResponse(response)
}

/**
 * Gọi API và trả về raw ApiResponse (không unwrap data).
 * Dùng khi cần kiểm tra isSuccess, message, errors trực tiếp.
 */
export async function apiRaw<T>(url: string, config?: Parameters<typeof apiClient.request>[0]): Promise<ApiResponseData<T>> {
  const response = await apiClient.request<ApiResponseData<T>>({ url, ...config })
  return response.data
}

/**
 * Gọi API chuyên dùng để upload file (FormData).
 * Tự động ghi đè header Content-Type để BE đọc được IFormFile.
 *
 * @example
 * const form = new FormData()
 * form.append('file', fileBlob)
 * form.append('title', 'Tờ trình mua sắm')
 * const doc = await apiUpload<DocumentDto>('/documents', form)
 */
export async function apiUpload<T>(url: string, formData: FormData): Promise<T> {
  const response = await apiClient.post<ApiResponseData<T>>(url, formData, {
    headers: {
      'Content-Type': 'multipart/form-data',
    },
  })
  return unwrapResponse(response)
}

// ============================================================
// 6. UNWRAP RESPONSE
// ============================================================

function unwrapResponse<T>(response: AxiosResponse<ApiResponseData<T>>): T {
  const apiResponse = response.data

  if (!apiResponse.isSuccess) {
    // Server trả về 200 nhưng isSuccess = false (trường hợp hiếm)
    throw new ApiError(apiResponse.message ?? 'Unknown error', apiResponse)
  }

  return apiResponse.data as T
}

// ============================================================
// 7. CUSTOM ERROR CLASS
// ============================================================

export class ApiError extends Error {
  public readonly response: ApiResponse

  constructor(message: string, response: ApiResponse) {
    super(message)
    this.name = 'ApiError'
    this.response = response
  }

  /** Lấy validation errors (nếu có) */
  get validationErrors(): Record<string, string[]> | null {
    return this.response.errors ?? null
  }

  /** Lấy traceId để debug */
  get traceId(): string | null {
    return this.response.traceId ?? null
  }
}

// Export instance mặc định
export default apiClient
