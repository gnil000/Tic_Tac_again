"use strict";
//import { MainMatrix, GameState, Matrix } from "./matrix.js";
var __awaiter = (this && this.__awaiter) || function (thisArg, _arguments, P, generator) {
    function adopt(value) { return value instanceof P ? value : new P(function (resolve) { resolve(value); }); }
    return new (P || (P = Promise))(function (resolve, reject) {
        function fulfilled(value) { try { step(generator.next(value)); } catch (e) { reject(e); } }
        function rejected(value) { try { step(generator["throw"](value)); } catch (e) { reject(e); } }
        function step(result) { result.done ? resolve(result.value) : adopt(result.value).then(fulfilled, rejected); }
        step((generator = generator.apply(thisArg, _arguments || [])).next());
    });
};
let MainMatrix = {
    gameField: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
    //gameState: 2
};
class Player {
}
let oldMatrix = [-1, -1, -1, -1, -1, -1, -1, -1, -1];
const player = new Player();
const message = document.querySelector('#game-state-info');
const messageScore = document.querySelector('#score-info');
const messageTurn = document.querySelector("#whose_turn");
const messageOpponentName = document.querySelector("#opponent-name");
const messageContent = message === null || message === void 0 ? void 0 : message.textContent;
const startBtn = document.querySelector("#start-btn");
const resignBtn = document.querySelector("#resign-btn");
const fields = document.querySelectorAll(".field");
//+++++++++++++++++++++++++++++++++++++++
const registrBtn = document.querySelector('#register-btn');
const playerNameInput = document.querySelector('#player-name-field');
//+++++++++++++++++++++++++++++++++++++++
let id;
fields.forEach((f, i) => {
    f.addEventListener('click', (x) => UserInput(i));
});
startBtn.addEventListener('click', () => StartGame());
resignBtn.addEventListener('click', () => Disconnected());
//+++++++++++++++++++++++++++++++++++++++
registrBtn.addEventListener('click', x => Registration(playerNameInput.value));
//+++++++++++++++++++++++++++++++++++++++
function StartGame() {
    return __awaiter(this, void 0, void 0, function* () {
        let url = "https://localhost:7025/Game/StartGame?id=" + player.id.toString();
        fetch(url, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
        })
            .then(data => data.json())
            .then(json => {
            player.marker = json.marker;
            player.waitMove = json.waitMove;
            player.opponentName = json.opponentName;
            messageOpponentName.innerHTML = `Your opponent: ${player.opponentName}`;
            if (player.marker == 0) {
                messageTurn.innerHTML = 'Your turn!';
                alert('Your turn!');
            }
            else if (player.marker == 1) {
                messageTurn.innerHTML = `${player.opponentName} turn!`;
                alert('Opponent turn!');
                RequestGameField();
            }
        });
    });
}
function UserInput(i) {
    player.waitMove = true;
    console.log('UserInput!');
    //if(MainMatrix.gameState == GameState.Started){
    let p = fields[i].children.item(0);
    //console.log(p);
    if (p.textContent == " " || p.textContent == "") {
        //console.log('in if');
        if (player.marker == 0) {
            p.append("X");
            MainMatrix.gameField[i] = "X";
        }
        else if (player.marker == 1) {
            p.append("O");
            MainMatrix.gameField[i] = "O";
        }
        //console.log(`set to ${i}`)
        MakeMove(i);
    }
    //}
}
function MakeMove(i) {
    if (player.waitMove == false) {
        messageTurn.innerHTML = 'Youre turn!';
    }
    else if (player.waitMove == true) {
        messageTurn.innerHTML = `${player.opponentName} turn!`;
    }
    fetch('https://localhost:7025/Game/SendPosition?id=' + player.id.toString() + '&position=' + i.toString(), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })
        .then(data => data.json())
        .then(json => {
        player.countWin = json.youWinCounter;
        player.countWinOpponent = json.opponentWinCounter;
        player.waitMove = json.waitMove;
        player.gameState = json.isWin;
        oldMatrix = json.field;
        showMatrix();
        RequestGameField();
        GetGameField();
    });
}
function showMatrix() {
    for (let i = 0; i < 9; i++) {
        // for (let j = 0; j < 3; j++) {
        let p = fields[i].children.item(0);
        p.textContent = MainMatrix.gameField[i];
        console.log(p.textContent);
        // }
    }
}
function Registration(name) {
    //let gameState;
    fetch('https://localhost:7025/Clients', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({
            name: name
        })
    })
        .then(data => data.json())
        .then(json => {
        player.name = name;
        player.id = json.connId;
        player.gameState = json.isWin;
        player.marker = json.marker;
    });
}
//+++++++++++++++++++++++++++++++++++++++
function RequestGameField() {
    console.log('Request send to server');
    fetch('https://localhost:7025/Game/WaitFirstMove?id=' + player.id.toString(), {
        method: 'GET'
    })
        .then(data => data.json())
        .then(json => {
        player.countWin = json.youWinCounter;
        player.countWinOpponent = json.opponentWinCounter;
        player.waitMove = json.waitMove;
        player.gameState = json.isWin;
        oldMatrix = json.field;
        GetGameField();
    });
}
function GetGameField() {
    return __awaiter(this, void 0, void 0, function* () {
        if (player.marker == 0) {
            messageScore.innerHTML = `Score: X - ${player.countWin}, O - ${player.countWinOpponent}`;
        }
        else {
            messageScore.innerHTML = `Score: X - ${player.countWinOpponent}, O - ${player.countWin}`;
        }
        if (player.waitMove == false) {
            messageTurn.innerHTML = 'Your turn!';
        }
        else if (player.waitMove == true) {
            messageTurn.innerHTML = `${player.opponentName} turn!`;
        }
        for (let i = 0; i < 9; i++) {
            if (oldMatrix[i] == 1)
                MainMatrix.gameField[i] = 'O';
            if (oldMatrix[i] == 0)
                MainMatrix.gameField[i] = 'X';
            if (oldMatrix[i].toString() == '-1')
                MainMatrix.gameField[i] = ' ';
        }
        showMatrix();
    });
}
window.addEventListener("unload", () => Disconnected()); /* пользователь закрыл вкладку */
/* (или пытается закрыть вкладку, так как это действие можно отменить)*/
function Disconnected() {
    return __awaiter(this, void 0, void 0, function* () {
        fetch('https://localhost:7025/Game/Disconnected?id=' + player.id.toString(), {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
        });
    });
}
