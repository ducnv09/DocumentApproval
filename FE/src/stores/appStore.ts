import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { Network } from '@capacitor/network'
import { toastService } from '../utils/toastService'

// ============================================================
// APP STORE - Trạng thái toàn cục của ứng dụng
// ============================================================
export const useAppStore = defineStore('app', () => {
  // ---- State ----
  const isLoading = ref(false)
  const loadingMessage = ref('')
  const isOnline = ref(true)
  const globalError = ref<string | null>(null)

  // Đếm số request đang chạy để tránh tắt loading sớm
  const activeRequests = ref(0)

  // ---- Getters ----
  const hasError = computed(() => !!globalError.value)

  // ---- Actions ----

  /** Bật loading indicator với message tùy chọn */
  function startLoading(message = ''): void {
    activeRequests.value++
    isLoading.value = true
    loadingMessage.value = message
  }

  /** Tắt loading indicator (chỉ tắt khi không còn request nào) */
  function stopLoading(): void {
    activeRequests.value = Math.max(0, activeRequests.value - 1)
    if (activeRequests.value === 0) {
      isLoading.value = false
      loadingMessage.value = ''
    }
  }

  /** Hiển thị lỗi toàn cục */
  function setError(message: string): void {
    globalError.value = message
  }

  /** Xóa lỗi toàn cục */
  function clearError(): void {
    globalError.value = null
  }

  /**
   * Khởi tạo listener theo dõi trạng thái mạng.
   * Sử dụng Capacitor Network plugin.
   */
  async function initNetworkListener(): Promise<void> {
    // Kiểm tra trạng thái hiện tại
    const status = await Network.getStatus()
    isOnline.value = status.connected

    // Lắng nghe thay đổi
    Network.addListener('networkStatusChange', (newStatus) => {
      isOnline.value = newStatus.connected
      if (!newStatus.connected) {
        setError('Mất kết nối mạng. Vui lòng kiểm tra lại.')
        toastService.error('Mất kết nối mạng. Vui lòng kiểm tra lại.', 'Mạng')
      } else {
        clearError()
        toastService.success('Đã kết nối lại mạng.', 'Mạng')
      }
    })
  }

  return {
    // State
    isLoading,
    loadingMessage,
    isOnline,
    globalError,
    // Getters
    hasError,
    // Actions
    startLoading,
    stopLoading,
    setError,
    clearError,
    initNetworkListener,
  }
})