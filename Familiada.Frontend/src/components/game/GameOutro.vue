<script setup lang="ts">
import useTeamStore from '@/stores/teamStore';
import { computed } from 'vue';

const teams = useTeamStore()

const winner = computed(() => {
    return teams.teams.sort((a, b) => b.points - a.points)[0]
})

</script>

<template>
    <div class="container">
        <div class="title">FAMILIADA</div>
        <div class="info">WYGRAŁA DRUŻYNA {{ winner.name }}</div>
        <div class="info">ZDOBYŁA ONA {{ winner.points }} PUNKTÓW</div>
    </div>
</template>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    height: 100%;
    align-items: center;
    justify-content: center;
    text-align: center;
}

.container>div>div {
    margin-bottom: 15px;
}

.title {
    font-size: 120pt;
}

.info {
    font-size: 24pt;
}
</style>