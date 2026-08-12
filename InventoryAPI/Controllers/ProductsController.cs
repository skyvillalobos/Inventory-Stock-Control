using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;

    public ProductsController(ProductService productService)
    {
        _productService = productService;
    }

    // GET /api/products
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_productService.GetAll());
    }

    // GET /api/products/1
    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        Product? product = _productService.GetById(id);

        if (product == null)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return Ok(product);
    }

    // POST /api/products
    [HttpPost]
    public IActionResult Create(Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest(new
            {
                message = "El nombre del producto es obligatorio."
            });
        }

        if (product.Price < 0)
        {
            return BadRequest(new
            {
                message = "El precio no puede ser negativo."
            });
        }

        if (product.CurrentStock < 0 || product.MinimumStock < 0)
        {
            return BadRequest(new
            {
                message = "Las cantidades de inventario no pueden ser negativas."
            });
        }

        Product createdProduct = _productService.Create(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = createdProduct.Id },
            createdProduct
        );
    }

    // PUT /api/products/1
    [HttpPut("{id}")]
    public IActionResult Update(int id, Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest(new
            {
                message = "El nombre del producto es obligatorio."
            });
        }

        if (product.Price < 0 ||
            product.CurrentStock < 0 ||
            product.MinimumStock < 0)
        {
            return BadRequest(new
            {
                message = "El precio y las cantidades no pueden ser negativos."
            });
        }

        bool updated = _productService.Update(id, product);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return Ok(_productService.GetById(id));
    }

    // PUT /api/products/1/add-stock
    [HttpPut("{id}/add-stock")]
    public IActionResult AddStock(int id, AddStockDto request)
    {
        if (request.Quantity <= 0)
        {
            return BadRequest(new
            {
                message = "La cantidad debe ser mayor que cero."
            });
        }

        bool updated = _productService.AddStock(id, request.Quantity);

        if (!updated)
        {
            return NotFound(new
            {
                message = "Producto no encontrado."
            });
        }

        return Ok(new
        {
            message = "Stock agregado correctamente.",
            product = _productService.GetById(id)
        });
    }

    // DELETE /api/products/1
    [HttpDelete("{id}")]
    public IActionResult Delete(int id)
    {
        bool deleted = _productService.Delete(id, out string message);

        if (!deleted)
        {
            Product? product = _productService.GetById(id);

            if (product == null)
            {
                return NotFound(new { message });
            }

            return BadRequest(new { message });
        }

        return Ok(new { message });
    }
}