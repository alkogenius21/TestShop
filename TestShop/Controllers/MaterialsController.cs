using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestShop.Database;
using TestShop.Models;

namespace TestShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MaterialsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Material>>> GetMaterials()
        {
            return await _context.Materials
                .Include(m => m.Seller)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterialById(int id)
        {
            var material = await _context.Materials
                .Include(m => m.Seller)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (material == null)
            {
                return NotFound();
            }

            return Ok(material);
        }

        [HttpPost]
        public async Task<ActionResult<Material>> CreateMaterial(MaterialDto materialDto)
        {
            var seller = await _context.Sellers.FindAsync(materialDto.SellerId);
            if (seller == null)
            {
                return NotFound("Продавец не найден");
            }

            var newMaterial = new Material
            {
                Name = materialDto.Name,
                Price = materialDto.Price,
                SellerId = materialDto.SellerId
            };
            _context.Materials.Add(newMaterial);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMaterialById", new { id = newMaterial.Id }, newMaterial);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMaterial(int id, MaterialDto updatedMaterialDto)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound("Материал не найден");
            }

            var seller = await _context.Sellers.FindAsync(updatedMaterialDto.SellerId);
            if (seller == null)
            {
                return NotFound("Продавец не найден");
            }

            material.Name = updatedMaterialDto.Name;
            material.Price = updatedMaterialDto.Price;
            material.SellerId = updatedMaterialDto.SellerId;
            await _context.SaveChangesAsync();

            return Ok(material);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await _context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound("Материал не найден");
            }

            _context.Materials.Remove(material);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
