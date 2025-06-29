<script setup lang="ts">
import { useSignalR } from '@/composables/useSignalR';
import { inject } from 'vue';

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

const test = () => {
    connection.value?.send("SendAnswer", 1)
}

const test2 = () => {
    connection.value?.send("SendWrong", "left", "small")
}

const test3 = () => {
    connection.value?.send("SendWrong", "right", "big")
}

</script>

<template>
    <button v-on:click.prevent="test()">TEST SEND CORRECT FOR INDEX 1</button>
    <button v-on:click.prevent="test2()">TEST SEND WRONG FOR TEAM LEFT</button>
    <button v-on:click.prevent="test3()">TEST SEND WRONG FOR TEAM RIGHT</button>
</template>

<style scoped>
</style>