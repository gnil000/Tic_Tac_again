using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Tic_Tac_again.Models;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class GameController : Controller
    {
        private readonly ClientService _clientContext;
        private readonly TicTacToeService _tictactoeContext;

        public GameController(ClientService _clientContext, TicTacToeService _tictactoeContext)
        {
            this._clientContext = _clientContext;
            this._tictactoeContext = _tictactoeContext;
        }


        [HttpGet]
        public async Task GetGame()
        {


        }


        //[HttpPost/*("StartGame")*/]
        //[Route("StartGame")]
        //public async Task<bool> StartGame([FromBody] Client client)
        //{
        //    Debug.WriteLine($"{client.Name}, {client.ConnId}");
        //    return await Game.FindOpponent(_clientContext, _tictactoeContext, client.ConnId);
        //}


        [HttpPost]
        [Route("StartGame")]
        public async Task<bool> StartGame(int id)
        {
            return await Game.FindOpponent(_clientContext, _tictactoeContext, id);
        }

        [HttpPost]
        [Route("SendPosition")]
        public async Task<bool> SendPosition(int id, int position)
        {
            return await Game.Play(_clientContext, _tictactoeContext,id, position);
        }




    }
}
