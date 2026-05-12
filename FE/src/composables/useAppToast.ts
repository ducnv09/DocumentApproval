import { useToast as usePrimeToast } from 'primevue/usetoast'

// ============================================================
// TOAST COMPOSABLE - Wrapper gọn cho PrimeVue Toast
// Dùng trong <script setup> của các component Vue.
//
// @example
// const toast = useAppToast()
// toast.success('Đã lưu thành công!')
// toast.error('Lỗi: không thể kết nối')
// toast.warn('Tờ trình đã được duyệt bởi người khác')
// toast.info('Đang tải dữ liệu...')
// ============================================================

export function useAppToast() {
  const toast = usePrimeToast()

  return {
    success(detail: string, summary = 'Thành công') {
      toast.add({ severity: 'success', summary, detail, life: 3000 })
    },

    error(detail: string, summary = 'Lỗi') {
      toast.add({ severity: 'error', summary, detail, life: 5000 })
    },

    warn(detail: string, summary = 'Cảnh báo') {
      toast.add({ severity: 'warn', summary, detail, life: 4000 })
    },

    info(detail: string, summary = 'Thông tin') {
      toast.add({ severity: 'info', summary, detail, life: 3000 })
    },

    /** Xóa toàn bộ toast đang hiển thị */
    clear() {
      toast.removeAllGroups()
    },
  }
}
