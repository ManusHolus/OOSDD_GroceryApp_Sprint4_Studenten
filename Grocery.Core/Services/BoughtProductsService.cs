
using Grocery.Core.Interfaces.Repositories;
using Grocery.Core.Interfaces.Services;
using Grocery.Core.Models;



namespace Grocery.Core.Services
{
    public class BoughtProductsService : IBoughtProductsService
    {
        private readonly IGroceryListItemsRepository _groceryListItemsRepository;
        private readonly IClientRepository _clientRepository;
        private readonly IProductRepository _productRepository;
        private readonly IGroceryListRepository _groceryListRepository;
        public BoughtProductsService(IGroceryListItemsRepository groceryListItemsRepository, IGroceryListRepository groceryListRepository, IClientRepository clientRepository, IProductRepository productRepository)
        {
            _groceryListItemsRepository=groceryListItemsRepository;
            _groceryListRepository=groceryListRepository;
            _clientRepository=clientRepository;
            _productRepository=productRepository;
        }
        //code added for the list of best bought products
        public List<BoughtProducts> Get(int? productId)
        {
            // Checking if the productID is null
            if (productId == null)
                return new List<BoughtProducts>();

            // Picks up items with the specific ID
            var itemsWithProduct = _groceryListItemsRepository
                .GetAll()
                .Where(item => item.ProductId == productId)
                .ToList();

            
            var product = _productRepository.Get(productId.Value);

            var result = new List<BoughtProducts>();

            foreach (var item in itemsWithProduct)
            {
                var groceryList = _groceryListRepository.Get(item.GroceryListId);
                if (groceryList == null) continue;

                var client = _clientRepository.Get(groceryList.ClientId);
                if (client == null) continue;

                result.Add(new BoughtProducts(client, groceryList, product ?? new Product(0, "Onbekend", 0)));
            }

            return result;
        }
    }
    }

