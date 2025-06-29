import { type App } from "vue";
import { useSignalR } from "@/composables/useSignalR";

export default {
  install(app: App) {
    const hubUrl = import.meta.env.VITE_API + "/hub";
    const signalr = useSignalR(hubUrl);
    signalr.start();

    app.config.globalProperties.$signalr = signalr;
    app.provide("signalr", signalr);
  }
};