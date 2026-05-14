import { createRouter, createWebHistory, createWebHashHistory, type RouteRecordRaw } from 'vue-router'
import { Capacitor } from '@capacitor/core'
import { setupRouterGuards } from './guards'

// ============================================================
// ROUTE DEFINITIONS
// ============================================================

const routes: RouteRecordRaw[] = [
  // ---- Auth Routes (Guest Only) ----
  {
    path: '/login',
    name: 'Login',
    component: () => import('../pages/auth/Login.vue'),
    meta: { guestOnly: true },
  },

  // ---- Main App Routes (Requires Auth) ----
  {
    path: '/',
    component: () => import('../layouts/MainLayout.vue'),
    meta: { requiresAuth: true },
    children: [
      // ========================================
      // USER MENU
      // ========================================
      {
        path: '',
        name: 'Dashboard',
        component: () => import('../pages/dashboard/Dashboard.vue'),
      },

      // --- Tờ trình ---
      {
        path: 'documents',
        name: 'DocumentList',
        component: () => import('../pages/documents/DocumentList.vue'),
      },
      {
        path: 'documents/create',
        name: 'DocumentCreate',
        component: () => import('../pages/documents/DocumentCreate.vue'),
      },
      {
        path: 'documents/:id',
        name: 'DocumentDetail',
        component: () => import('../pages/documents/DocumentDetail.vue'),
        props: true,
      },

      // --- Phê duyệt ---
      {
        path: 'approvals',
        name: 'ApprovalList',
        component: () => import('../pages/approvals/ApprovalHistory.vue'),
      },

      // --- Tài khoản cá nhân ---
      {
        path: 'profile',
        name: 'Profile',
        component: () => import('../pages/profile/Profile.vue'),
      },

      // ========================================
      // ADMIN MENU (chỉ dành cho Admin)
      // ========================================
      {
        path: 'admin',
        name: 'AdminDashboard',
        component: () => import('../pages/admin/AdminDashboard.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'admin/users',
        name: 'AdminUsers',
        component: () => import('../pages/admin/Users.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'admin/groups',
        name: 'AdminGroups',
        component: () => import('../pages/admin/Groups.vue'),
        meta: { requiresAdmin: true },
      },
      {
        path: 'admin/workflows',
        name: 'AdminWorkflows',
        component: () => import('../pages/admin/Workflows.vue'),
        meta: { requiresAdmin: true },
      },
    ],
  },

  // ---- Catch-all: Redirect về Dashboard ----
  {
    path: '/:pathMatch(.*)*',
    redirect: { name: 'Dashboard' },
  },
]

// ============================================================
// KHỞI TẠO ROUTER
// ============================================================

const router = createRouter({
  history: Capacitor.isNativePlatform()
    ? createWebHashHistory()
    : createWebHistory(),
  routes,
  scrollBehavior(_to, _from, savedPosition) {
    // Giữ vị trí scroll khi quay lại trang trước
    return savedPosition ?? { top: 0 }
  },
})

// Đăng ký Navigation Guards
setupRouterGuards(router)

export default router