<script setup lang="ts">

const props = defineProps<{
    name: string,
    points: number
}>();

</script>

<template>
    <div>
        <div>{{ props.name }}</div>
        <div>{{ props.points }}</div>
    </div>
</template>

<style scoped>

</style>