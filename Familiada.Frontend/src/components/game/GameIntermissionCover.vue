<script setup lang="ts">
import useGameStore from '@/stores/gameStore';
import { computed, watch } from 'vue';
import intermissionSfx from '@/assets/sounds/intermission.mp3';
import { useSound } from '@vueuse/sound';

const state = useGameStore();
const intermission = useSound(intermissionSfx)

watch(state, () => {
    if (state.intermissionVisible) {
        intermission.play()
    }
})

</script>

<template>
    <div class="container" :class="{ 'visible': state.intermissionVisible }">
        <div class="title">FAMILIADA</div>
    </div>
</template>

<style scoped>
.container {
    position: fixed;
    display: flex;
    flex-direction: column;
    height: 100%;
    width: 100%;
    align-items: center;
    justify-content: center;
    text-align: center;
    background: black;
    opacity: 0;
    transition: opacity 0.4s ease-out;
}

.title {
    font-size: 120pt;
}

.visible {
    opacity: 1 !important;
    transition: opacity 0.4s ease-out;
}
</style>