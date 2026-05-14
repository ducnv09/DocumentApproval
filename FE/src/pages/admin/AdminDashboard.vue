<template>
  <div class="space-y-6">
    <!-- Header -->
    <div>
      <h2 class="text-xl font-bold text-slate-800">Bảng điều khiển quản trị</h2>
      <p class="text-sm text-slate-500 mt-1">Tổng quan hệ thống</p>
    </div>

    <!-- Stat Cards -->
    <div class="grid grid-cols-2 gap-3">
      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-blue-50 flex items-center justify-center">
            <i class="pi pi-users text-blue-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.totalUsers }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Tổng người dùng</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-violet-50 flex items-center justify-center">
            <i class="pi pi-file text-violet-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.totalDocuments }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Tổng tờ trình</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-amber-50 flex items-center justify-center">
            <i class="pi pi-clock text-amber-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.pendingDocuments }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Chờ xử lý</p>
      </div>

      <div class="bg-white rounded-xl p-4 shadow-sm border border-slate-200">
        <div class="flex items-center gap-2 mb-2">
          <div class="w-8 h-8 rounded-lg bg-green-50 flex items-center justify-center">
            <i class="pi pi-check-circle text-green-500 text-sm" />
          </div>
        </div>
        <p class="text-2xl font-bold text-slate-800">{{ stats.completedDocuments }}</p>
        <p class="text-xs text-slate-400 mt-0.5">Hoàn thành</p>
      </div>
    </div>

    <!-- Quick Admin Actions -->
    <div>
      <h3 class="text-sm font-semibold text-slate-600 mb-3">Quản lý nhanh</h3>
      <div class="flex flex-col gap-2">
        <router-link
          to="/admin/users"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-violet-200"
        >
          <div class="w-10 h-10 rounded-lg bg-blue-500 flex items-center justify-center shrink-0">
            <i class="pi pi-users text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Quản lý Tài khoản</p>
            <p class="text-xs text-slate-400 mt-0.5">Tạo, sửa, vô hiệu hóa tài khoản</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>

        <router-link
          to="/admin/groups"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-violet-200"
        >
          <div class="w-10 h-10 rounded-lg bg-emerald-500 flex items-center justify-center shrink-0">
            <i class="pi pi-sitemap text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Quản lý Nhóm & Vai trò</p>
            <p class="text-xs text-slate-400 mt-0.5">Phòng ban, chức danh, phân nhóm</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>

        <router-link
          to="/admin/workflows"
          class="flex items-center gap-3 bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 no-underline text-slate-700 transition-all active:scale-[0.98] hover:border-violet-200"
        >
          <div class="w-10 h-10 rounded-lg bg-violet-500 flex items-center justify-center shrink-0">
            <i class="pi pi-share-alt text-white" />
          </div>
          <div>
            <p class="font-semibold text-sm">Cấu hình Luồng Duyệt</p>
            <p class="text-xs text-slate-400 mt-0.5">Tuần tự, song song, nhóm duyệt</p>
          </div>
          <i class="pi pi-chevron-right text-slate-300 ml-auto text-xs" />
        </router-link>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'

// Placeholder stats - sẽ gọi API thực tế khi backend có endpoint
const stats = reactive({
  totalUsers: 0,
  totalDocuments: 0,
  pendingDocuments: 0,
  completedDocuments: 0,
})
</script>
