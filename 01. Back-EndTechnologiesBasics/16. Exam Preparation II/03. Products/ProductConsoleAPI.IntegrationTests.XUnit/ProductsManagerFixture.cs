using ProductConsoleAPI.Business;
using ProductConsoleAPI.Business.Contracts;
using ProductConsoleAPI.DataAccess;

namespace ProductConsoleAPI.IntegrationTests.XUnit
{
    public class ProductsManagerFixture : IDisposable
    {
        public ProductsManagerFixture()
        {
            DbContext = new TestProductsDbContext();
            var productsRepository = new ProductsRepository(DbContext);
            ProductsManager = new ProductsManager(productsRepository);
        }

        public TestProductsDbContext DbContext { get; private set; }
        public IProductsManager ProductsManager { get; private set; }

        public void Dispose()
        {
            DbContext.Dispose();
        }
    }
}
