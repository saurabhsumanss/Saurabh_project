using LibraryAPI.Data;
using LibraryAPI.DTOs;
using LibraryAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ClientsController : ControllerBase
    {
        private readonly LibraryDbContext _context;

        public ClientsController(LibraryDbContext context)
        {
            _context = context;
        }

        // GET: api/Clients
        [HttpGet]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<IEnumerable<ClientDto>>> GetClients()
        {
            var clients = await _context.Clients
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    MembershipDate = c.MembershipDate,
                    IsActive = c.IsActive
                })
                .ToListAsync();

            return Ok(clients);
        }

        // GET: api/Clients/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientDto>> GetClient(int id)
        {
            var client = await _context.Clients
                .Where(c => c.Id == id)
                .Select(c => new ClientDto
                {
                    Id = c.Id,
                    FirstName = c.FirstName,
                    LastName = c.LastName,
                    Email = c.Email,
                    PhoneNumber = c.PhoneNumber,
                    Address = c.Address,
                    MembershipDate = c.MembershipDate,
                    IsActive = c.IsActive
                })
                .FirstOrDefaultAsync();

            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            return Ok(client);
        }

        // POST: api/Clients
        [HttpPost]
        [Authorize(Roles = "Librarian")]
        public async Task<ActionResult<ClientDto>> CreateClient([FromBody] CreateClientDto createClientDto)
        {
            // Check if email already exists
            if (await _context.Clients.AnyAsync(c => c.Email == createClientDto.Email))
            {
                return BadRequest(new { message = "Email already exists" });
            }

            var client = new Client
            {
                FirstName = createClientDto.FirstName,
                LastName = createClientDto.LastName,
                Email = createClientDto.Email,
                PhoneNumber = createClientDto.PhoneNumber,
                Address = createClientDto.Address,
                MembershipDate = DateTime.UtcNow,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            var clientDto = new ClientDto
            {
                Id = client.Id,
                FirstName = client.FirstName,
                LastName = client.LastName,
                Email = client.Email,
                PhoneNumber = client.PhoneNumber,
                Address = client.Address,
                MembershipDate = client.MembershipDate,
                IsActive = client.IsActive
            };

            return CreatedAtAction(nameof(GetClient), new { id = client.Id }, clientDto);
        }

        // PUT: api/Clients/5
        [HttpPut("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> UpdateClient(int id, [FromBody] UpdateClientDto updateClientDto)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            if (updateClientDto.Email != null && updateClientDto.Email != client.Email)
            {
                if (await _context.Clients.AnyAsync(c => c.Email == updateClientDto.Email))
                {
                    return BadRequest(new { message = "Email already exists" });
                }
                client.Email = updateClientDto.Email;
            }

            if (updateClientDto.FirstName != null) client.FirstName = updateClientDto.FirstName;
            if (updateClientDto.LastName != null) client.LastName = updateClientDto.LastName;
            if (updateClientDto.PhoneNumber != null) client.PhoneNumber = updateClientDto.PhoneNumber;
            if (updateClientDto.Address != null) client.Address = updateClientDto.Address;
            if (updateClientDto.IsActive.HasValue) client.IsActive = updateClientDto.IsActive.Value;

            client.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Clients/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Librarian")]
        public async Task<IActionResult> DeleteClient(int id)
        {
            var client = await _context.Clients.FindAsync(id);

            if (client == null)
            {
                return NotFound(new { message = "Client not found" });
            }

            // Check if client has active loans
            var hasActiveLoans = await _context.BookLoans
                .AnyAsync(bl => bl.ClientId == id && bl.Status == "Active");

            if (hasActiveLoans)
            {
                return BadRequest(new { message = "Cannot delete client with active loans" });
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
