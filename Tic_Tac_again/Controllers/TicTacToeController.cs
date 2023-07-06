using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tic_Tac_again.Models;

namespace Tic_Tac_again.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicTacToeController : Controller
    {
        //bool k = true;
      //  Game game = new Game();


        [HttpGet]
        public async Task StartGame()
        {


        }




        //[HttpGet]
        //public async Task<int[]> CreateGame()
        //{
        //    var game = new TicTacToe();
        //    Game.games.Add(game);
        //    Debug.WriteLine(Game.games.Count);

        //    return await Task.FromResult(game.field);
        //}



        //[HttpPost]
        //public async Task<bool> StartGame(int position)
        //{
        //    if (k)
        //    {
        //        k = false;
        //        Debug.WriteLine($"Count of list {Game.games.Count}");
        //        return await Task.FromResult(Game.games[0].StartGame(0, position));
        //    }
        //    else
        //    {
        //        k = true;
        //        Debug.WriteLine(Game.games.Count);

        //        return await Task.FromResult(Game.games[0].StartGame(1, position));
        //    }
        //}



    }
}
