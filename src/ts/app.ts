//import { MainMatrix, GameState, Matrix } from "./matrix.js";


// enum GameState {
//     Started,
//     Aborted,
//     NotStarted,
//     FinishedByDraw,
//     FinishedByXWin,
//     FinishedByOWin,
// }

type Matrix = {
    gameField: string[];
    //gameState: GameState;
}

let MainMatrix: Matrix ={
    gameField: [' ', ' ', ' ', ' ', ' ', ' ', ' ', ' ', ' '],
    //gameState: 2
};

class Player{
    id:number;
    name: string;
    gameState: number;
    marker: number; //0-X | 1-O
    countWin: number;
    countWinOpponent: number;
    waitMove: boolean;
    opponentName: string;
}

let oldMatrix = [-1,-1,-1,-1,-1,-1,-1,-1,-1];
const player = new Player();

const message: Element = 
    document.querySelector('#game-state-info') as Element;

const messageScore: Element = document.querySelector('#score-info') as Element;

const messageTurn: Element = document.querySelector("#whose_turn") as Element;

const messageOpponentName: Element = document.querySelector("#opponent-name") as Element;

const messageContent: string = 
    message?.textContent as string;
    
const startBtn: Element =
    document.querySelector("#start-btn") as Element;

const resignBtn: Element =
    document.querySelector("#resign-btn") as Element;

const fields: NodeListOf<Element> = 
    document.querySelectorAll(".field");

//+++++++++++++++++++++++++++++++++++++++
const registrBtn: Element = document.querySelector('#register-btn') as Element;

const playerNameInput = <HTMLInputElement>document.querySelector('#player-name-field');
//+++++++++++++++++++++++++++++++++++++++


let id: number;

fields.forEach((f: Element, i: number) => {
    f.addEventListener('click', (x) => UserInput(i))
});

startBtn.addEventListener('click', () => StartGame());
resignBtn.addEventListener('click', () => Disconnected());

//+++++++++++++++++++++++++++++++++++++++
registrBtn.addEventListener('click', x=>Registration(playerNameInput.value));

//+++++++++++++++++++++++++++++++++++++++

async function StartGame() {
    let url = "https://localhost:7025/Game/StartGame?id="+player.id.toString();
    fetch(url, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
        },
    })
    .then(data => data.json())
    .then(json => {
        player.marker=json.marker;
        player.waitMove = json.waitMove;
        player.opponentName = json.opponentName;

        messageOpponentName.innerHTML = `Your opponent: ${player.opponentName}`;

        if(player.marker == 0){
            messageTurn.innerHTML = 'Your turn!';
            alert('Your turn!');
        }
        else if(player.marker==1){
            messageTurn.innerHTML = `${player.opponentName} turn!`;
            alert('Opponent turn!');
            RequestGameField();
        }
        
    });
}



function UserInput(i: number): void{
    player.waitMove=true;

    console.log('UserInput!');
    //if(MainMatrix.gameState == GameState.Started){
        let p = fields[i].children.item(0) as HTMLTextAreaElement;
        //console.log(p);
        if (p.textContent == " " || p.textContent=="") {
            //console.log('in if');
            if(player.marker==0){
                p.append("X");
                MainMatrix.gameField[i] = "X";
            }
            else if(player.marker==1){
                p.append("O");
                MainMatrix.gameField[i] = "O";
            }
            //console.log(`set to ${i}`)
            MakeMove(i);
        }
    //}
}

function MakeMove(i: number){
    if(player.waitMove==false){
        messageTurn.innerHTML = 'Youre turn!';
    }
    else if(player.waitMove==true){
        messageTurn.innerHTML = `${player.opponentName} turn!`;}

    fetch('https://localhost:7025/Game/SendPosition?id='+player.id.toString()+'&position='+i.toString(), {
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
        oldMatrix=json.field;
        
        showMatrix();
        RequestGameField();
        GetGameField();
    });
}

function showMatrix(){
    for (let i = 0; i < 9; i++) {
       // for (let j = 0; j < 3; j++) {
            let p = fields[i].children.item(0) as HTMLTextAreaElement;
            p.textContent = MainMatrix.gameField[i];
            console.log(p.textContent);
       // }
    }
}

function Registration(name:string){
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
        player.id = json.connId as number;
        player.gameState = json.isWin as number;
        player.marker = json.marker as number; 
    });
}


//+++++++++++++++++++++++++++++++++++++++
function RequestGameField(){
    console.log('Request send to server');
    fetch('https://localhost:7025/Game/WaitFirstMove?id='+player.id.toString(),{
        method: 'GET'
    })
    .then(data=>data.json())
    .then(json=>{   
        player.countWin = json.youWinCounter;
        player.countWinOpponent = json.opponentWinCounter;
        player.waitMove = json.waitMove;
        player.gameState = json.isWin;

        oldMatrix=json.field;

        GetGameField();
    })
}

async function GetGameField(){
       if(player.marker==0){
        messageScore.innerHTML = `Score: X - ${player.countWin}, O - ${player.countWinOpponent}`;}
    else{
        messageScore.innerHTML = `Score: X - ${player.countWinOpponent}, O - ${player.countWin}`;}

    if(player.waitMove==false){
        messageTurn.innerHTML = 'Your turn!';
    }
    else if(player.waitMove==true){
        messageTurn.innerHTML = `${player.opponentName} turn!`;}

       for(let i = 0;i<9;i++){
            if(oldMatrix[i]==1)
                MainMatrix.gameField[i]='O';
            if (oldMatrix[i]==0)
                MainMatrix.gameField[i]='X';
            if(oldMatrix[i].toString()=='-1')
                MainMatrix.gameField[i]=' ';
       }
       showMatrix();
}

window.addEventListener("unload", ()=>Disconnected()) /* пользователь закрыл вкладку */ 
     /* (или пытается закрыть вкладку, так как это действие можно отменить)*/ 


async function Disconnected(){
    fetch('https://localhost:7025/Game/Disconnected?id='+player.id.toString(), {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
    })
}