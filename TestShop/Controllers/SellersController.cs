using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestShop.Database;
using TestShop.Models;

namespace TestShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SellersController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SellersController (ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Seller>>> GetSellers()
        {
            return await _context.Sellers.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Seller>> GetSellerById(int id)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(m => m.Id == id);
            if (seller == null)
            {
                return NotFound();
            }

            return Ok(seller);
        }

        [HttpPost]
        public async Task<ActionResult<Seller>> CreateSeller(SellerDto sellerDto)
        {
            var newSeller = new Seller
            {
                Name = sellerDto.Name,
            };
            _context.Sellers.Add(newSeller);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSellerById", new { id = newSeller.Id }, newSeller);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSeller (int id,SellerDto sellerDto)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound("Такого продавца нет");
            }

            seller.Name = sellerDto.Name;
            await _context.SaveChangesAsync();
            return Ok(seller);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSeller(int id)
        {
            var seller = await _context.Sellers.FindAsync(id);
            if (seller == null)
            {
                return NotFound("Такого продавца нет");
            }
            _context.Sellers.Remove(seller);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
