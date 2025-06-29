<script setup lang="ts">
import { computed } from 'vue';

const width = 20;

const props = defineProps<{
    index: number,
    answer?: string,
    points?: number
}>();

const answer = computed(() => {
    const padding = "l".repeat(width);

    if (props.answer !== undefined) {
        if (props.answer.length > width) {
            return props.answer.slice(0, width);
        }

        return props.answer + " ".repeat(width - props.answer.length);
    }

    return padding;
});

const points = computed(() => {
    return props.points !== undefined
        ? props.points
        : "mm";
});

</script>

<template>
    <div>{{ props.index }} {{ answer }} {{ points }}</div>
</template>

<style scoped>

</style>