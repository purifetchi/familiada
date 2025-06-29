<script setup lang="ts">
import ConnectingScreen from '@/components/ConnectingScreen.vue';
import GameScreen from '@/components/GameScreen.vue';
import Loader from '@/components/Loader.vue';
import { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { inject, onMounted } from 'vue';

const signalr = inject<ReturnType<typeof useSignalR>>('signalr')!;

const state = useGameStore();

onMounted(() => {
  const { connection } = signalr;
  connection.value!.on("SessionCreated", (sessionId: string) => {
    state.token = sessionId
  })

  connection.value!.on("HostConnected", () => {
    state.state = GameState.InProgress
  })
})
</script>

<template>
  <ConnectingScreen v-if="state.state == GameState.Connecting" />
  <Loader v-else-if="state.state === GameState.Awaiting"/>
  <GameScreen v-else />
</template>

<style scoped>

</style>