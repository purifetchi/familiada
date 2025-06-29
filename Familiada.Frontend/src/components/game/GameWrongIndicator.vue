<script setup lang="ts">

const props = defineProps<{
    type: "big" | "small"
}>();

</script>

<template>
    <div class="wrong big" v-if="props.type == 'big'">
        <div>c b</div>
        <div>e d</div>
        <div> h </div>
        <div>b c</div>
        <div>d e</div>
    </div>
    <div class="wrong small" v-else>
        <div>fig</div>
        <div> h </div>
        <div>gjf</div>
    </div>
</template>

<style scoped>
.wrong>div {
    white-space: pre;
}

.big > div {
    font-size: 72px !important;
}

.small > div {
    font-size: 48px !important;
}
</style>