export var GameState;
(function (GameState) {
    GameState[GameState["Started"] = 0] = "Started";
    GameState[GameState["Aborted"] = 1] = "Aborted";
    GameState[GameState["NotStarted"] = 2] = "NotStarted";
    GameState[GameState["FinishedByDraw"] = 3] = "FinishedByDraw";
    GameState[GameState["FinishedByXWin"] = 4] = "FinishedByXWin";
    GameState[GameState["FinishedByOWin"] = 5] = "FinishedByOWin";
})(GameState || (GameState = {}));
export let MainMatrix = {
    gameField: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
    gameState: 2
};
