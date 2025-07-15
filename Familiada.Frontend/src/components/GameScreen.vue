<script setup lang="ts">
import GameAnswerRow from './game/GameAnswerRow.vue';
import GameWrongIndicator from './game/GameWrongIndicator.vue';
import GameTeamDisplay from './game/GameTeamDisplay.vue';
import { computed, inject, onMounted, ref } from 'vue';
import { useSound } from '@vueuse/sound';
import correctSfx from '@/assets/sounds/correct.mp3'
import wrongSfx from '@/assets/sounds/wrong.mp3'
import { useSignalR } from '@/composables/useSignalR';
import useRoundStore from '@/stores/roundStore';

const correctSound = useSound(correctSfx)
const wrongSound = useSound(wrongSfx)

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

const roundStore = useRoundStore()

const points = computed(() => {
    var points = roundStore.answers
        .filter(a => a.state == "shown")
        .reduce((s, a) => s + (a.points! * roundStore.multiplier), 0)
        .toString()

    return points.padStart(3, "0")
});

const showAnswer = (
    index: number,
    answer: string,
    points: number
) => {
    roundStore.answers[index] = {
        state: "shown",
        answer: answer,
        points: points
    }

    correctSound.play()
}

const showWrong = (
    team: "left" | "right",
    type: "big" | "small"
) => {
    wrongSound.play()
    roundStore.wrongMarks[team] = [
        ...roundStore.wrongMarks[team],
        {
            type
        }
    ]
}

onMounted(() => {
    connection.value?.on("ShowPanelAnswer", (index: number, answer: string, points: number) => {
        showAnswer(index, answer.toUpperCase(), points)
    })

    connection.value?.on("WrongAnswer", (team: "left" | "right", type: "big" | "small") => {
        showWrong(team, type)
    })

    connection.value?.on("ClearWrongDisplay", (team: "left" | "right") => {
        roundStore.wrongMarks[team] = []
    })
})

</script>

<template>
    <div class="vertical-divider">
        <div class="horizontal-divider">
            <GameTeamDisplay :team-index="0" />
            <div class="points">
                {{ points }}
            </div>
            <GameTeamDisplay :team-index="1" />
        </div>
        <div class="horizontal-divider board-container">
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator v-for="mark of roundStore.wrongMarks.left" :type="mark.type" />
            </div>
            <div class="board">
                <GameAnswerRow v-for="(answer, index) of roundStore.answers" :index="index + 1" :answer="answer.answer"
                    :points="answer.points" />
            </div>
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator v-for="mark of roundStore.wrongMarks.right" :type="mark.type" />
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
    min-width: 162px !important;
    justify-content: start !important;
}

.board>div {
    white-space: pre;
}

.points {
    font-size: 104px !important;
}
</style>