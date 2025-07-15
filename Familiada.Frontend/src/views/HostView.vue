<script setup lang="ts">
import GamePanel from '@/components/host/GamePanel.vue';
import SendRoundInfoPanel from '@/components/host/SendRoundInfoPanel.vue';
import UpcomingRoundPanel from '@/components/host/UpcomingRoundPanel.vue';
import { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import type RoundSchema from '@/models/roundSchema';
import type TeamUpdate from '@/models/teamUpdate';
import type UpcomingRound from '@/models/upcomingRound';
import useGameStore from '@/stores/gameStore';
import useHostStore from '@/stores/hostStore';
import useTeamStore from '@/stores/teamStore';
import { inject, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';

const hostStore = useHostStore()
const gameStore = useGameStore()
const teams = useTeamStore()
const route = useRoute()
const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

watch(gameStore, () => {
    if (gameStore.state == GameState.Awaiting) {
        connection.value?.send("HelloHost", route.params.sessionId)
        gameStore.state = GameState.HostWaitingForRoundJson
    }
})

onMounted(() => {
    connection.value!.on("ShowUpcomingRound", (upcomingRound: UpcomingRound) => {
        hostStore.upcomingRound = upcomingRound
        gameStore.state = GameState.HostShowingUpcomingRound
    })

    connection.value!.on("SetHostRoundSchema", (schema: RoundSchema) => {
        hostStore.currentRound = schema
        hostStore.leaderTeam = undefined
        gameStore.state = GameState.ActingAsHost
    })

    connection.value!.on("SetCurrentRoundPointWinningTeam", (message: {
        winningTeam: number,
        canSendSmallWrongAnswers: boolean,
        canSendBigWrongAnswers: boolean
    }) => {
        hostStore.leaderTeam = message.winningTeam
        hostStore.canSendBigWrongAnswers = message.canSendBigWrongAnswers
        hostStore.canSendSmallWrongAnswers = message.canSendSmallWrongAnswers
    })

    connection.value!.on("SetTeamData", (info: TeamUpdate[]) => {
        for (var i = 0; i < info.length; i++) {
            teams.teams[i].name = info[i].name
            teams.teams[i].points = info[i].points
        }
    })
})
</script>

<template>
    <SendRoundInfoPanel v-if="gameStore.state == GameState.HostWaitingForRoundJson" />
    <UpcomingRoundPanel v-else-if="gameStore.state == GameState.HostShowingUpcomingRound" />
    <GamePanel v-else-if="gameStore.state == GameState.ActingAsHost" />
</template>

<style scoped>
</style>