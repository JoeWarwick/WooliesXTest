using APIProxy1.Models;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Linq;

namespace APIProxy1.Services
{
    public class ResourceService : IResourceService
    {
        private HttpClient _client;
        private Config _settings;

        public ResourceService(HttpClient httpClient, IOptions<Config> options)
        {
            this._client = httpClient;
            this._settings = options?.Value;
        }
        public UserModel GetUser(string requestUrl)
        {
            var res = GetUserFromLocalApi(requestUrl);
            return res;
        }

        public UserModel GetUserFromLocalApi(string requestUrl)
        {
            // Call  API
            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get,
                string.Format("{0}/user", requestUrl));

            //Read server response
            HttpResponseMessage response =  _client.SendAsync(newRequest).Result;

            string result = response.Content.ReadAsStringAsync().Result;

            UserModel usr = JsonConvert.DeserializeObject<UserModel>(result);
            return usr;
        }

        public async Task<ShopperHistoryModel[]> GetShopperHistorySortedBy(SortType sort)
        {
            var history = await FetchShopperHistory();
            switch (sort)
            {
                case SortType.Ascending:
                    foreach(ShopperHistoryModel shp in history)
                    {
                        shp.products = shp.products.OrderBy(p => p.name).ToArray();
                    }
                    break;
                case SortType.Descending:
                    foreach (ShopperHistoryModel shp in history)
                    {
                        shp.products = shp.products.OrderByDescending(p => p.name).ToArray();
                    }
                    break;
                case SortType.High:
                    foreach (ShopperHistoryModel shp in history)
                    {
                        shp.products = shp.products.OrderBy(p => p.price).ToArray();
                    }
                    break;
                case SortType.Low:
                    foreach (ShopperHistoryModel shp in history)
                    {
                        shp.products = shp.products.OrderByDescending(p => p.price).ToArray();
                    }
                    break;
                default:
                    foreach (ShopperHistoryModel shp in history)
                    {
                        shp.products = shp.products.OrderByDescending(p => history
                            .Sum(h => h.products
                                .Sum(p1 => p1.name == p.name ? p1.quantity : 0)
                            )).ToArray();
                    }
                    break;
            }
            return history;
        }

        public virtual async Task<ShopperHistoryModel[]> FetchShopperHistory()
        {
            // Call  API
            HttpRequestMessage newRequest = new HttpRequestMessage(HttpMethod.Get, 
                string.Format("{0}shopperHistory?token={1}", _settings.ResourceApiBaseUrl, _settings.IdentityToken));

            //Read server response
            HttpResponseMessage response = await _client.SendAsync(newRequest);

            string result = await response.Content.ReadAsStringAsync();
            
            ShopperHistoryModel[] shm = JsonConvert.DeserializeObject<ShopperHistoryModel[]>(result);
            return shm;
        }

        public decimal TrolleyTotal(ProductModel[] products, SpecialModel[] specials, QuantityModel[] quantities)
        {
            var productsTotal = products.Sum(p => p.price * quantities.FirstOrDefault(q => q.name == p.name).quantity);
            var specialsTotal = specials.Sum(s => s.total);

            return (decimal)(productsTotal + specialsTotal);
        }
    }
}
