import './assets/main.css'

import { createApp } from 'vue'
import { createPinia } from 'pinia'
import signalRPlugin from "./plugins/signalr";

import App from './App.vue'
import router from './router'

const app = createApp(App)

app.use(signalRPlugin)
app.use(createPinia())
app.use(router)

app.mount('#app')
