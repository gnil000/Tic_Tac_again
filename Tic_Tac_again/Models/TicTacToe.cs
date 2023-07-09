namespace Tic_Tac_again.Models
{
    public class TicTacToe
    {
        public int Id { get; set; }

        public bool isGameOver { get; set; }
        public bool IsDraw { get; set; }

        public Client Client1 { get; set; }
        public Client Client2 { get; set; }

        public int[] field { get; set; }
        //-1 - cell is empty
        //0 - player1
        //1 - player2

        private int moves;

        public TicTacToe()
        {
            moves = 9;
            field = new int[9];
            for(int i = 0; i < 9; i++)
                field[i] = -1;
            isGameOver = false;
            IsDraw = false;
            Id = DataSource.GetInstance()._games.Count + 1;
        }

        public bool StartGame(int marker, int position)
        {
            if (isGameOver)
                return false;

            SetMarker(marker, position);

            return CheckWinner();
        }


        private bool CheckWinner()
        {
            for (int i = 0; i < 3; i++)
            {
                if (
                    ((field[i * 3] != -1 && field[(i * 3)] == field[(i * 3) + 1] && field[(i * 3)] == field[(i * 3) + 2]) ||
                     (field[i] != -1 && field[i] == field[i + 3] && field[i] == field[i + 6])))
                {
                    isGameOver = true;
                    return true;
                }
            }

            if ((field[0] != -1 && field[0] == field[4] && field[0] == field[8]) || (field[2] != -1 && field[2] == field[4] && field[2] == field[6]))
            {
                isGameOver = true;
                return true;
            }

            return false;

        }

        private bool SetMarker(int marker, int position)
        {

            moves--;
            if (moves <= 0)
            {
                isGameOver = true;
                IsDraw = true;
                return false;
            }


            if (position > field.Length || position < 0)
                return false;
            if (field[position] != -1)
                return false;

            field[position] = marker;

            return true;
        }



    }
}
