<script setup lang="ts">
import type { useSignalR } from '@/composables/useSignalR';
import useTeamStore from '@/stores/teamStore';
import { computed, inject } from 'vue';

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

const props = defineProps<{
    team: 0 | 1
}>()

const teams = useTeamStore()

const team = computed(() => {
    return teams.teams[props.team]
})

const sendWrong = (wrongType: 'small' | 'big') => {
    connection.value?.send("SendWrong", props.team, wrongType)
}

const clearWrongs = () => {
    connection.value?.send("ClearWrong", props.team)
}

const awardPoints = () => {
    connection.value?.send("AwardPoints", props.team)
}

</script>

<template>
    <div class="team-actions-container">
        <div>DRUZYNA {{ team.name }}</div>
        <button v-on:click="sendWrong('small')">WYŚLIJ MAŁY BŁĄD</button>
        <button v-on:click="sendWrong('big')">WYŚLIJ DUŻY BŁĄD</button>
        <hr />
        <button v-on:click="clearWrongs()">WYCZYŚĆ BŁĘDY</button>
        <hr />
        <button v-on:click="awardPoints()">PRZYZNAJ PUNKTY</button>
    </div>    
</template>

<style scoped>
    .team-actions-container {
        text-align: center;
        padding: 20px;
        border: 2px solid var(--yellow);
    }
</style>