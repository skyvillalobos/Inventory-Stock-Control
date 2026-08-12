public class ProductService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public List<Product> GetAll()
    {
        return _dbContext.Products.ToList();
    }

    public Product? GetById(int id)
    {
        return _dbContext.Products.FirstOrDefault(product => product.Id == id);
    }

    public Product Create(Product product)
    {
        _dbContext.Products.Add(product);
        _dbContext.SaveChanges();

        return product;
    }

    public bool Update(int id, Product updatedProduct)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            return false;
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.CurrentStock = updatedProduct.CurrentStock;
        product.MinimumStock = updatedProduct.MinimumStock;

        _dbContext.SaveChanges();

        return true;
    }

    public bool AddStock(int id, int quantity)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            return false;
        }

        product.CurrentStock += quantity;
        _dbContext.SaveChanges();

        return true;
    }

    public bool Delete(int id, out string message)
    {
        Product? product = GetById(id);

        if (product == null)
        {
            message = "Producto no encontrado.";
            return false;
        }

        if (product.CurrentStock > 0)
        {
            message =
                "No se puede eliminar el producto porque todavía tiene existencias. Vacía primero el inventario.";

            return false;
        }

        _dbContext.Products.Remove(product);
        _dbContext.SaveChanges();

        message = "Producto eliminado correctamente.";
        return true;
    }
}