<template>
  <!-- PrimeVue Toast - hiển thị thông báo toàn cục -->
  <Toast position="top-center" />

  <router-view></router-view>
</template>

<script setup lang="ts">
import { onMounted, onUnmounted } from 'vue'
import Toast from 'primevue/toast'
import { useToast } from 'primevue/usetoast'
import { onToast } from './utils/toastService'

// Kết nối toastService (headless) → PrimeVue Toast (UI)
const toast = useToast()
let unsubscribe: (() => void) | null = null

onMounted(() => {
  unsubscribe = onToast((event) => {
    toast.add(event)
  })
})

onUnmounted(() => {
  unsubscribe?.()
})
</script>

<style>
  body {
    margin: 0;
    padding: 0;
    font-family: -apple-system, BlinkMacSystemFont, "Segoe UI", Roboto, Helvetica, Arial, sans-serif;
  }
</style>