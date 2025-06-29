import { defineStore } from "pinia"

const useTeamStore = defineStore("teams", {
    state: () => {
        return {
            teams: [] as {
                name: string,
                points: number
            }[]
        }
    }
})

export default useTeamStore