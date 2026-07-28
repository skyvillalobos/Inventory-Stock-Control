public class ProductService
{
    private readonly List<Product> _products = new();

    public List<Product> GetAll()
    {
        return _products;
    }

    public Product? GetById(int id)
    {
        return _products.FirstOrDefault(product => product.Id == id);
    }

    public Product Create(Product product)
    {
        product.Id = _products.Count == 0
            ? 1
            : _products.Max(product => product.Id) + 1;

        _products.Add(product);

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

        _products.Remove(product);

        message = "Producto eliminado correctamente.";
        return true;
    }
}