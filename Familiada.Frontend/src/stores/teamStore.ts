import { defineStore } from "pinia"

const useTeamStore = defineStore("teams", {
    state: () => {
        return {
            teams: [
                {
                    name: "CZERWONI",
                    points: 0
                },
                {
                    name: "ZIELONI",
                    points: 0
                }
            ] as {
                name: string,
                points: number
            }[]
        }
    }
})

export default useTeamStore