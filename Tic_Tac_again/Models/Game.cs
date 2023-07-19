using System.Diagnostics;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Models
{
    public static class Game
    {
        //private List<TicTacToe> games = new List<TicTacToe>();

        //public static List<Client> clients = new List<Client>();

        

        //TicTacToeService _games;
        //ClientService _clients;

        //Game(TicTacToeService context, ClientService clients)
        //{
        //    _games = context;
        //    _clients = clients;
        //}

        //public static async Task<bool> StartGame(int id)
        //{
        //    return await FindOpponent(id);
        //}

        public static async Task<bool> FindOpponent(ClientService _clients, TicTacToeService _games, int id)
        {
            Random random = new Random();
            var client = _clients.GetClient(id).Result;

            if (client == null)
                return false;

            client.SetSearchGame(true);
            int i = 0;
            Client? opponent = null;
            while (opponent == null) {
                opponent = _clients.GetClients().Result.Where(x => x.isPlaying == false && x.isSearchGame == true && x != client).FirstOrDefault();
                Task.Delay(1000).Wait();
                if (i++ == 10)
                {
                    client.SetSearchGame(false);
                    return false;
                }
            }
            client.SetSearchGame(false);

            object locker = new();

            lock (locker) {
                if (_games.GetGame(id).Result == null)
                {
                    if (random.Next(0, 2) == 0)
                    {
                        client.WaitMove = false;
                        client.SetMarker(0);
                        opponent.WaitMove = true;
                        opponent.SetMarker(1);
                    }
                    else
                    {
                        client.WaitMove = true;
                        client.SetMarker(1);
                        opponent.WaitMove = false;
                        opponent.SetMarker(0);
                    }

                    client.SetIsPlaying(true);
                    opponent.SetIsPlaying(true);

                    client.Opponent = opponent;
                    opponent.Opponent = client;

                    _games.AddGame(new TicTacToe { Client1 = client, Client2 = opponent });
                }
                else
                    return true;
            }
            return true;
        }


        public static async Task<bool> Play(ClientService _clients, TicTacToeService _games, int clientId, int position)
        {
            var game = _games.GetGames().Result.FirstOrDefault(x => x.Client1.ConnId == clientId || x.Client2.ConnId == clientId);

            var player = _clients.GetClient(clientId).Result;

            if (game.isGameOver && game.IsDraw)
            {
                game.Client1.SetIsWin(-1);
                game.Client2.SetIsWin(-1);
                //_games.RemoveGame(game);
                return false;
            }

            if (game.Client1.WaitMove == false)
            {
                if(game.StartGame(game.Client1.Marker, position))
                {
                    game.Client1.SetIsWin(1);
                    game.Client2.SetIsWin(0);
                    //_games.RemoveGame(game);
                    return false;
                }
            }
            else if(game.Client2.WaitMove == false)
            {
                if (game.StartGame(game.Client2.Marker, position))
                {
                    game.Client2.SetIsWin(1);
                    game.Client1.SetIsWin(0);
                    //_games.RemoveGame(game);
                    return false;
                }
            }

            if (!game.isGameOver)
            {
                player.WaitMove = !player.WaitMove;
                player.Opponent.WaitMove = !player.Opponent.WaitMove;
            }

            _games.UpdateTicTacToeState(game);
            //_clients.UpdateClientState(player);

            return true;
        }


    }
}
