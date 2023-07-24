using Microsoft.AspNetCore.Mvc;
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
        public TicTacToe? GetGame(int id)
        {
            return _tictactoeContext.GetGame(id);
        }

        [HttpGet]
        [Route("GetField")]
        public GameStateStruct GetField(int id)
        {
            Game.WaitFirstMove(_clientContext, _tictactoeContext, id);
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        [HttpPost]
        [Route("StartGame")]
        public GameStateStruct StartGame(int id)
        {
            Game.FindOpponent(_clientContext, _tictactoeContext, id);
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        [HttpPost]
        [Route("SendPosition")]
        public GameStateStruct SendPosition(int id, int position)
        {
            Game.Play(_clientContext, _tictactoeContext, id, position);
            
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        [HttpPost]
        [Route("Disconnected")]
        public void Desconnected(int id)
        {
            Game.PlayerDisconnected(_clientContext, _tictactoeContext, id);
        }
    }
}
