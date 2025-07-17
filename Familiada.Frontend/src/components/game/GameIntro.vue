<script setup lang="ts">
import { GameState } from '@/models/gameState';
import useGameStore from '@/stores/gameStore';
import { ref, watch } from 'vue';

const state = useGameStore();
const videoRef = ref<HTMLVideoElement | null>(null);

const onVideoEnded = () => {
    if (state.state === GameState.WatchingIntro) {
        state.state = GameState.InProgress;
        state.intermissionVisible = true;
    }
}

watch(state, () => {
    if (state.state === GameState.WatchingIntro && videoRef.value) {
        videoRef.value!.play()
    } else {
        videoRef.value?.pause()
    }
})

</script>

<template>
    <div class="container" :class="{ 'visible': state.state == GameState.WatchingIntro }">
        <video ref="videoRef" @ended="onVideoEnded">
            <source src="@/assets/intro.mp4" type="video/mp4" />
        </video>
    </div>
</template>

<style scoped>
    .container {
        position: fixed;
        height: 100%;
        width: 100%;
        align-items: center;
        justify-content: center;
        text-align: center;
        background: black;
        opacity: 0;
        transition: opacity 0.4s ease-out;
        pointer-events: none;
        z-index: 999;
    }

    video {
        width: 100%;
        height: 100%;
    }

    .visible {
        opacity: 1;
        transition: opacity 0.4s ease-out;
    }

</style>