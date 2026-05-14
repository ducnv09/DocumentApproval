<template>
  <div class="space-y-6">
    <!-- Greeting -->
    <div>
      <h2 class="text-xl font-bold text-slate-800">
        Xin chào, {{ authStore.fullName }}
      </h2>
      <p class="text-sm text-slate-500 mt-1">Tổng quan hoạt động của bạn</p>
    </div>

    <!-- Stat Cards -->
    <div class="grid grid-cols-2 gap-3">
      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-amber-50 flex items-center justify-center">
            <i class="pi pi-clock text-amber-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.pending }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Chờ duyệt</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-green-50 flex items-center justify-center">
            <i class="pi pi-check-circle text-green-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.approved }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Đã duyệt</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-red-50 flex items-center justify-center">
            <i class="pi pi-times-circle text-red-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.rejected }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Từ chối</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-blue-50 flex items-center justify-center">
            <i class="pi pi-file text-blue-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.total }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Tổng tờ trình</p>
      </div>
    </div>

    <!-- Quick Actions -->
    <div>
      <h3 class="text-sm font-semibold text-slate-600 mb-3">Thao tác nhanh</h3>
      <div class="flex flex-col gap-2">
        <router-link
          to="/documents/create"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-blue-200"
        >
          <div class="w-10 h-10 rounded-lg bg-blue-500 flex items-center justify-center shrink-0">
            <i class="pi pi-plus text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Tạo tờ trình mới</p>
            <p class="text-xs text-slate-400 mt-0.5">Soạn và gửi phê duyệt</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>

        <router-link
          to="/approvals"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-blue-200"
        >
          <div class="w-10 h-10 rounded-lg bg-emerald-500 flex items-center justify-center shrink-0">
            <i class="pi pi-check-circle text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Phê duyệt tờ trình</p>
            <p class="text-xs text-slate-400 mt-0.5">Xem danh sách chờ ký</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>

        <router-link
          v-if="authStore.isAdmin"
          to="/admin"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-violet-200"
        >
          <div class="w-10 h-10 rounded-lg bg-violet-500 flex items-center justify-center shrink-0">
            <i class="pi pi-cog text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Quản trị hệ thống</p>
            <p class="text-xs text-slate-400 mt-0.5">Nhân sự, phòng ban, luồng duyệt</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useAuthStore } from '../../stores/authStore'

const authStore = useAuthStore()

// Placeholder stats - sẽ gọi API thực tế khi backend có endpoint
const stats = reactive({
  pending: 0,
  approved: 0,
  rejected: 0,
  total: 0,
})
</script>
