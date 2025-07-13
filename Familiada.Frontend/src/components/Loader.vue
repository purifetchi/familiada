<script setup lang="ts">
import { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { computed, inject, onMounted, ref } from 'vue';

const { connection } = inject<ReturnType<typeof useSignalR>>("signalr")!;
const gameStore = useGameStore()

const url = computed(() => {
    if (gameStore.token === "") {
        return "";
    }

    return `${window.location.href}host/${gameStore.token}`
})

onMounted(() => {
    connection.value?.send("Hello")
})

const openHost = () => {
    window.open(url.value, "_blank", "popup")
}
</script>

<template>
    <div class="container">
        <div class="title">FAMILIADA</div>
        <div v-if="gameStore.state == GameState.Awaiting">
            <div class="info">OCZEKIWANIE NA POŁĄCZENIE GOSPODARZA</div>
            <div class="url" v-if="gameStore.token !== ''" v-on:click.prevent="openHost()">{{ url }}</div>
            <div class="url-info">UŻYJ LINKU POWYŻEJ ABY DOŁĄCZYĆ JAKO GOSPODARZ</div>
        </div>
        <div v-else-if="gameStore.state == GameState.WaitingForHostStart">
            <div class="info">OCZEKIWANIE NA ROZPOCZĘCIE GRY PRZEZ GOSPODARZA</div>
        </div>
    </div>
</template>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    height: 100%;
    align-items: center;
    justify-content: center;
    text-align: center;
}

.container>div>div {
    margin-bottom: 15px;
}

.title {
    font-size: 120pt;
}

.info {
    font-size: 24pt;
}

.url {
    font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
    padding: 10px;
    border: 1px solid var(--yellow);
    font-weight: bold;

    cursor: pointer;
}

.url-info {
    font-size: 10pt;
}
</style>