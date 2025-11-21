using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductManagement.API.Data;
using ProductManagement.API.Models;
using ProductManagement.API.DTOs;
using ProductManagement.API.Services;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ProductPdfService _pdfService;

        public ProductsController(ApplicationDbContext context, ProductPdfService pdfService)
        {
            _context = context;
            _pdfService = pdfService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts(
            [FromQuery] string? search = null,
            [FromQuery] bool? estado = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var query = _context.Products.AsQueryable();

            // Aplicar filtros
            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(p => p.Nombre.Contains(search) || 
                                        (p.Descripcion != null && p.Descripcion.Contains(search)));
            }

            if (estado.HasValue)
            {
                query = query.Where(p => p.Estado == estado.Value);
            }

            var totalCount = await query.CountAsync();
            var products = await query
                .OrderBy(p => p.Nombre)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    Estado = p.Estado,
                    UsuarioCreacion = p.UsuarioCreacion,
                    FechaCreacion = p.FechaCreacion,
                    UsuarioModificacion = p.UsuarioModificacion,
                    FechaModificacion = p.FechaModificacion
                })
                .ToListAsync();

            var response = new
            {
                Products = products,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize)
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var productDto = new ProductDto
            {
                Id = product.Id,
                Nombre = product.Nombre,
                Descripcion = product.Descripcion,
                Precio = product.Precio,
                Estado = product.Estado,
                UsuarioCreacion = product.UsuarioCreacion,
                FechaCreacion = product.FechaCreacion,
                UsuarioModificacion = product.UsuarioModificacion,
                FechaModificacion = product.FechaModificacion
            };

            return productDto;
        }

        [HttpPost]
        public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto createProductDto)
        {
            var product = new Product
            {
                Id = Guid.NewGuid(),
                Nombre = createProductDto.Nombre,
                Descripcion = createProductDto.Descripcion,
                Precio = createProductDto.Precio,
                Estado = createProductDto.Estado,
                UsuarioCreacion = User.Identity?.Name ?? "System",
                FechaCreacion = DateTime.UtcNow
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            var productDto = new ProductDto
            {
                Id = product.Id,
                Nombre = product.Nombre,
                Descripcion = product.Descripcion,
                Precio = product.Precio,
                Estado = product.Estado,
                UsuarioCreacion = product.UsuarioCreacion,
                FechaCreacion = product.FechaCreacion
            };

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, productDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(Guid id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            product.Nombre = updateProductDto.Nombre;
            product.Descripcion = updateProductDto.Descripcion;
            product.Precio = updateProductDto.Precio;
            product.Estado = updateProductDto.Estado;
            product.UsuarioModificacion = User.Identity?.Name ?? "System";
            product.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(Guid id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("report")]
        public async Task<IActionResult> GenerateReport()
        {
            var products = await _context.Products.ToListAsync();
            var pdfBytes = _pdfService.GeneratePdfReport(products);

            return File(pdfBytes, "application/pdf", $"Reporte_Productos_{DateTime.Now:yyyyMMddHHmmss}.pdf");
        }
    }
}