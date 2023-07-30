namespace Tic_Tac_again.Models
{
    public struct GameStateStruct
    {
        public int Id { get; set; }

        public string opponentName { get; set; }

        public int YouWinCounter { get; set; }
        public int OpponentWinCounter { get; set; }

        public int isWin { get; set; }

        public bool isGameOver { get; set; }
        public bool isDraw { get; set; }

        public bool WaitMove { get; set; }

        public int marker { get; set; }

        public int[] field { get; set; }

        public GameStateStruct(TicTacToe tt, int id)
        {
            Id = tt.Id;
            opponentName = id != tt.Client1.ConnId ? tt.Client1.Name : tt.Client2.Name;
            isWin = id == tt.Client1.ConnId ? tt.Client1.isWin : tt.Client2.isWin;

            isGameOver = tt.isGameOver;
            isDraw = tt.IsDraw;
            field = tt.field;

            marker = id == tt.Client1.ConnId ? tt.Client1.Marker : tt.Client2.Marker;

            WaitMove = id == tt.Client1.ConnId ? tt.Client1.WaitMove : tt.Client2.WaitMove;

            YouWinCounter = id == tt.Client1.ConnId ? tt.WinClient1 : tt.WinClient2;
            OpponentWinCounter = id != tt.Client1.ConnId ? tt.WinClient1 : tt.WinClient2;
        }
    }
}
