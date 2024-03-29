﻿using System.Diagnostics;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Models
{
    public static class Game
    {
        /// <summary>
        /// Метод для ожидания изменений в игровой сессии и отправке этих изменений если они случатся.
        /// </summary>
        /// <param name="_clients"></param>
        /// <param name="_games"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public static bool WaitFirstMove(ClientService _clients, TicTacToeService _games, int id)
        {
            var client = _clients.GetClient(id);
            var game = _games.GetGames().FirstOrDefault(x => x.Client1.ConnId == id || x.Client2.ConnId == id);
            int[] oldField = new int[9];
            for (int i = 0; i < 9; i++)
                oldField[i] = game.field[i];
            while (true)
            {
                for (int i = 0; i < 9; i++)
                    if (game.field[i] != oldField[i])
                    {
                        oldField[i] = game.field[i];
                        return true;
                    }
                    Task.Delay(1000).Wait();
            }
        }

        /// <summary>
        /// Метод для поиска противника
        /// </summary>
        /// <param name="_clients">объект для управления списком игроков</param>
        /// <param name="_games">объект для управления списком игр</param>
        /// <param name="id">айди клиента</param>
        /// <returns>true = всё удачно прошло, false = что-то пошло не так на каком-то из этапов</returns>
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
                opponent = _clients.GetClients().Where(x => x.isPlaying == false && x.isSearchGame == true && x != client).FirstOrDefault();
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
            return true;
        }

        /// <summary>
        /// Метод для совершения хода.
        /// </summary>
        /// <param name="_clients"></param>
        /// <param name="_games"></param>
        /// <param name="clientId"></param>
        /// <param name="position">позиция от 0 до 8</param>
        /// <returns>true = всё удачно прошло, false = что-то пошло не так на каком-то из этапов</returns>
        public static bool Play(ClientService _clients, TicTacToeService _games, int clientId, int position)
        {
            var game = _games.GetGames().FirstOrDefault(x => x.Client1.ConnId == clientId || x.Client2.ConnId == clientId);

            var player = _clients.GetClient(clientId);

            if (player.WaitMove == true) //если запрос отправил игрок который не должен сейчас ходить
                return false;

            if (game.isGameOver && game.IsDraw)
            {
                game.Client1.SetIsWin(-1);
                game.Client2.SetIsWin(-1);
                return false;
            }

            if (game.Client1.WaitMove == false)
            {
                if (game.StartGame(game.Client1.Marker, position))
                {
                    game.Client1.SetIsWin(1);
                    game.Client2.SetIsWin(0);
                    game.WinClient1 += 1;
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
                    return false;
                }
            }

            if (!game.isGameOver)
            {
                player.WaitMove = !player.WaitMove;
                player.Opponent.WaitMove = !player.Opponent.WaitMove;
            }

            return WaitFirstMove(_clients, _games,clientId);//для ожидания хода второго игрока
        }

        /// <summary>
        /// Метод для разрыва соединения с клиентом, удаления его из списка игроков и удаления его существующей игры из списка активных игр
        /// </summary>
        /// <param name="_clients"></param>
        /// <param name="_games"></param>
        /// <param name="id"></param>
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
