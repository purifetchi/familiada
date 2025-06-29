<script setup lang="ts">
import GamePanel from '@/components/host/GamePanel.vue';
import { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { inject, watch } from 'vue';
import { useRoute } from 'vue-router';

const gameStore = useGameStore()
const route = useRoute()
const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

watch(gameStore, () => {
    if (gameStore.state == GameState.Awaiting) {
        connection.value?.send("HelloHost", route.params.sessionId)
        gameStore.state = GameState.ActingAsHost
    }
})
</script>

<template>
    <GamePanel v-if="gameStore.state == GameState.ActingAsHost" />
</template>

<style scoped>
</style>