using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tic_Tac_again.Models;
using Tic_Tac_again.Models.Services;

namespace Tic_Tac_again.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ClientController : Controller
    {
        private readonly ClientService _context;

        public ClientController(ClientService context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Client>>> Client()
        {
            return await _context.GetClients();
        }

        [HttpGet ("{id}")]
        public async Task<ActionResult<Client>> Client(int id)
        {
            var client =  _context.GetClient(id);
            if (client == null)
                return NotFound();
            return await Task.FromResult(client);
        }

        [HttpPost]
        public async Task<ActionResult<Client>> Client([FromBody]Client client)
        {
            var result = await _context.AddClient(client);
            if (result == null)
                BadRequest();
            return await Task.FromResult(result);
        }
    }
}