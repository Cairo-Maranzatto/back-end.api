using AgendeMe.Data.Context;
using AgendeMe.Data.Entity;
using AgendeMe.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Microsoft.Extensions.Logging;
using AgendeMe.Data.Repository;

namespace AgendeMe.Controllers
{
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class ClienteTelefoneController : ControllerBase
    {
        private readonly ILogger<ClienteTelefoneController> _logger;
        private readonly IRepository<ClienteEntity> _clienteRepository;
        private readonly IRepository<ClienteTelefoneEntity> _clienteTelefoneRepository;

        public ClienteTelefoneController(ILogger<ClienteTelefoneController> logger, IRepository<ClienteEntity> clienteRepository, IRepository<ClienteTelefoneEntity> clienteTelefoneRepository)
        {
            _logger = logger;
            _clienteRepository = clienteRepository;
            _clienteTelefoneRepository = clienteTelefoneRepository;
        }

        // GET: api/ClienteTelefone
        [HttpGet]
        public IActionResult Get(int page = 1, int pageSize = 10)
        {
            var query = _clienteRepository.GetAll();

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

        // GET: api/ClienteTelefone/{parametro}
        [HttpGet("{parametro}")]
        public IActionResult Get(string parametro)
        {
            var clientes = _clienteRepository.Find(c => c.Nome.Contains(parametro) || c.Email.Contains(parametro));

            if (clientes.Count == 0)
                return NotFound();

            return Ok(clientes);
        }

        // POST: api/ClienteTelefone
        [HttpPost]
        public IActionResult Post([FromBody] ClienteModel clienteModel)
        {
            ClienteEntity clienteExiste = _clienteRepository.FirstOrDefault(c => c.Email == clienteModel.Email);
            if (clienteExiste != null)
                return StatusCode((int)HttpStatusCode.InternalServerError, new Response { Status = "Error", Message = "Email já cadastrado!" });

            ClienteEntity cliente = new ClienteEntity
            {
                Id = Guid.NewGuid(),
                Nome = clienteModel.Nome,
                Email = clienteModel.Email,
                DataNascimento = DateTime.ParseExact(clienteModel.DataNascimento, "dd/MM/yyyy", null),
                DataCadastro = DateTime.Now,
                DataAlteracao = DateTime.Now
            };

            _clienteRepository.Add(cliente);

            return CreatedAtAction(nameof(Get), new { id = cliente.Id }, cliente);
        }

        // PUT: api/ClienteTelefone/{id}
        [HttpPut("{id}")]
        public IActionResult Put(Guid id, [FromBody] ClienteEntity cliente)
        {
            ClienteEntity existingCliente = _clienteRepository.FirstOrDefault(c => c.Id == id);
            if (existingCliente == null)
                return NotFound();

            existingCliente.Nome = cliente.Nome;
            existingCliente.Email = cliente.Email;
            existingCliente.DataNascimento = cliente.DataNascimento;
            existingCliente.DataAlteracao = DateTime.Now;

            _clienteRepository.Update(existingCliente);

            return NoContent();
        }

        // DELETE: api/ClienteTelefone/{id}
        [HttpDelete("{id}")]
        public IActionResult Delete(Guid id)
        {
            ClienteEntity cliente = _clienteRepository.FirstOrDefault(c => c.Id == id);
            if (cliente == null)
                return NotFound();

            _clienteRepository.Remove(cliente);
            _clienteTelefoneRepository.RemoveRange(_clienteTelefoneRepository.Find(ct => ct.ClienteId == id));

            return NoContent();
        }
    }
}
