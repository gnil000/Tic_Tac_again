export enum GameState {
    Started,
    Aborted,
    NotStarted,
    FinishedByDraw,
    FinishedByXWin,
    FinishedByOWin,
}

export type Matrix = {
    gameField: string[];
    gameState: GameState;
}

export let MainMatrix: Matrix ={
    gameField: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
    gameState: 2
};
