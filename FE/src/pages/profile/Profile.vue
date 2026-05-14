<template>
  <div class="space-y-6">
    <!-- User Info Card -->
    <div class="bg-white rounded-xl border border-slate-200 shadow-sm p-5">
      <div class="flex items-center gap-4">
        <div class="w-16 h-16 rounded-full bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center shrink-0">
          <span class="text-white text-xl font-bold">{{ initials }}</span>
        </div>
        <div class="flex-1 min-w-0">
          <h2 class="text-lg font-bold text-slate-800 truncate">{{ authStore.fullName }}</h2>
          <p class="text-sm text-slate-400 mt-0.5">{{ authStore.user?.email || 'Chưa có email' }}</p>
          <span
            v-if="authStore.isAdmin"
            class="inline-flex items-center gap-1 mt-1.5 px-2 py-0.5 rounded-full bg-violet-50 text-violet-600 text-[10px] font-semibold"
          >
            <i class="pi pi-shield text-[9px]" />
            Quản trị viên
          </span>
        </div>
      </div>
    </div>

    <!-- Menu Items -->
    <div class="space-y-2">
      <!-- Admin Panel -->
      <router-link
        v-if="authStore.isAdmin"
        to="/admin"
        class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-violet-200"
      >
        <div class="w-10 h-10 rounded-lg bg-violet-50 flex items-center justify-center shrink-0">
          <i class="pi pi-cog text-violet-500" />
        </div>
        <div class="flex-1">
          <p class="font-semibold text-sm">Quản trị hệ thống</p>
          <p class="text-xs text-slate-400 mt-0.5">Tài khoản, nhóm, luồng duyệt</p>
        </div>
        <i class="pi pi-chevron-right text-slate-300 text-xs" />
      </router-link>

      <!-- Logout -->
      <button
        class="w-full flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-red-100 text-red-500 transition-all active:scale-[0.98] hover:bg-red-50 cursor-pointer"
        @click="handleLogout"
      >
        <div class="w-10 h-10 rounded-lg bg-red-50 flex items-center justify-center shrink-0">
          <i class="pi pi-sign-out text-red-500" />
        </div>
        <p class="font-semibold text-sm">Đăng xuất</p>
      </button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter } from 'vue-router'
import { useAuthStore } from '../../stores/authStore'
import { toastService } from '../../utils/toastService'

const router = useRouter()
const authStore = useAuthStore()

const initials = computed(() => {
  const name = authStore.fullName
  if (!name) {
    return '?'
  }
  const parts = name.split(' ').filter(Boolean)
  if (parts.length >= 2) {
    return (parts[0][0] + parts[parts.length - 1][0]).toUpperCase()
  }
  return name[0].toUpperCase()
})

async function handleLogout() {
  await authStore.logout()
  toastService.info('Đã đăng xuất.')
  router.replace({ name: 'Login' })
}
</script>
