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
            //Debug.WriteLine($"{_clients.GetClients().Result.Count}");
            var client = _clients.GetClient(id).Result;
           // Debug.WriteLine($"CLIENTS IS EMPTY: {client.Name}");
            if (client == null)
            {
                //client = new Client();
                //clients.Add(client);
                return false;
            }

            var opponent = _clients.GetClients().Result.Where(x => x.isPlaying == false && x != client).FirstOrDefault();
            
            if(opponent == null)
                return false;

            if (random.Next(0, 1) == 0)
            {
                client.WaitMove = false;
                client.Marker = 0;
                opponent.WaitMove = true;
                opponent.Marker = 1;
            }
            else
            {
                client.WaitMove = true;
                client.Marker = 1;
                opponent.WaitMove = false;
                opponent.Marker = 0;
            }
            


            client.Opponent = opponent;
            opponent.Opponent = client;


            await _games.AddGame(new TicTacToe { Client1 = client, Client2 = opponent});
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
                _games.RemoveGame(game);
                return false;
            }

            if (game.Client1.WaitMove == false)
            {
                if(game.StartGame(game.Client1.Marker, position))
                {
                    game.Client1.SetIsWin(1);
                    game.Client2.SetIsWin(0);
                    _games.RemoveGame(game);
                    return false;
                }
                game.Client1.WaitMove = true;
                game.Client2.WaitMove = false;

            }
            else if(game.Client2.WaitMove == false)
            {
                if (game.StartGame(game.Client2.Marker, position))
                {
                    game.Client2.SetIsWin(1);
                    game.Client1.SetIsWin(0);
                    _games.RemoveGame(game);
                    return false;
                }
                game.Client1.WaitMove = false;
                game.Client2.WaitMove = true;
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
