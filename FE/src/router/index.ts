import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import Login from '../views/Login.vue';

const routes = [
  { 
    path: '/login', 
    name: 'Login', 
    component: Login 
  },
  { 
    path: '/register', 
    name: 'Register', 
    component: () => import('../views/Register.vue')
  },
  { 
    path: '/', 
    name: 'Home', 
    component: Home,
    meta: { requiresAuth: true } // Đánh dấu route này cần đăng nhập
  }
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

// Điều hướng (Navigation Guard)
router.beforeEach((to, _from, next) => {
  // Lấy token từ bộ nhớ thiết bị
  const token = localStorage.getItem('app_token');

  if (to.meta.requiresAuth && !token) {
    // Nếu trang cần đăng nhập mà chưa có token -> Đẩy về màn Login
    next('/login');
  } else if ((to.path === '/login' || to.path === '/register') && token) {
    // Nếu có token rồi mà đòi vào Login/Register -> Đẩy thẳng vào Home
    next('/');
  } else {
    // Cho phép đi tiếp
    next();
  }
});

export default router;