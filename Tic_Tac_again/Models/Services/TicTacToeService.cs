﻿namespace Tic_Tac_again.Models.Services
{
    public class TicTacToeService
    {

        public TicTacToe AddGame(TicTacToe game)
        {
            DataSource.GetInstance()._games.Add(game);
            return game;
        }

        public TicTacToe? GetGame(int id)
        {
            return DataSource.GetInstance()._games.FirstOrDefault(x => x.Client1.ConnId == id || x.Client2.ConnId==id);
        }

        public List<TicTacToe> GetGames() {
            return DataSource.GetInstance()._games;
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