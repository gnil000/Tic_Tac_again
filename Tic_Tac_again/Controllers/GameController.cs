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

        /// <summary>
        /// Предназначен для ожидания изменения игрового поля. После начала игры, если ваш маркер = 1 вызываете этот метод, дальше изменения поля идут из метода SendPosition. 
        /// </summary>
        /// <param name="id">Айди клиента</param>
        /// <returns>Возвращает всю игровую информацию о данной игре</returns>
        [HttpGet]
        [Route("WaitFirstMove")]
        public GameStateStruct WaitFirstMove(int id)
        {
            Game.WaitFirstMove(_clientContext, _tictactoeContext, id);
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        /// <summary>
        /// Предназначен для поиска противника.
        /// </summary>
        /// <param name="id">отправляется айди клиента</param>
        /// <returns>Возвращает всю игровую информацию о данной игре</returns>
        [HttpPost]
        [Route("StartGame")]
        public GameStateStruct StartGame(int id)
        {
            Game.FindOpponent(_clientContext, _tictactoeContext, id);
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        /// <summary>
        /// Предназначен для отправления позиции поставленного клиентом хода и ожидания хода противника. Работает по приницпу long polling.
        /// </summary>
        /// <param name="id">Айди клиента</param>
        /// <param name="position">Позиция на поле от 0 до 8</param>
        /// <returns>Возвращает всю игровую информацию о данной игре</returns>
        [HttpPost]
        [Route("SendPosition")]
        public GameStateStruct SendPosition(int id, int position)
        {
            Game.Play(_clientContext, _tictactoeContext, id, position);
            
            var tictac = _tictactoeContext.GetGame(id);
            return new GameStateStruct(tictac, id);
        }

        /// <summary>
        /// Предназначен для разрыва соединения клиента и сервера.
        /// </summary>
        /// <param name="id">Айди клиента</param>
        [HttpPost]
        [Route("Disconnected")]
        public void Desconnected(int id)
        {
            Game.PlayerDisconnected(_clientContext, _tictactoeContext, id);
        }
    }
}
