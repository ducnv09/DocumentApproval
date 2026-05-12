import { defineStore } from 'pinia'
import { ref, computed } from 'vue'
import { Preferences } from '@capacitor/preferences'
import type { LoginResponse } from '../features/auth/auth.types'

// ============================================================
// KEY lưu trữ trên thiết bị (Capacitor Preferences)
// ============================================================
const STORAGE_KEYS = {
  TOKEN: 'app_token',
  USER: 'app_user',
} as const

// ============================================================
// AUTH STORE - Quản lý thông tin đăng nhập & Token
// ============================================================
export const useAuthStore = defineStore('auth', () => {
  // ---- State ----
  const token = ref<string | null>(null)
  const user = ref<LoginResponse | null>(null)
  const isInitialized = ref(false)

  // ---- Getters ----
  const isAuthenticated = computed(() => !!token.value)
  const isAdmin = computed(() => user.value?.isAdmin ?? false)
  const fullName = computed(() => user.value?.fullName ?? '')
  const userId = computed(() => user.value?.id ?? '')

  // ---- Actions ----

  /**
   * Khôi phục session từ Capacitor Preferences khi app khởi động.
   * Gọi 1 lần duy nhất trong main.ts trước khi mount app.
   */
  async function initialize(): Promise<void> {
    if (isInitialized.value) return

    try {
      const [savedToken, savedUser] = await Promise.all([
        Preferences.get({ key: STORAGE_KEYS.TOKEN }),
        Preferences.get({ key: STORAGE_KEYS.USER }),
      ])

      if (savedToken.value) {
        token.value = savedToken.value
      }

      if (savedUser.value) {
        user.value = JSON.parse(savedUser.value)
      }
    } catch (error) {
      console.error('[AuthStore] Lỗi khôi phục session:', error)
    } finally {
      isInitialized.value = true
    }
  }

  /**
   * Lưu thông tin đăng nhập sau khi login thành công.
   */
  async function setAuth(loginResponse: LoginResponse): Promise<void> {
    token.value = loginResponse.token
    user.value = loginResponse

    // Persist xuống thiết bị bằng Capacitor Preferences
    await Promise.all([
      Preferences.set({ key: STORAGE_KEYS.TOKEN, value: loginResponse.token }),
      Preferences.set({ key: STORAGE_KEYS.USER, value: JSON.stringify(loginResponse) }),
    ])
  }

  /**
   * Đăng xuất - Xóa toàn bộ session.
   */
  async function logout(): Promise<void> {
    token.value = null
    user.value = null

    await Promise.all([
      Preferences.remove({ key: STORAGE_KEYS.TOKEN }),
      Preferences.remove({ key: STORAGE_KEYS.USER }),
    ])
  }

  return {
    // State
    token,
    user,
    isInitialized,
    // Getters
    isAuthenticated,
    isAdmin,
    fullName,
    userId,
    // Actions
    initialize,
    setAuth,
    logout,
  }
})