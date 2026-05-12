import type { Router } from 'vue-router'
import { useAuthStore } from '../stores/authStore'

// ============================================================
// NAVIGATION GUARDS
// ============================================================

/**
 * Đăng ký guard bảo vệ route.
 * Tách riêng ra file để router/index.ts gọn gàng hơn.
 */
export function setupRouterGuards(router: Router): void {
  router.beforeEach(async (to, _from, next) => {
    const authStore = useAuthStore()

    // Đảm bảo auth store đã khôi phục session từ Capacitor Preferences
    if (!authStore.isInitialized) {
      await authStore.initialize()
    }

    const isAuthenticated = authStore.isAuthenticated
    const requiresAuth = to.meta.requiresAuth as boolean | undefined
    const requiresAdmin = to.meta.requiresAdmin as boolean | undefined
    const isGuestOnly = to.meta.guestOnly as boolean | undefined

    // 1. Route yêu cầu đăng nhập mà chưa có token → Đẩy về Login
    if (requiresAuth && !isAuthenticated) {
      return next({ name: 'Login', query: { redirect: to.fullPath } })
    }

    // 2. Route chỉ dành cho Admin mà user không phải Admin → Đẩy về Dashboard
    if (requiresAdmin && !authStore.isAdmin) {
      return next({ name: 'Dashboard' })
    }

    // 3. Route chỉ dành cho guest (Login, Register) mà đã đăng nhập → Đẩy vào Dashboard
    if (isGuestOnly && isAuthenticated) {
      return next({ name: 'Dashboard' })
    }

    // 4. Cho phép đi tiếp
    next()
  })
}
