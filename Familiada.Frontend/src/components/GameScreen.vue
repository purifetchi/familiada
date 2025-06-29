<script setup lang="ts">
import GameAnswerRow from './game/GameAnswerRow.vue';
import GameWrongIndicator from './game/GameWrongIndicator.vue';
import GameTeamDisplay from './game/GameTeamDisplay.vue';

</script>

<template>
    <div class="vertical-divider">
        <div class="horizontal-divider">
            <GameTeamDisplay name="NIEBIESCY" :points="123" />
            <div class="timer">
                000
            </div>
            <GameTeamDisplay name="ZIELONI" :points="0" />
        </div>
        <div class="horizontal-divider board-container">
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator type="big" />
            </div>
            <div class="board">
                <GameAnswerRow :index="1" answer="ZAPISKI ZIELARKI" :points="35"/>
                <GameAnswerRow :index="2" />
                <GameAnswerRow :index="3" answer="DEMON SLAYER" :points="15"/>
                <GameAnswerRow :index="4" answer="JJK" :points="5"/>
                <GameAnswerRow :index="5"/>
            </div>
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator type="small" />
                <GameWrongIndicator type="small" />
                <GameWrongIndicator type="small" />
            </div>
        </div>
    </div>
</template>

<style scoped>
div {
    font-size: 48px;
}

.vertical-divider {
    width: inherit;
    height: inherit;
    display: flex;

    justify-content: space-around;
    flex-direction: column;
}

.horizontal-divider {
    display: flex;
    justify-content: space-between;
    align-items: center;
}

.board-container {
    flex: 1;
}

.wrong-container {
    height: calc(100% - (64px * 2));
    justify-content: start !important;
}

.board>div {
    white-space: pre;
}

.timer {
    font-size: 104px !important;
}
</style>