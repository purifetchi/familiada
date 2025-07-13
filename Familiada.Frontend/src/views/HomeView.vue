<script setup lang="ts">
import ConnectingScreen from '@/components/ConnectingScreen.vue';
import GameScreen from '@/components/GameScreen.vue';
import Loader from '@/components/Loader.vue';
import { useSignalR } from '@/composables/useSignalR';
import { GameState } from '@/models/gameState';
import type RoundInfo from '@/models/roundInfo';
import type TeamUpdate from '@/models/teamUpdate';
import useGameStore from '@/stores/gameStore';
import useRoundStore from '@/stores/roundStore';
import useTeamStore from '@/stores/teamStore';
import { inject, onMounted } from 'vue';

const signalr = inject<ReturnType<typeof useSignalR>>('signalr')!;

const state = useGameStore()
const teams = useTeamStore()
const round = useRoundStore()

onMounted(() => {
  const { connection } = signalr;
  connection.value!.on("SessionCreated", (sessionId: string) => {
    state.token = sessionId
  })

  connection.value!.on("HostConnected", () => {
    state.state = GameState.WaitingForHostStart
  })

  connection.value!.on("SetRoundInfo", (info: RoundInfo) => {
    round.answers = Array(info.questionAmount).fill({
      state: 'hidden'
    })
    round.wrongMarks = {
      left: [],
      right: []
    }
    round.multiplier = info.multiplier

    state.state = GameState.InProgress
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
  <ConnectingScreen v-if="state.state == GameState.Connecting" />
  <Loader v-else-if="state.state === GameState.Awaiting || state.state == GameState.WaitingForHostStart"/>
  <GameScreen v-else />
</template>

<style scoped>

</style>