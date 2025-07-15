<script setup lang="ts">
import { useSignalR } from '@/composables/useSignalR';
import useHostStore from '@/stores/hostStore';
import { inject } from 'vue';
import TeamActionPanel from './TeamActionPanel.vue';

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;
const hostStore = useHostStore()

const showAnswer = (index: number) => {
    connection.value?.send("SendAnswer", index)
}

const endRound = () => {
    connection.value?.send("EndRound")
}

</script>

<template>
    <div v-if="hostStore.currentRound !== undefined" class="panel-container">
        <TeamActionPanel :team="0" />
        <div>
            <div>PYTANIE: {{ hostStore.currentRound.question.toUpperCase() }}</div>
            <div class="answers">
                <div v-for="(answer, index) of hostStore.currentRound.answers">
                    <input type="checkbox" v-on:change="showAnswer(index)"> {{ answer.text.toUpperCase() }} - {{
                    answer.points }}
                </div>
            </div>
            <hr>
            <button v-on:click="endRound()">ZAKOŃCZ RUNDĘ</button>
        </div>
        <TeamActionPanel :team="1" />
    </div>
</template>

<style scoped>
.panel-container {
    width: 100%;
    height: 100%;
    font-size: 32px;
    display: flex;
    flex-direction: row;
    justify-content: space-around;
    align-items: center;
}
</style>