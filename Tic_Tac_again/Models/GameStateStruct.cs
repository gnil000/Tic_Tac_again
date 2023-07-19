namespace Tic_Tac_again.Models
{
    public struct GameStateStruct
    {
        public int Id { get; set; }

        public string opponentName { get; set; }

        public int YouWin { get; set; }
        public int OpponentWin { get; set; }
        //public string client2Name { get; set; }

        public int isWin { get; set; }

        public bool isGameOver { get; set; }
        public bool isDraw { get; set; }

        public int[] field { get; set; }

        public GameStateStruct(TicTacToe tt, int id)
        {
            Id = tt.Id;
            opponentName = id != tt.Client1.ConnId ? tt.Client1.Name : tt.Client2.Name;
            isWin = id == tt.Client1.ConnId ? tt.Client1.isWin : tt.Client2.isWin;

            isGameOver = tt.isGameOver;
            isDraw = tt.IsDraw;
            field = tt.field;

            YouWin = id == tt.Client1.ConnId ? tt.WinClient1 : tt.WinClient2;
            OpponentWin = id != tt.Client1.ConnId ? tt.WinClient1 : tt.WinClient2;
        }

    }
}
