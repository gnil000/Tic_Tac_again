using System.ComponentModel;

namespace Tic_Tac_again.Models
{
    public class DataSource
    {
        private static DataSource instance;

        private DataSource() { }

        public static DataSource GetInstance()
        {
            if(instance == null)
                instance = new DataSource();
            return instance;
        }

        public List<Client> _clients = new List<Client>();
        public List<TicTacToe> _games = new List<TicTacToe>();
    }
}
