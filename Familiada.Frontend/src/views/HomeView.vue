<script setup lang="ts">
import GameScreen from '@/components/GameScreen.vue';
import Loader from '@/components/Loader.vue';
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';

const state = useGameStore();
</script>

<template>
  <Loader v-if="state.state === GameState.InProgress"/>
  <GameScreen v-else />
</template>

<style scoped>

</style>