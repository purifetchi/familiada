<script setup lang="ts">
import type { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { inject, ref } from 'vue';

const gameState = useGameStore();
const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;
const jsonText = ref<string>("")

const sendRoundJson = () => {
    connection.value?.send("PrepareGame", jsonText.value)
    gameState.state = GameState.ActingAsHost
}

</script>

<template>
    <textarea v-model="jsonText" placeholder="WPISZ TUTAJ JSON">

    </textarea>
    <button v-on:click="sendRoundJson()" >SUBMIT</button>
</template>

<style scoped>

</style>