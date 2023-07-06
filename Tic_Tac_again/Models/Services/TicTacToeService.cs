namespace Tic_Tac_again.Models.Services
{
    public class TicTacToeService
    {

        public async Task<TicTacToe> AddGame(TicTacToe game)
        {
            DataSource.GetInstance()._games.Add(game);
            return await Task.FromResult(game);
        }

        public async Task<TicTacToe> GetGame(int id)
        {
            return await Task.FromResult(DataSource.GetInstance()._games.First(x => x.Client1.ConnId == id || x.Client2.ConnId==id));
        }

        public async Task<List<TicTacToe>> GetGames() {
            return await Task.FromResult(DataSource.GetInstance()._games);
        }

        public void UpdateTicTacToeState(TicTacToe game)
        {
            DataSource.GetInstance()._games.Remove(DataSource.GetInstance()._games.First(x => x.Id == game.Id));
            DataSource.GetInstance()._games.Add(game);
        }

        public void RemoveGame(TicTacToe game)
        {
            DataSource.GetInstance()._games.Remove(game);
        }
    }
}