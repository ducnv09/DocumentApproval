<template>
  <div class="min-h-dvh flex flex-col items-center justify-center p-6 bg-gradient-to-br from-slate-900 via-slate-800 to-slate-700">
    <!-- Card -->
    <div class="w-full max-w-[400px] login-card rounded-2xl p-8 shadow-2xl">
      <!-- Header -->
      <div class="text-center mb-8">
        <div class="w-16 h-16 mx-auto mb-4 rounded-full bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center shadow-lg shadow-blue-500/35">
          <i class="pi pi-verified text-3xl text-white" />
        </div>
        <h1 class="text-slate-100 text-2xl font-bold tracking-tight">Phê Duyệt Tờ Trình</h1>
        <p class="text-slate-400 text-sm mt-1">Đăng nhập để tiếp tục</p>
      </div>

      <!-- Form -->
      <form class="flex flex-col gap-5" @submit.prevent="handleLogin">
        <!-- Username -->
        <div class="flex flex-col gap-2">
          <label for="login-username" class="text-slate-300 text-[13px] font-medium flex items-center gap-1.5">
            <i class="pi pi-user text-xs text-slate-500" /> Tên đăng nhập
          </label>
          <input
            id="login-username"
            v-model="form.username"
            type="text"
            class="w-full px-4 py-3 bg-white/[0.07] border border-white/[0.12] rounded-xl text-slate-100 text-[15px] placeholder-slate-500 outline-none transition-all focus:border-blue-500 focus:ring-[3px] focus:ring-blue-500/15 disabled:opacity-50 disabled:cursor-not-allowed"
            placeholder="Nhập tên đăng nhập"
            autocomplete="username"
            :disabled="isSubmitting"
            required
          />
        </div>

        <!-- Password -->
        <div class="flex flex-col gap-2">
          <label for="login-password" class="text-slate-300 text-[13px] font-medium flex items-center gap-1.5">
            <i class="pi pi-lock text-xs text-slate-500" /> Mật khẩu
          </label>
          <div class="relative">
            <input
              id="login-password"
              v-model="form.password"
              :type="showPassword ? 'text' : 'password'"
              class="w-full px-4 py-3 pr-12 bg-white/[0.07] border border-white/[0.12] rounded-xl text-slate-100 text-[15px] placeholder-slate-500 outline-none transition-all focus:border-blue-500 focus:ring-[3px] focus:ring-blue-500/15 disabled:opacity-50 disabled:cursor-not-allowed"
              placeholder="Nhập mật khẩu"
              autocomplete="current-password"
              :disabled="isSubmitting"
              required
            />
            <button
              type="button"
              class="absolute right-3 top-1/2 -translate-y-1/2 bg-transparent border-none text-slate-500 cursor-pointer p-1 text-base transition-colors hover:text-slate-300"
              @click="showPassword = !showPassword"
              tabindex="-1"
            >
              <i :class="showPassword ? 'pi pi-eye-slash' : 'pi pi-eye'" />
            </button>
          </div>
        </div>

        <!-- Error -->
        <div v-if="errorMessage" class="flex items-center gap-2 px-4 py-3 bg-red-500/10 border border-red-500/25 rounded-xl text-red-300 text-[13px]">
          <i class="pi pi-exclamation-circle text-red-500 shrink-0" />
          {{ errorMessage }}
        </div>

        <!-- Submit -->
        <button
          id="login-submit"
          type="submit"
          class="flex items-center justify-center gap-2 py-3.5 bg-gradient-to-r from-blue-500 to-blue-600 text-white text-[15px] font-semibold border-none rounded-xl cursor-pointer transition-all mt-2 hover:opacity-90 active:scale-[0.98] disabled:opacity-40 disabled:cursor-not-allowed"
          :disabled="isSubmitting || !isFormValid"
        >
          <i v-if="isSubmitting" class="pi pi-spin pi-spinner" />
          <i v-else class="pi pi-sign-in" />
          {{ isSubmitting ? 'Đang đăng nhập...' : 'Đăng nhập' }}
        </button>
      </form>
    </div>

    <!-- Footer -->
    <p class="mt-8 text-slate-500 text-xs text-center">© 2026 Venus — Hệ thống quản lý phê duyệt</p>
  </div>
</template>

<script setup lang="ts">
import { ref, computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../../stores/authStore'
import { login } from '../../features/auth/auth.api'
import { toastService } from '../../utils/toastService'

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

const form = ref({ username: '', password: '' })
const showPassword = ref(false)
const isSubmitting = ref(false)
const errorMessage = ref('')

const isFormValid = computed(() =>
  form.value.username.trim().length > 0 && form.value.password.length > 0
)

async function handleLogin() {
  if (!isFormValid.value || isSubmitting.value) return

  isSubmitting.value = true
  errorMessage.value = ''

  try {
    const response = await login({
      username: form.value.username.trim(),
      password: form.value.password,
    })

    await authStore.setAuth(response)
    toastService.success(`Xin chào, ${response.fullName}!`, 'Đăng nhập thành công')

    const redirectTo = (route.query.redirect as string) || '/'
    await router.replace(redirectTo)
  } catch (error: any) {
    const apiMsg = error?.response?.data?.message
    errorMessage.value = apiMsg || 'Sai tên đăng nhập hoặc mật khẩu.'
  } finally {
    isSubmitting.value = false
  }
}
</script>

<style scoped>
/* Glassmorphism - Tailwind không hỗ trợ backdrop-filter custom rgba */
.login-card {
  background: rgba(255, 255, 255, 0.05);
  backdrop-filter: blur(24px);
  -webkit-backdrop-filter: blur(24px);
  border: 1px solid rgba(255, 255, 255, 0.1);
}
</style>
