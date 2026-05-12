// ============================================================
// TOAST SERVICE - Dùng được ở MỌI NƠI (không cần Vue component)
//
// Vấn đề: PrimeVue useToast() chỉ chạy trong <script setup>.
// Giải pháp: Tạo một event-bus đơn giản, để App.vue lắng nghe
// và chuyển tiếp cho PrimeVue Toast component.
//
// Cách dùng:
//   import { toastService } from '@/utils/toastService'
//   toastService.error('Phiên đăng nhập hết hạn')   // Từ axiosClient
//   toastService.warn('Dữ liệu đã thay đổi')        // Từ interceptor
// ============================================================

type ToastSeverity = 'success' | 'info' | 'warn' | 'error'

export interface ToastEvent {
  severity: ToastSeverity
  summary: string
  detail: string
  life: number
}

type ToastListener = (event: ToastEvent) => void

// ---- Singleton Event Bus ----
// Tạo một bộ nhớ lưu trữ tất cả các nơi đang muốn "nghe" thông báo.
const listeners: Set<ToastListener> = new Set()

/** Đăng ký listener (gọi 1 lần trong App.vue) */
export function onToast(listener: ToastListener): () => void {
  listeners.add(listener)
  // Trả về hàm unsubscribe
  return () => listeners.delete(listener)
}

/** Phát sự kiện toast (gọi từ bất kỳ đâu) */
function emit(event: ToastEvent): void {
  listeners.forEach(fn => fn(event))
}

// ---- Public API ----
export const toastService = {
  success(detail: string, summary = 'Thành công') {
    emit({ severity: 'success', summary, detail, life: 3000 })
  },

  error(detail: string, summary = 'Lỗi') {
    emit({ severity: 'error', summary, detail, life: 5000 })
  },

  warn(detail: string, summary = 'Cảnh báo') {
    emit({ severity: 'warn', summary, detail, life: 4000 })
  },

  info(detail: string, summary = 'Thông tin') {
    emit({ severity: 'info', summary, detail, life: 3000 })
  },
}
