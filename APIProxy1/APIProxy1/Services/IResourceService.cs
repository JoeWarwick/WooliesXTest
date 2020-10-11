using APIProxy1.Models;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace APIProxy1.Services
{
    public interface IResourceService
    {
        public UserModel GetUser(string requestUrl, ILogger log);
        public Task<ShopperHistoryModel[]> GetShopperHistorySortedBy(SortType sort);
        public Task<ShopperHistoryModel[]> FetchShopperHistory();
        public decimal TrolleyTotal(ProductModel[] products, SpecialModel[] specials, QuantityModel[] quantities);
    }
}
