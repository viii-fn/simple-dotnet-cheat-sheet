# C# ASP.NET Core Web API CRUD Cheat Sheet

## 1. Naming and Namespace Conventions

* **Solution / Project Name:** `AppName.Api`
* **Controller File Name:** Plural noun + `Controller.cs` (e.g., `ProductsController.cs`)
* **Namespace:** `AppName.Api.Controllers`
* **Route Template:** `[Route("api/[controller]")]` -> Resolves to `/api/products`

---

## 2. Standard Async Controller Template

```csharp
using Microsoft.AspNetCore.Mvc;

namespace AppName.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // Inject your DbContext or Service here
    public ProductsController() { }

    // 1. READ ALL (GET /api/products)
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductDto>>> GetAll()
    {
        return Ok(await _productService.GetAllAsync());
    }

    // 2. READ ONE (GET /api/products/{id})
    [HttpGet("{id:int}")]
    public async Task<ActionResult<ProductDto>> GetById(int id)
    {
        var product = await _productService.GetByIdAsync(id);
        if (product == null) return NotFound(new { message = "Item not found" });

        return Ok(product);
    }

    // 3. CREATE (POST /api/products)
    [HttpPost]
    public async Task<ActionResult<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        if (!ModelState.IsValid) return BadRequest(ModelState);

        var created = await _productService.CreateAsync(dto);

        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    // 4. UPDATE (PUT /api/products/{id})
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateProductDto dto)
    {
        var updated = await _productService.UpdateAsync(id, dto);
        if (!updated) return NotFound();

        return NoContent();
    }

    // 5. DELETE (DELETE /api/products/{id})
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _productService.DeleteAsync(id);
        if (!deleted) return NotFound();

        return NoContent();
    }
}

// --- DTOs (Data Transfer Objects) ---
public record ProductDto(int Id, string Name, decimal Price);
public record CreateProductDto(string Name, decimal Price);
public record UpdateProductDto(string Name, decimal Price);