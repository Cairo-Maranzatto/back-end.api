using AgendeMe.Data.Context;
using AgendeMe.Data.Entity;
using AgendeMe.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgendeMe.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteController : ControllerBase
    {
        private readonly ILogger<ClienteController> _logger;
        private readonly ApplicationDbContext _context;

        public ClienteController(ILogger<ClienteController> logger, ApplicationDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Cliente
        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10)
        {
            IQueryable<ClienteEntity> query = _context.Clientes.AsQueryable();

            int totalItems = query.Count();
            int totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

            var clientes = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var paginationMetadata = new
            {
                totalItems,
                totalPages,
                currentPage = page,
                pageSize
            };

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(paginationMetadata));

            return Ok(clientes);
        }

        // GET: api/Cliente/{parametro}
        [HttpGet("{parametro}")]
        public IActionResult Get(string parametro)
        {
            List<ClienteEntity> clientes = _context.Clientes
                .Where(c => c.Nome.Contains(parametro) || c.Email.Contains(parametro))
                .ToList();

            if (clientes.Count == 0)
                return NotFound();

            return Ok(clientes);
        }

        // POST: api/Cliente
        [HttpPost]
        public IActionResult Post([FromBody] ClienteModel clienteModel)
        {

            ClienteEntity clienteExiste = _context.Clientes.FirstOrDefault(c => c.Email == clienteModel.Email);
            if (clienteExiste != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "Email já cadastrado!" });

            ClienteEntity cliente = new ClienteEntity();

            cliente.Id = Guid.NewGuid();
            cliente.Nome = clienteModel.Nome;
            cliente.Email = clienteModel.Email;
            cliente.DataNascimento = DateTime.ParseExact(clienteModel.DataNascimento, "dd/MM/yyyy", null);
            cliente.DataCadastro = DateTime.Now;
            _context.Clientes.Add(cliente);
            _context.SaveChanges();

            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        // PUT: api/Cliente/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] ClienteEntity cliente)
        {
            ClienteEntity existingCliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (existingCliente == null)
                return NotFound();

            existingCliente.Nome = cliente.Nome;
            existingCliente.Email = cliente.Email;
            existingCliente.DataNascimento = cliente.DataNascimento;
            existingCliente.DataAlteracao = DateTime.Now;

            _context.SaveChanges();

            return NoContent();
        }

        // DELETE: api/Cliente/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ClienteEntity cliente = _context.Clientes.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            _context.Clientes.Remove(cliente);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
