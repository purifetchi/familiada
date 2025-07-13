<script setup lang="ts">
import useTeamStore from '@/stores/teamStore';
import { computed } from 'vue';


const props = defineProps<{
    teamIndex: number
}>();

const teamStore = useTeamStore()

const name = computed(() => {
    return teamStore
        .teams[props.teamIndex]
        .name
})

const points = computed(() => {
    return teamStore
        .teams[props.teamIndex]
        .points
})

</script>

<template>
    <div>
        <div>{{ name }}</div>
        <div>{{ points }}</div>
    </div>
</template>

<style scoped>

</style>