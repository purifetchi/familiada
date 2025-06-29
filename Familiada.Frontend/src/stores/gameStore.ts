import { GameState } from "@/models/gameState";
import { defineStore } from "pinia";

const useGameStore = defineStore("gameStore", {
    state: () => {
        return {
            token: "",
            state: GameState.Connecting
        }        
    },
})

export default useGameStore