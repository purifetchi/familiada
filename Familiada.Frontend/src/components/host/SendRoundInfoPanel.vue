<script setup lang="ts">
import type { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { inject, ref } from 'vue';

const gameState = useGameStore();
const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;
const jsonText = ref<string>("")
const leftTeamName = ref<string>("")
const rightTeamName = ref<string>("")


const sendRoundJson = () => {
    connection.value?.send("PrepareGame", jsonText.value, leftTeamName.value, rightTeamName.value)
    gameState.state = GameState.ActingAsHost
}

</script>

<template>
    <div class="setup-container">
        <div>
            USTAW PARAMETRY GRY
        </div>
        <div>
            <textarea v-model="jsonText" placeholder="WPISZ TUTAJ JSON"></textarea>
        </div>
        <div>
            NAZWA LEWEJ DRUŻYNY <input v-model="leftTeamName">
        </div>
        <div>
            NAZWA PRAWEJ DRUŻYNY <input v-model="rightTeamName"> 
        </div>
        <button v-on:click="sendRoundJson()" >ZATWIERDŹ GRĘ</button>
    </div>
</template>

<style scoped>
.setup-container {
    display: flex;
    flex-direction: column;
    height: 100%;
    align-items: center;
    justify-content: center;
    text-align: center;
    font-size: 32px;
}

.container>div {
    margin-bottom: 15px;
}
</style>