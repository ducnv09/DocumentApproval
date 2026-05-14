<template>
  <div class="flex flex-col min-h-dvh bg-slate-100">
    <!-- ==================== HEADER ==================== -->
    <header class="sticky top-0 z-50 bg-gradient-to-r from-slate-800 to-slate-700 shadow-lg safe-area-top">
      <div class="flex items-center justify-between px-4 py-2.5">
        <h1 class="text-slate-100 text-[17px] font-bold m-0 flex items-center gap-2 tracking-tight">
          <i class="pi pi-verified text-xl text-blue-400" />
          Phê Duyệt
        </h1>

        <div class="flex items-center gap-3">
          <span class="flex items-center gap-1.5 text-slate-400 text-xs font-medium">
            <i class="pi pi-user text-[11px]" />
            {{ authStore.fullName }}
          </span>
          <button
            class="bg-white/[0.08] border border-white/10 rounded-lg text-slate-400 px-2 py-1.5 cursor-pointer transition-all flex items-center hover:bg-red-500/15 hover:border-red-500/30 hover:text-red-300"
            @click="handleLogout"
            title="Đăng xuất"
          >
            <i class="pi pi-sign-out text-sm" />
          </button>
        </div>
      </div>
    </header>

    <!-- ==================== MAIN CONTENT ==================== -->
    <main class="flex-1 p-4 pb-[calc(4.5rem+env(safe-area-inset-bottom,0px))] overflow-y-auto">
      <router-view v-slot="{ Component }">
        <transition name="page-fade" mode="out-in">
          <component :is="Component" />
        </transition>
      </router-view>
    </main>

    <!-- ==================== BOTTOM NAVIGATION ==================== -->
    <nav class="fixed bottom-0 left-0 right-0 z-50 bg-white border-t border-slate-200 shadow-[0_-4px_16px_rgba(0,0,0,0.06)] safe-area-bottom">
      <!-- User Menu -->
      <div v-if="!isAdminSection" class="flex justify-around items-center">
        <button
          v-for="item in userNavItems"
          :key="item.name"
          class="nav-item flex flex-col items-center gap-0.5 py-1.5 px-3 border-0 bg-transparent cursor-pointer transition-colors rounded-lg min-w-[4rem] relative"
          :class="{ 'nav-item--active !text-blue-500': isActiveRoute(item) }"
          @click="navigateTo(item)"
        >
          <!-- Nút tạo mới nổi bật -->
          <template v-if="item.isFab">
            <div class="w-11 h-11 rounded-full bg-blue-500 flex items-center justify-center -mt-5 shadow-lg shadow-blue-500/30 border-[3px] border-white">
              <i class="pi pi-plus text-white text-lg" />
            </div>
            <span class="text-[10px] font-semibold tracking-wide text-slate-400">{{ item.label }}</span>
          </template>
          <template v-else>
            <i :class="item.icon" class="text-xl" />
            <span class="text-[10px] font-semibold tracking-wide">{{ item.label }}</span>
          </template>
        </button>
      </div>

      <!-- Admin Menu -->
      <div v-else class="flex justify-around items-center">
        <button
          v-for="item in adminNavItems"
          :key="item.name"
          class="nav-item flex flex-col items-center gap-0.5 py-1.5 px-3 border-0 bg-transparent cursor-pointer transition-colors rounded-lg min-w-[4rem] relative"
          :class="{
            'nav-item--active !text-violet-500': isActiveRoute(item),
            '!text-slate-400': !isActiveRoute(item),
          }"
          @click="navigateTo(item)"
        >
          <i :class="item.icon" class="text-xl" />
          <span class="text-[10px] font-semibold tracking-wide">{{ item.label }}</span>
        </button>
      </div>
    </nav>
  </div>
</template>

<script setup lang="ts">
import { computed } from 'vue'
import { useRouter, useRoute } from 'vue-router'
import { useAuthStore } from '../stores/authStore'
import { toastService } from '../utils/toastService'

interface NavItem {
  name: string
  to: string
  icon?: string
  label: string
  isFab?: boolean
  /** Route names mà item này match (cho trường hợp parent/child routes) */
  matchNames?: string[]
}

const router = useRouter()
const route = useRoute()
const authStore = useAuthStore()

// ---- Detect xem đang ở Admin section không ----
const isAdminSection = computed(() => {
  return route.path.startsWith('/admin')
})

// ---- User Navigation Items ----
const userNavItems = computed<NavItem[]>(() => {
  const items: NavItem[] = [
    {
      name: 'Dashboard',
      to: '/',
      icon: 'pi pi-home',
      label: 'Trang chủ',
      matchNames: ['Dashboard'],
    },
    {
      name: 'DocumentList',
      to: '/documents',
      icon: 'pi pi-file',
      label: 'Tờ trình',
      matchNames: ['DocumentList', 'DocumentDetail'],
    },
    {
      name: 'DocumentCreate',
      to: '/documents/create',
      label: 'Tạo mới',
      isFab: true,
      matchNames: ['DocumentCreate'],
    },
    {
      name: 'ApprovalList',
      to: '/approvals',
      icon: 'pi pi-check-circle',
      label: 'Phê duyệt',
      matchNames: ['ApprovalList'],
    },
    {
      name: 'Profile',
      to: '/profile',
      icon: 'pi pi-user',
      label: 'Tài khoản',
      matchNames: ['Profile'],
    },
  ]
  return items
})

// ---- Admin Navigation Items ----
const adminNavItems = computed<NavItem[]>(() => [
  {
    name: 'AdminDashboard',
    to: '/admin',
    icon: 'pi pi-chart-bar',
    label: 'Tổng quan',
    matchNames: ['AdminDashboard'],
  },
  {
    name: 'AdminUsers',
    to: '/admin/users',
    icon: 'pi pi-users',
    label: 'Tài khoản',
    matchNames: ['AdminUsers'],
  },
  {
    name: 'AdminGroups',
    to: '/admin/groups',
    icon: 'pi pi-sitemap',
    label: 'Nhóm',
    matchNames: ['AdminGroups'],
  },
  {
    name: 'AdminWorkflows',
    to: '/admin/workflows',
    icon: 'pi pi-share-alt',
    label: 'Luồng duyệt',
    matchNames: ['AdminWorkflows'],
  },
  {
    name: 'BackToUser',
    to: '/',
    icon: 'pi pi-arrow-left',
    label: 'Quay lại',
    matchNames: [],
  },
])

// ---- Active Route Detection (dùng route name thay vì path prefix) ----
function isActiveRoute(item: NavItem): boolean {
  const currentName = route.name as string
  return item.matchNames?.includes(currentName) ?? false
}

// ---- Navigate ----
function navigateTo(item: NavItem) {
  router.push(item.to)
}

// ---- Logout ----
async function handleLogout() {
  await authStore.logout()
  toastService.info('Đã đăng xuất.')
  router.replace({ name: 'Login' })
}
</script>

<style scoped>
/* Safe area padding - không có trong Tailwind */
.safe-area-top {
  padding-top: env(safe-area-inset-top, 0);
}
.safe-area-bottom {
  padding-bottom: calc(0.375rem + env(safe-area-inset-bottom, 0));
  padding-top: 0.375rem;
}

/* Active indicator dot */
.nav-item--active::before {
  content: '';
  position: absolute;
  top: -0.375rem;
  left: 50%;
  transform: translateX(-50%);
  width: 1.25rem;
  height: 3px;
  background: currentColor;
  border-radius: 0 0 3px 3px;
}

.nav-item:active {
  transform: scale(0.92);
}

/* Page transition */
.page-fade-enter-active,
.page-fade-leave-active {
  transition: opacity 0.2s ease;
}
.page-fade-enter-from,
.page-fade-leave-to {
  opacity: 0;
}
</style>