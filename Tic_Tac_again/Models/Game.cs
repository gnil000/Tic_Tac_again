﻿using System.Diagnostics;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Models
{
    public static class Game
    {

        public static bool WaitFirstMove(ClientService _clients, TicTacToeService _games, int id)
        {
            var client = _clients.GetClient(id);
            var game = _games.GetGames().Result.FirstOrDefault(x => x.Client1.ConnId == id || x.Client2.ConnId == id);
            //int[] oldField1=new int[9];
            //for (int i = 0; i < 9; i++)
            //    oldField1[i] = -1;
            //int[] oldField = game.field;
            //object locker = new();
            int[] oldField = new int[9];
            for (int i = 0; i < 9; i++)
                oldField[i] = game.field[i];
            while (true)
            {
                //lock (locker)
                //{

                for (int i = 0; i < 9; i++)
                    if (game.field[i] != oldField[i])
                    {
                        oldField[i] = game.field[i];
                        return true;
                    }
                    //field[i] = -1;
                    // game.oldField = game.field;


                    Task.Delay(1000).Wait();
                    //Thread.Sleep(1000);
                //}
            }
            
        }

        public static bool FindOpponent(ClientService _clients, TicTacToeService _games, int id)
        {
            Random random = new Random();
            var client = _clients.GetClient(id);

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
                if (_games.GetGame(id) == null)
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
            }
            //if (client.WaitMove)
            //{
            //    var game = _games.GetGames().Result.FirstOrDefault(x => x.Client1.ConnId == id || x.Client2.ConnId == id);
            //    var oldField = game.field;
            //    while (game.field == oldField)
            //    {
            //        Task.Delay(1000).Wait();
            //    }
            //}
            return true;
        }


        public static bool Play(ClientService _clients, TicTacToeService _games, int clientId, int position)
        {
            var game = _games.GetGames().Result.FirstOrDefault(x => x.Client1.ConnId == clientId || x.Client2.ConnId == clientId);

            var player = _clients.GetClient(clientId);

            if (game.isGameOver && game.IsDraw)
            {
                game.Client1.SetIsWin(-1);
                game.Client2.SetIsWin(-1);
                //game.StartNewGame();
                //_games.UpdateTicTacToeState(game);

                //_games.RemoveGame(game);
                return false;
            }

            if (game.Client1.WaitMove == false)
            {
                if (game.StartGame(game.Client1.Marker, position))
                {
                    game.Client1.SetIsWin(1);
                    game.Client2.SetIsWin(0);
                    game.WinClient1 += 1;
                    //_games.UpdateTicTacToeState(game);

                    //_games.RemoveGame(game);
                    return false;
                }
            }
            else if (game.Client2.WaitMove == false)
            {
                if (game.StartGame(game.Client2.Marker, position))
                {
                    game.Client2.SetIsWin(1);
                    game.Client1.SetIsWin(0);
                    game.WinClient2 += 1;
                    //_games.RemoveGame(game);
                    //_games.UpdateTicTacToeState(game);
                    return false;
                }
            }

            if (!game.isGameOver)
            {
                player.WaitMove = !player.WaitMove;
                player.Opponent.WaitMove = !player.Opponent.WaitMove;
            }

            // _games.UpdateTicTacToeState(game);
            //_clients.UpdateClientState(player);
            //var oldField = game.field;
            //game.oldField = game.field;
            //object locker = new();
            //int[] oldField = new int[9];
            //for (int i = 0; i < 9; i++)
            //    oldField[i] = game.field[i];
            //while (true)
            //{
            //    //lock (locker)
            //    //{
            //    for (int i = 0; i < 9; i++)
            //        if (game.field[i] != oldField[i])
            //        {
            //            oldField[i] = game.field[i];
            //            return true;
            //        }

            //    Task.Delay(1000).Wait();
            //        //Thread.Sleep(1000);
            //    //}

            //    //return true;
            //}
            return WaitFirstMove(_clients, _games,clientId);
        }

        public static void PlayerDisconnected(ClientService _clients, TicTacToeService _games, int id)
            {
                var game = _games.GetGame(id);
                if (game != null)
                    _games.RemoveGame(game);
                var client = _clients.GetClient(id);
                if (client != null)
                {
                    _clients.RemoveClient(client);
                    if (client.Opponent != null)
                    {
                        client.Opponent.Opponent = null;
                        client.Opponent.SetIsPlaying(false);
                    }
                }
            }


    }
}
