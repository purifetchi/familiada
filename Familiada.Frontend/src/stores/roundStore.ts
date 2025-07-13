import { defineStore } from "pinia"

const useRoundStore = defineStore("rounds", {
    state: () => {
        return {
            answers: [

            ] as {
                state: "shown" | "hidden",
                answer?: string,
                points?: number
            }[],
            wrongMarks: {
                left: [],
                right: []
            } as {
                left: {
                    type: "big" | "small"
                }[],
                right: {
                    type: "big" | "small"
                }[]
            },
            multiplier: 1
        }
    }
})

export default useRoundStore