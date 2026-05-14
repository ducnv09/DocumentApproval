<template>
  <div class="space-y-4">
    <div class="flex items-center justify-between">
      <div>
        <h2 class="text-lg font-bold text-slate-800">Danh sách tờ trình</h2>
        <p class="text-xs text-slate-400 mt-0.5">Tất cả tờ trình của bạn</p>
      </div>
      <router-link
        to="/documents/create"
        class="flex items-center gap-1.5 bg-blue-500 text-white text-sm font-semibold px-3.5 py-2 rounded-lg no-underline hover:bg-blue-600 active:scale-95 transition-all"
      >
        <i class="pi pi-plus text-xs" /> Tạo mới
      </router-link>
    </div>

    <!-- Empty state -->
    <div class="bg-white rounded-xl border border-slate-200 shadow-sm p-8 text-center">
      <div class="w-16 h-16 mx-auto mb-4 rounded-full bg-blue-50 flex items-center justify-center">
        <i class="pi pi-file text-2xl text-blue-400" />
      </div>
      <h3 class="text-sm font-semibold text-slate-600 mb-1">Chưa có tờ trình</h3>
      <p class="text-xs text-slate-400">Bấm "Tạo mới" để soạn tờ trình đầu tiên.</p>
    </div>
  </div>
</template>
