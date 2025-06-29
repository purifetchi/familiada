<script setup lang="ts">
import GameAnswerRow from './game/GameAnswerRow.vue';
import GameWrongIndicator from './game/GameWrongIndicator.vue';
import GameTeamDisplay from './game/GameTeamDisplay.vue';
import { computed, inject, onMounted, ref } from 'vue';
import { useSound } from '@vueuse/sound';
import correctSfx from '@/assets/sounds/correct.mp3'
import wrongSfx from '@/assets/sounds/wrong.mp3'
import { useSignalR } from '@/composables/useSignalR';

const correctSound = useSound(correctSfx)
const wrongSound = useSound(wrongSfx)

const { connection } = inject<ReturnType<typeof useSignalR>>('signalr')!;

const answers = ref<{
    state: "shown" | "hidden",
    answer?: string,
    points?: number
}[]>([
    {
        state: "hidden"
    },
    {
        state: "hidden"
    },
    {
        state: "hidden"
    },
    {
        state: "hidden"
    },
    {
        state: "hidden"
    },
    {
        state: "hidden"
    }
])

const wrongMarks = ref<{
    left: {
        type: "big" | "small"
    }[],
    right: {
        type: "big" | "small"
    }[]
}>({
    left: [],
    right: []
})

const points = computed(() => {
    var points = answers.value
        .filter(a => a.state == "shown")
        .reduce((s, a) => s + a.points!, 0)
        .toString()

    return points.padStart(3, "0")
});

const showAnswer = (
    index: number,
    answer: string,
    points: number
) => {
    answers.value[index] = {
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
    wrongMarks.value[team] = [
        ...wrongMarks.value[team],
        {
            type
        }
    ]
}

onMounted(() => {
    connection.value?.on("CorrectAnswer", (index: number, answer: string, points: number) => {
        showAnswer(index - 1, answer, points)
    })

    connection.value?.on("WrongAnswer", (team: "left" | "right", type: "big" | "small") => {
        showWrong(team, type)
    })
})

</script>

<template>
    <div class="vertical-divider">
        <div class="horizontal-divider">
            <GameTeamDisplay name="NIEBIESCY" :points="123" />
            <div class="points">
                {{ points }}
            </div>
            <GameTeamDisplay name="ZIELONI" :points="0" />
        </div>
        <div class="horizontal-divider board-container">
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator v-for="mark of wrongMarks.left" :type="mark.type" />
            </div>
            <div class="board">
                <GameAnswerRow v-for="(answer, index) of answers" :index="index + 1" :answer="answer.answer"
                    :points="answer.points" />
                    <button v-on:click.prevent="showWrong('left', 'small')">TEST WRONG LEFT</button>
                    <button v-on:click.prevent="showWrong('right', 'big')">TEST WRONG RIGHT</button>
            </div>
            <div class="vertical-divider wrong-container">
                <GameWrongIndicator v-for="mark of wrongMarks.right" :type="mark.type" />
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