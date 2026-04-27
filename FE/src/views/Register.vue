<template>
  <div class="flex justify-center items-center min-h-screen bg-gray-50 p-5">
    <div class="bg-white p-8 rounded-2xl shadow-[0_8px_30px_rgb(0,0,0,0.08)] w-full max-w-md">
      <h2 class="text-2xl font-bold text-center text-gray-800 mb-6">
        Đăng Ký Tài Khoản
      </h2>
      
      <!-- Form Container -->
      <div class="flex flex-col gap-6">
        <!-- Email Field -->
        <div class="flex flex-col gap-2 text-left">
          <label for="email" class="text-xs font-bold text-gray-600 uppercase tracking-wider">Email</label>
          <div class="relative">
            <i class="pi pi-envelope absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 z-10"></i>
            <InputText id="email" v-model="email" placeholder="you@example.com" class="w-full !pl-10 py-3 rounded-xl border-gray-200 focus:border-teal-500 focus:ring-teal-500 transition-colors" />
          </div>
          <small class="text-red-500 font-medium" v-if="errors.email">{{ errors.email }}</small>
        </div>

        <!-- Password Field -->
        <div class="flex flex-col gap-2 text-left">
          <label for="password" class="text-xs font-bold text-gray-600 uppercase tracking-wider">Mật khẩu</label>
          <div class="relative">
            <i class="pi pi-lock absolute left-3 top-1/2 -translate-y-1/2 text-gray-400 z-10"></i>
            <Password id="password" v-model="password" placeholder="••••••••" :feedback="false" toggleMask class="w-full" inputClass="w-full !pl-10 py-3 rounded-xl border-gray-200 focus:border-teal-500 focus:ring-teal-500 transition-colors" />
          </div>
          <small class="text-red-500 font-medium" v-if="errors.password">{{ errors.password }}</small>
        </div>

        <!-- Server Error -->
        <div v-if="serverError" class="text-sm text-red-600 text-center bg-red-50 p-3 rounded-lg border border-red-100">
          {{ serverError }}
        </div>

        <!-- Submit Button -->
        <Button label="Đăng Ký" class="w-full py-3.5 mt-2 font-bold rounded-xl bg-teal-500 border-none hover:bg-teal-600 transition-colors" @click="handleRegister" :loading="isLoading" />
        
        <!-- Toggle Mode -->
        <div class="text-center mt-2 text-sm text-gray-600">
          Đã có tài khoản?
          <router-link to="/login" class="text-teal-600 font-bold hover:underline transition-all">
            Đăng nhập
          </router-link>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import InputText from 'primevue/inputtext';
import Password from 'primevue/password';
import Button from 'primevue/button';

const router = useRouter();

// Trạng thái dữ liệu
const email = ref('');
const password = ref('');
const errors = ref<Record<string, string>>({});
const serverError = ref('');
const isLoading = ref(false);

// Hàm kiểm tra tính hợp lệ
const validateForm = () => {
  errors.value = {};
  let isValid = true;
  
  if (!email.value) {
    errors.value.email = 'Email không được để trống';
    isValid = false;
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(email.value)) {
    errors.value.email = 'Email không hợp lệ';
    isValid = false;
  }
  
  if (!password.value) {
    errors.value.password = 'Mật khẩu không được để trống';
    isValid = false;
  }
  
  return isValid;
};

// Hàm xử lý Đăng ký
const handleRegister = async () => {
  if (!validateForm()) return;

  isLoading.value = true;
  serverError.value = '';

  try {
    const apiUrl = `https://192.168.5.115:7092/api/auth/register`;

    const response = await fetch(apiUrl, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email: email.value, password: password.value })
    });

    const data = await response.json();

    if (response.ok) {
      alert('Đăng ký thành công! Vui lòng đăng nhập.');
      router.push('/login');
    } else {
      serverError.value = data.message || 'Đăng ký thất bại!';
    }
  } catch (error) {
    serverError.value = 'Không thể kết nối đến máy chủ!';
  } finally {
    isLoading.value = false;
  }
};
</script>
