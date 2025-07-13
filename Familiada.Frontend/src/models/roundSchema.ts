import type AnswerSchema from "./answerSchema";

export default interface RoundSchema {
    question: string,
    answers: AnswerSchema[]
}