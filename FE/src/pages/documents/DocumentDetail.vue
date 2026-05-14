<template>
  <div class="space-y-4">
    <h2 class="text-lg font-bold text-slate-800">Chi tiết tờ trình</h2>
    <div class="bg-white rounded-xl border border-slate-200 shadow-sm p-8 text-center">
      <div class="w-16 h-16 mx-auto mb-4 rounded-full bg-slate-100 flex items-center justify-center">
        <i class="pi pi-file-check text-2xl text-slate-400" />
      </div>
      <p class="text-xs text-slate-400">Sẽ triển khai sau khi hoàn thiện backend Document API.</p>
    </div>
  </div>
</template>

<script setup lang="ts">
defineProps<{ id: string }>()
</script>
