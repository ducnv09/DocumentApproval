<template>
  <div class="flex justify-center items-center min-h-screen bg-gray-50 p-5">
    <div class="bg-white p-8 rounded-3xl shadow-[0_8px_30px_rgb(0,0,0,0.08)] w-full max-w-sm flex flex-col items-center">
      
      <!-- Avatar Placeholder -->
      <div class="w-24 h-24 rounded-full bg-teal-100 flex items-center justify-center mb-5 shadow-sm border-4 border-white">
        <i class="pi pi-user text-teal-500 text-4xl"></i>
      </div>

      <!-- Welcome Text -->
      <h2 class="text-2xl font-bold text-gray-800 mb-1 text-center">
        Xin chào, {{ userInfo.name }}!
      </h2>
      
      <!-- Email Badge -->
      <div class="flex items-center gap-2 bg-gray-50 px-4 py-2 rounded-full mb-8 mt-2 border border-gray-100">
        <i class="pi pi-envelope text-gray-400 text-sm"></i>
        <span class="text-gray-600 text-sm font-medium">{{ userInfo.email }}</span>
      </div>
      
      <!-- Logout Button -->
      <Button 
        label="Đăng xuất" 
        icon="pi pi-sign-out" 
        class="w-full py-3.5 font-bold rounded-xl bg-red-50 text-red-600 border-none hover:bg-red-100 transition-colors shadow-none" 
        @click="handleLogout" 
      />
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import Button from 'primevue/button';

const router = useRouter();
const userInfo = ref({ name: 'User', email: 'Đang tải...' });

// Hàm decode JWT chuẩn để lấy Payload
const parseJwt = (token: string) => {
  try {
    const base64Url = token.split('.')[1];
    const base64 = base64Url.replace(/-/g, '+').replace(/_/g, '/');
    const jsonPayload = decodeURIComponent(window.atob(base64).split('').map(function(c) {
        return '%' + ('00' + c.charCodeAt(0).toString(16)).slice(-2);
    }).join(''));
    return JSON.parse(jsonPayload);
  } catch (e) {
    return null;
  }
};

onMounted(() => {
  const token = localStorage.getItem('app_token');
  if (token) {
    const decodedData = parseJwt(token);
    if (decodedData) {
      // Tuỳ vào cách C# ASP.NET map claim, key có thể khác nhau.
      userInfo.value.name = decodedData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name'] || decodedData.name || 'Người dùng';
      userInfo.value.email = decodedData['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/email'] || decodedData.email || 'No email';
    }
  }
});

const handleLogout = () => {
  // Xóa token khỏi bộ nhớ thiết bị
  localStorage.removeItem('app_token');
  // Đẩy về màn hình đăng nhập
  router.push('/login');
};
</script>