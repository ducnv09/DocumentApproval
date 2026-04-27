import { createApp } from 'vue'
import './style.css'
import './tailwind.css'
import App from './App.vue'
import router from './router'

import PrimeVue from 'primevue/config'
import Aura from '@primevue/themes/aura' // Giao diện mới mặc định của v4
import 'primeicons/primeicons.css'

const app = createApp(App)

app.use(router)

// Cách thiết lập Theme mới của PrimeVue 4
app.use(PrimeVue, {
    theme: {
        preset: Aura,
        options: {
            darkModeSelector: '.my-app-dark',
        }
    }
})

app.mount('#app')