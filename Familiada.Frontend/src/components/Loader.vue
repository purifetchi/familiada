<script setup lang="ts">

</script>

<template>
    <div class="container">
        <div class="title">FAMILIADA</div>
        <div class="info">OCZEKIWANIE NA POŁĄCZENIE ORGANIZATORA</div>
        <div class="url">https://familiada.prefetcher.net/host/654e45-4353445t-2343er34-342</div>
        <div class="url-info">UŻYJ LINKU POWYŻEJ ABY DOŁĄCZYĆ JAKO ORGANIZATOR</div>
    </div>
</template>

<style scoped>
.container {
    display: flex;
    flex-direction: column;
    height: 100%;
    align-items: center;
    justify-content: center;
}

.container > div {
    margin-bottom: 15px;
}

.title {
    font-size: 120pt;
}

.info {
    font-size: 24pt;
}

.url {
    font-family: system-ui, -apple-system, BlinkMacSystemFont, 'Segoe UI', Roboto, Oxygen, Ubuntu, Cantarell, 'Open Sans', 'Helvetica Neue', sans-serif;
    padding: 10px;
    border: 1px solid var(--yellow);
    font-weight: bold;
}

.url-info {
    font-size: 10pt;
}
</style>