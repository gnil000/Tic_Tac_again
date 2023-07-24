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

        [HttpGet]
        public List<Client> Clients()
        {
            return _context.GetClients();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Client>> Clients(int id)
        {
            var client =  _context.GetClient(id);
            if (client == null)
                return NotFound();
            return await Task.FromResult(client);
        }

        [HttpPost]
        public Client Clients([FromBody]Client client)
        {
            var result =  _context.AddClient(client);
            if (result == null)
                BadRequest();
            return result;
        }
    }
}