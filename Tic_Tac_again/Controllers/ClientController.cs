using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tic_Tac_again.Models;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientsController : Controller
    {
        private readonly ClientService _context;

        public ClientsController(ClientService context)
        {
            _context = context;
        }

        /// <summary>
        /// Предназначен для получения списка всех клиентов.
        /// </summary>
        /// <returns>Возвращает список всех зарегестрированных клиентов с полной информацией.</returns>
        [HttpGet]
        public List<Client> Clients()
        {
            return _context.GetClients();
        }

        /// <summary>
        /// Предназначен для получения клиента по айди
        /// </summary>
        /// <param name="id">Айди клиента который вы получаете после регистрации</param>
        /// <returns>Возвращает объект клиента</returns>
        [HttpGet ("{id}")]
        public Client? Clients(int id)
        {
            var client =  _context.GetClient(id);
            if (client == null)
                return null;
            return client;
        }

        /// <summary>
        /// Метод для регистрации клиента.
        /// </summary>
        /// <param name="client">Вы должны отправить только имя</param>
        /// <returns>Возвращает объект клиента</returns>
        [HttpPost]
        public Client? Clients([FromBody]Client client)
        {
            var result =  _context.AddClient(client);
            if (result == null)
                return null;
            return result;
        }
    }
}