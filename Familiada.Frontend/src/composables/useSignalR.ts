import { GameState } from "@/models/gameState";
import useGameStore from "@/stores/gameStore";
import { HubConnectionBuilder, LogLevel, HubConnection } from "@microsoft/signalr";
import { onBeforeUnmount, ref } from "vue";

export function useSignalR(hubUrl: string) {
  const connection = ref<HubConnection | null>(null);

  async function start() {
    connection.value = new HubConnectionBuilder()
      .withUrl(hubUrl)
      .withAutomaticReconnect()
      .configureLogging(LogLevel.Information)
      .build();

    await connection.value.start();

    const gameStore = useGameStore()
    gameStore.state = GameState.Awaiting
  }

  async function stop() {
    if (connection.value) {
      await connection.value.stop();
    }
  }

  onBeforeUnmount(stop);

  return { start, stop, connection };
}