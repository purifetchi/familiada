<script setup lang="ts">
import { useSignalR } from '@/composables/useSignalR';
import useHostStore from '@/stores/hostStore';
import useTeamStore from '@/stores/teamStore';
import { inject } from 'vue';

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;
const hostStore = useHostStore()
const teamStore = useTeamStore()

const showAnswer = (index: number) => {
    connection.value?.send("SendAnswer", index)
}

const setStartingTeam = (index: number) => {
    connection.value?.send("SetStartingTeam", index)
}

const sendWrong = (wrongType: 'small' | 'big') => {
    connection.value?.send("SendWrong", wrongType)
}

</script>

<template>
    <div v-if="hostStore.currentRound !== undefined" class="panel-container">
        <div>PYTANIE: {{ hostStore.currentRound.question.toUpperCase() }}</div>
        <div class="info-container" v-if="hostStore.leaderTeam !== undefined">
            <div class="answers">
                <div v-for="(answer, index) of hostStore.currentRound.answers">
                    <input type="checkbox" v-on:change="showAnswer(index)"> {{ answer.text.toUpperCase() }} - {{ answer.points }}
                </div>
            </div>
            <div class="team-operations">
                <div>PUNKTY OBECNIE ZDOBYWA DRUZYNA: {{ teamStore.teams[hostStore.leaderTeam].name }}</div>
                <button v-on:click="sendWrong('small')" :disabled="!hostStore.canSendSmallWrongAnswers">WYŚLIJ MAŁY BŁĄD</button>
                <button v-on:click="sendWrong('big')" :disabled="!hostStore.canSendBigWrongAnswers">WYŚLIJ DUŻY BŁĄD</button>
            </div>
        </div>
        <div v-else>
            <button v-for="(team, index) of teamStore.teams" v-on:click="setStartingTeam(index)">ZACZYNA DRUZYNA {{ team.name }}</button>
        </div>
        
    </div>
</template>

<style scoped>
    .panel-container {
        width: 100%;
        height: 100%;
        font-size: 32px;
        display: flex;
        flex-direction: column;
        justify-content: center;
        align-items: center;
    }

    .info-container > * {
        margin-bottom: 20px;
    }

    .panel-container > * {
        margin-bottom: 20px;
    }

    .team-operations {
        padding: 10px;
        border: 2px solid var(--yellow);
    }
</style>