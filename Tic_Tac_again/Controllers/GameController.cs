using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Text.Json;
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
        public async Task<TicTacToe> GetGame(int id)
        {
            return await _tictactoeContext.GetGame(id);

        }


        [HttpGet]
        [Route("GetField")]
        public async Task<GameStateStruct> GetField(int id)
        { //id client 
            var tictac = _tictactoeContext.GetGame(id).Result;
            return await Task.FromResult(new GameStateStruct(tictac, id));
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
        public async Task<ActionResult<Client>> StartGame(int id)
        {
            //bool result = await Game.FindOpponent(_clientContext, _tictactoeContext, id);
            //if(result)
            //    return Ok(result);
            //return BadRequest(result);
            await Game.FindOpponent(_clientContext, _tictactoeContext, id);
            return await Task.FromResult(_clientContext.GetClient(id).Result);
        }

        //[HttpPost]
        //[Route("SendPosition")]
        //public async Task<bool> SendPosition(int id, int position)
        //{
        //    return await Game.Play(_clientContext, _tictactoeContext,id, position);
        //}



        //[HttpPost]
        //[Route("StartGame")]
        //public async Task<bool> StartGame([FromBody] int id)
        //{
        //    return await Game.FindOpponent(_clientContext, _tictactoeContext, id);
        //}

        [HttpPost]
        [Route("SendPosition")]
        public async Task<bool> SendPosition(int id, int position)
        {
            
            return await Game.Play(_clientContext, _tictactoeContext, id, position);
        }



    }
}
