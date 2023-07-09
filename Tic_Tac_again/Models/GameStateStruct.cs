namespace Tic_Tac_again.Models
{
    public struct GameStateStruct
    {
        public int Id { get; set; }

        public string opponentName { get; set; }
        //public string client2Name { get; set; }

        public int isWin { get; set; }

        public bool isGameOver { get; set; }
        public bool isDraw { get; set; }

        public int[] field { get; set; }

        public GameStateStruct(TicTacToe tt, int id)
        {
            Id = tt.Id;
            opponentName = id != tt.Client1.ConnId ? tt.Client1.Name : tt.Client2.Name;
            //client2Name = tt.Client2.Name;
            isWin = id == tt.Client1.ConnId ? tt.Client1.isWin : tt.Client2.isWin;

            //if (tt.Client2.ConnId == id)
            //    isWin = tt.Client2.isWin;
            //else
            //    isWin = tt.Client1.isWin;

            isGameOver = tt.isGameOver;
            isDraw = tt.IsDraw;
            field = tt.field;

        }

    }
}
