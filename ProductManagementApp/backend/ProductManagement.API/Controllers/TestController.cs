using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace ProductManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult Public()
        {
            return Ok(new { message = "Este es un endpoint público" });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult Protected()
        {
            return Ok(new { 
                message = "Este es un endpoint protegido",
                user = User.Identity?.Name,
                claims = User.Claims.Select(c => new { c.Type, c.Value })
            });
        }

        [HttpPost("test-product")]
        [Authorize]
        public IActionResult TestProduct([FromBody] object productData)
        {
            return Ok(new { 
                message = "Producto recibido",
                data = productData,
                user = User.Identity?.Name
            });
        }
    }
}