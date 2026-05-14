<template>
  <div class="space-y-4">
    <!-- Header -->
    <div class="flex items-center justify-between">
      <div>
        <h2 class="text-lg font-bold text-slate-800">Quản lý nhân sự</h2>
        <p class="text-xs text-slate-400 mt-0.5">{{ users.length }} người dùng</p>
      </div>
      <button
        class="flex items-center gap-1.5 bg-blue-500 text-white text-sm font-semibold px-3.5 py-2 rounded-lg border-none cursor-pointer transition-all hover:bg-blue-600 active:scale-95"
        @click="openCreateDialog"
      >
        <i class="pi pi-plus text-xs" />
        Thêm
      </button>
    </div>

    <!-- Search -->
    <div class="relative">
      <i class="pi pi-search absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 text-sm" />
      <input
        v-model="searchQuery"
        type="text"
        placeholder="Tìm kiếm theo tên, email..."
        class="w-full pl-9 pr-4 py-2.5 bg-white border border-slate-200 rounded-xl text-sm text-slate-700 placeholder-slate-400 outline-none transition-all focus:border-blue-400 focus:ring-2 focus:ring-blue-500/10"
      />
    </div>

    <!-- Loading -->
    <div v-if="isLoading" class="flex justify-center py-12">
      <i class="pi pi-spin pi-spinner text-2xl text-blue-500" />
    </div>

    <!-- User List -->
    <div v-else class="flex flex-col gap-2.5">
      <div v-if="filteredUsers.length === 0" class="text-center py-12 text-slate-400 text-sm">
        <i class="pi pi-users text-3xl mb-2 block" />
        Không tìm thấy người dùng nào
      </div>

      <div
        v-for="user in filteredUsers"
        :key="user.id"
        class="bg-white rounded-xl px-4 py-3.5 shadow-sm border border-slate-200 flex items-center gap-3"
      >
        <!-- Avatar -->
        <div class="w-10 h-10 rounded-full bg-gradient-to-br from-blue-500 to-violet-500 flex items-center justify-center shrink-0">
          <span class="text-white text-sm font-bold">{{ getInitials(user.fullName) }}</span>
        </div>

        <!-- Info -->
        <div class="flex-1 min-w-0">
          <div class="flex items-center gap-2">
            <p class="font-semibold text-sm text-slate-800 truncate">{{ user.fullName }}</p>
            <span
              v-if="user.isAdmin"
              class="text-[10px] font-bold px-1.5 py-0.5 rounded bg-violet-100 text-violet-600"
            >ADMIN</span>
          </div>
          <p class="text-xs text-slate-400 truncate mt-0.5">{{ user.email }}</p>
        </div>

        <!-- Status badge -->
        <span
          class="text-[10px] font-semibold px-2 py-1 rounded-full shrink-0"
          :class="user.isActive ? 'bg-green-50 text-green-600' : 'bg-red-50 text-red-500'"
        >
          {{ user.isActive ? 'Hoạt động' : 'Đã khóa' }}
        </span>

        <!-- Actions -->
        <button
          class="p-1.5 text-slate-400 hover:text-blue-500 transition-colors cursor-pointer bg-transparent border-none"
          @click="openEditDialog(user)"
        >
          <i class="pi pi-pencil text-sm" />
        </button>
      </div>
    </div>

    <!-- ==================== CREATE DIALOG ==================== -->
    <div v-if="showCreateDialog" class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center" @click.self="showCreateDialog = false">
      <div class="absolute inset-0 bg-black/50" @click="showCreateDialog = false" />
      <div class="relative bg-white w-full sm:max-w-md sm:rounded-2xl rounded-t-2xl p-6 z-10 max-h-[90vh] overflow-y-auto">
        <div class="flex items-center justify-between mb-5">
          <h3 class="text-base font-bold text-slate-800">Thêm người dùng</h3>
          <button class="text-slate-400 hover:text-slate-600 bg-transparent border-none cursor-pointer" @click="showCreateDialog = false">
            <i class="pi pi-times" />
          </button>
        </div>

        <form class="flex flex-col gap-4" @submit.prevent="handleCreate">
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Tên đăng nhập *</label>
            <input v-model="createForm.username" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Mật khẩu *</label>
            <input v-model="createForm.password" type="password" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Họ và tên *</label>
            <input v-model="createForm.fullName" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Email *</label>
            <input v-model="createForm.email" type="email" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <label class="flex items-center gap-2 text-sm text-slate-700 cursor-pointer">
            <input v-model="createForm.isAdmin" type="checkbox" class="accent-blue-500 w-4 h-4" />
            Quyền Admin
          </label>

          <button
            type="submit"
            class="w-full py-2.5 bg-blue-500 text-white text-sm font-semibold rounded-lg border-none cursor-pointer transition-all hover:bg-blue-600 active:scale-[0.98] disabled:opacity-50"
            :disabled="isSaving"
          >
            <i v-if="isSaving" class="pi pi-spin pi-spinner mr-1" />
            {{ isSaving ? 'Đang lưu...' : 'Tạo người dùng' }}
          </button>
        </form>
      </div>
    </div>

    <!-- ==================== EDIT DIALOG ==================== -->
    <div v-if="showEditDialog" class="fixed inset-0 z-[100] flex items-end sm:items-center justify-center" @click.self="showEditDialog = false">
      <div class="absolute inset-0 bg-black/50" @click="showEditDialog = false" />
      <div class="relative bg-white w-full sm:max-w-md sm:rounded-2xl rounded-t-2xl p-6 z-10 max-h-[90vh] overflow-y-auto">
        <div class="flex items-center justify-between mb-5">
          <h3 class="text-base font-bold text-slate-800">Sửa thông tin</h3>
          <button class="text-slate-400 hover:text-slate-600 bg-transparent border-none cursor-pointer" @click="showEditDialog = false">
            <i class="pi pi-times" />
          </button>
        </div>

        <form class="flex flex-col gap-4" @submit.prevent="handleUpdate">
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Họ và tên</label>
            <input v-model="editForm.fullName" type="text" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <div class="flex flex-col gap-1.5">
            <label class="text-xs font-medium text-slate-600">Email</label>
            <input v-model="editForm.email" type="email" required class="w-full px-3 py-2.5 border border-slate-200 rounded-lg text-sm outline-none focus:border-blue-500 focus:ring-2 focus:ring-blue-500/10" />
          </div>
          <label class="flex items-center gap-2 text-sm text-slate-700 cursor-pointer">
            <input v-model="editForm.isActive" type="checkbox" class="accent-green-500 w-4 h-4" />
            Đang hoạt động
          </label>

          <div class="flex gap-2">
            <button
              type="submit"
              class="flex-1 py-2.5 bg-blue-500 text-white text-sm font-semibold rounded-lg border-none cursor-pointer hover:bg-blue-600 disabled:opacity-50"
              :disabled="isSaving"
            >
              {{ isSaving ? 'Đang lưu...' : 'Cập nhật' }}
            </button>
            <button
              type="button"
              class="py-2.5 px-4 bg-red-50 text-red-500 text-sm font-semibold rounded-lg border border-red-200 cursor-pointer hover:bg-red-100 disabled:opacity-50"
              :disabled="isSaving"
              @click="handleDelete"
            >
              <i class="pi pi-trash text-xs" />
            </button>
          </div>
        </form>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
import { ref, computed, onMounted } from 'vue'
import { getUsers, createUser, updateUser, deleteUser } from '../../features/admin/admin.api'
import type { UserDto, CreateUserDto, UpdateUserDto } from '../../features/admin/admin.types'
import { toastService } from '../../utils/toastService'

// ---- State ----
const users = ref<UserDto[]>([])
const searchQuery = ref('')
const isLoading = ref(false)
const isSaving = ref(false)

const showCreateDialog = ref(false)
const showEditDialog = ref(false)
const editingUserId = ref<string | null>(null)

const createForm = ref<CreateUserDto>({ username: '', password: '', fullName: '', email: '', isAdmin: false })
const editForm = ref<UpdateUserDto>({ fullName: '', email: '', isActive: true })

// ---- Computed ----
const filteredUsers = computed(() => {
  const q = searchQuery.value.toLowerCase()
  if (!q) return users.value
  return users.value.filter(u =>
    u.fullName.toLowerCase().includes(q) ||
    u.email.toLowerCase().includes(q) ||
    u.username.toLowerCase().includes(q)
  )
})

// ---- Helpers ----
function getInitials(name: string): string {
  return name.split(' ').map(w => w[0]).slice(0, 2).join('').toUpperCase()
}

// ---- Load data ----
async function loadUsers() {
  isLoading.value = true
  try {
    users.value = await getUsers()
  } catch {
    // Interceptor đã toast lỗi
  } finally {
    isLoading.value = false
  }
}

// ---- Create ----
function openCreateDialog() {
  createForm.value = { username: '', password: '', fullName: '', email: '', isAdmin: false }
  showCreateDialog.value = true
}

async function handleCreate() {
  isSaving.value = true
  try {
    await createUser(createForm.value)
    toastService.success('Đã tạo người dùng mới.')
    showCreateDialog.value = false
    await loadUsers()
  } catch {
    // Interceptor đã toast lỗi
  } finally {
    isSaving.value = false
  }
}

// ---- Edit ----
function openEditDialog(user: UserDto) {
  editingUserId.value = user.id
  editForm.value = { fullName: user.fullName, email: user.email, isActive: user.isActive }
  showEditDialog.value = true
}

async function handleUpdate() {
  if (!editingUserId.value) return
  isSaving.value = true
  try {
    await updateUser(editingUserId.value, editForm.value)
    toastService.success('Đã cập nhật thông tin.')
    showEditDialog.value = false
    await loadUsers()
  } catch {
    // Interceptor đã toast lỗi
  } finally {
    isSaving.value = false
  }
}

// ---- Delete ----
async function handleDelete() {
  if (!editingUserId.value || !confirm('Bạn có chắc muốn xóa người dùng này?')) return
  isSaving.value = true
  try {
    await deleteUser(editingUserId.value)
    toastService.success('Đã xóa người dùng.')
    showEditDialog.value = false
    await loadUsers()
  } catch {
    // Interceptor đã toast lỗi
  } finally {
    isSaving.value = false
  }
}

onMounted(loadUsers)
</script>
