<script setup lang="ts">
import type { useSignalR } from '@/composables/useSignalR';
import useHostStore from '@/stores/hostStore';
import { inject } from 'vue';

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;
const hostStore = useHostStore()

const beginRound = () => {
    connection.value!.send("StartRound")
}

</script>

<template>
    <div v-if="hostStore.upcomingRound !== undefined" class="upcoming-container">
        <div>NASTĘPNE PYTANIE: {{ hostStore.upcomingRound.question.toUpperCase() }}</div>
        <div>MNOŻNIK: {{ hostStore.upcomingRound.multiplier }}</div>
        <button v-on:click="beginRound()">ROZPOCZNIJ</button>
    </div>
</template>

<style scoped>
.upcoming-container {
    display: flex;
    flex-direction: column;
    height: 100%;
    align-items: center;
    justify-content: center;
    text-align: center;
    font-size: 32px;
}
</style>