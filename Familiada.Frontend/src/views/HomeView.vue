<script setup lang="ts">
import ConnectingScreen from '@/components/ConnectingScreen.vue';
import GameIntermissionCover from '@/components/game/GameIntermissionCover.vue';
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
import outroSfx from '@/assets/sounds/outro.mp3';
import bellsSfx from '@/assets/sounds/bells.mp3';
import { useSound } from '@vueuse/sound';
import GameIntro from '@/components/game/GameIntro.vue';
import GameOutro from '@/components/game/GameOutro.vue';

const signalr = inject<ReturnType<typeof useSignalR>>('signalr')!;

const state = useGameStore()
const teams = useTeamStore()
const round = useRoundStore()

const bells = useSound(bellsSfx)

const outro = useSound(outroSfx)

onMounted(() => {
  const { connection } = signalr;
  connection.value!.on("SessionCreated", (sessionId: string) => {
    state.token = sessionId
  })

  connection.value!.on("HostConnected", () => {
    state.state = GameState.WaitingForHostStart
  })

  connection.value!.on("EndRound", () => {
    state.intermissionVisible = true;
  })

  connection.value!.on("PlayIntro", () => {
    state.state = GameState.WatchingIntro
  })

  connection.value!.on("EndGame", () => {
    outro.play()
    state.state = GameState.End
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
    state.intermissionVisible = false;
  })

  connection.value!.on("SetTeamData", (info: TeamUpdate[]) => {
    bells.play()
    
    for (var i = 0; i < info.length; i++) {
      teams.teams[i].name = info[i].name
      teams.teams[i].points = info[i].points
    }
  })
})
</script>

<template>
  <GameIntro />
  <GameIntermissionCover v-if="state.state == GameState.InProgress" />
  <ConnectingScreen v-if="state.state == GameState.Connecting" />
  <GameOutro v-else-if="state.state == GameState.End" />
  <Loader v-else-if="state.state === GameState.Awaiting || state.state == GameState.WaitingForHostStart"/>
  <GameScreen v-else />
</template>

<style scoped>

</style>