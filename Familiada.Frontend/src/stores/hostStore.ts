import type RoundSchema from "@/models/roundSchema";
import type UpcomingRound from "@/models/upcomingRound";
import { defineStore } from "pinia";

const useHostStore = defineStore("hostStore", {
    state: () => {
        return {
            upcomingRound: undefined as UpcomingRound | undefined,
            currentRound: undefined as RoundSchema | undefined,
            leaderTeam: undefined as number | undefined,
            canSendSmallWrongAnswers: false,
            canSendBigWrongAnswers: false
        }        
    },
})

export default useHostStore