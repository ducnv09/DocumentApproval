import { createApp } from 'vue'
import { createPinia } from 'pinia'
import './style.css'
import './tailwind.css'
import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura' // Giao diện mới mặc định của v4
import ToastService from 'primevue/toastservice'
import 'primeicons/primeicons.css'

// Capacitor Plugins
import { SplashScreen } from '@capacitor/splash-screen'
import { StatusBar, Style } from '@capacitor/status-bar'
import { Keyboard } from '@capacitor/keyboard'
import { App as CapApp } from '@capacitor/app'

// Stores
import { useAuthStore } from './stores/authStore'
import { useAppStore } from './stores/appStore'

// ============================================================
// KHỞI TẠO ỨNG DỤNG
// ============================================================

const app = createApp(App)

// 1. Pinia (State Management) - Phải đăng ký TRƯỚC khi dùng stores
const pinia = createPinia()
app.use(pinia)

// 2. Vue Router
app.use(router)

// 3. PrimeVue UI Framework
app.use(PrimeVue, {
  theme: {
    preset: Aura,
    options: {
      darkModeSelector: '.my-app-dark',
    }
  }
})

// 4. PrimeVue Toast Service (dùng cho thông báo toàn cục)
app.use(ToastService)

// ============================================================
// KHỞI TẠO CAPACITOR PLUGINS & STORES
// ============================================================

async function initializeApp(): Promise<void> {
  try {
    // Khôi phục auth session từ native storage
    const authStore = useAuthStore()
    await authStore.initialize()

    // Khởi tạo network listener
    const appStore = useAppStore()
    await appStore.initNetworkListener()

    // Cấu hình Capacitor Plugins (chỉ chạy trên native platform)
    await configureCapacitorPlugins()

  } catch (error) {
    console.error('[App] Lỗi khởi tạo:', error)
  }
}

async function configureCapacitorPlugins(): Promise<void> {
  try {
    // Status Bar: Style tối cho nền sáng
    await StatusBar.setStyle({ style: Style.Light })
  } catch {
    // Bỏ qua lỗi khi chạy trên web (không có native StatusBar)
  }

  try {
    // Keyboard: Cuộn trang khi bàn phím xuất hiện
    Keyboard.addListener('keyboardWillShow', () => {
      document.body.classList.add('keyboard-open')
    })
    Keyboard.addListener('keyboardWillHide', () => {
      document.body.classList.remove('keyboard-open')
    })
  } catch {
    // Bỏ qua trên web
  }

  try {
    // Xử lý nút Back trên Android
    CapApp.addListener('backButton', ({ canGoBack }) => {
      if (canGoBack) {
        router.back()
      } else {
        CapApp.exitApp()
      }
    })
  } catch {
    // Bỏ qua trên web
  }

  try {
    // Ẩn Splash Screen sau khi app sẵn sàng
    await SplashScreen.hide()
  } catch {
    // Bỏ qua trên web
  }
}

// ============================================================
// MOUNT APP
// ============================================================

initializeApp().then(() => {
  app.mount('#app')
})