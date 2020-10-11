using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using APIProxy1.Services;
using APIProxy1.Models;
using System.Threading.Tasks;
using System.Linq;
using FluentAssertions;
using System.Net.Http;
using Microsoft.Extensions.Options;

namespace ApiProxy.Test
{
    [TestClass]
    public class ResourceServiceTest
    {

        [TestMethod]
        public void TestGetUser()
        {
            // arrange
            var resourceServiceMock = new Mock<IResourceService>();
            resourceServiceMock.Setup(rs => rs.GetUser(It.IsAny<string>()))
                .Returns(new UserModel { name = "test", token = "1234-455662-22233333-3333" });

            // act
            var res = resourceServiceMock.Object.GetUser(It.IsAny<string>());

            // assert
            Assert.AreEqual(res.name, "test");
            Assert.AreEqual(res.token, "1234-455662-22233333-3333");

        }

        [TestMethod]
        public async Task TestGetShopperHistory_Sort()
        {
            // arrange
            var listmock = new[]{
                new ShopperHistoryModel
                {
                    customerId = "111",
                    products = new[]
                    {
                        new ProductModel {name = "c", price = 1.01d, quantity = 4d},
                        new ProductModel {name = "a", price = 1.07d, quantity = 12d},
                        new ProductModel {name = "d", price = 0.99d, quantity = 3d},
                        new ProductModel {name = "b", price = 1.04d, quantity = 2d},
                    }
                }
            };

            var expectedAsc = listmock;
            expectedAsc[0].products = expectedAsc[0].products.OrderBy(p => p.name).ToArray();

            var expectedDesc = listmock;
            expectedAsc[0].products = expectedAsc[0].products.OrderByDescending(p => p.name).ToArray();

            var expectedHigh = listmock;
            expectedAsc[0].products = expectedAsc[0].products.OrderBy(p => p.price).ToArray();

            var expectedLow = listmock;
            expectedAsc[0].products = expectedAsc[0].products.OrderByDescending(p => p.price).ToArray();

            var expectedRec = listmock;
            expectedAsc[0].products = expectedAsc[0].products.OrderByDescending(p => p.quantity).ToArray();

            var resourceServiceMock = new Mock<ResourceService>(It.IsAny<HttpClient>(), It.IsAny<IOptions<Config>>());
            resourceServiceMock.Setup(rs => rs.FetchShopperHistory())
                .ReturnsAsync(listmock);

            // act
            var resAsc = await resourceServiceMock.Object.GetShopperHistorySortedBy(SortType.Ascending);
            var resDesc = await resourceServiceMock.Object.GetShopperHistorySortedBy(SortType.Descending);
            var resHigh = await resourceServiceMock.Object.GetShopperHistorySortedBy(SortType.High);
            var resLow = await resourceServiceMock.Object.GetShopperHistorySortedBy(SortType.Low);
            var resRec = await resourceServiceMock.Object.GetShopperHistorySortedBy(SortType.Recommended);

            // assert
            expectedAsc.Should().BeEquivalentTo(resAsc);
            expectedDesc.Should().BeEquivalentTo(resDesc);
            expectedHigh.Should().BeEquivalentTo(resHigh);
            expectedLow.Should().BeEquivalentTo(resLow);
            expectedRec.Should().BeEquivalentTo(resRec);
        }

        [TestMethod]
        public void TestTrolleyTotal()
        {
            // arrange
            var trolley = new TrolleyModel
            {
                products = new[]
                {
                    new ProductModel {name = "c", price = 1.01d},
                    new ProductModel {name = "a", price = 1.07d},
                    new ProductModel {name = "d", price = 0.99d},
                    new ProductModel {name = "b", price = 1.04d},
                },
                specials = new[]
                {
                    new SpecialModel {
                        quantities = new[]
                        {
                            new QuantityModel { name = "e", quantity = 3d },
                            new QuantityModel { name = "f", quantity = 5d },
                        },
                        total = 25d
                    }
                },
                quantities = new[]
                {
                    new QuantityModel { name = "a", quantity = 2d },
                    new QuantityModel { name = "b", quantity = 4d },
                    new QuantityModel { name = "c", quantity = 1d },
                    new QuantityModel { name = "d", quantity = 5d }
                }
            };

            // act
            var rs = new ResourceService(It.IsAny<HttpClient>(), It.IsAny<IOptions<Config>>());
            decimal total = rs.TrolleyTotal(trolley.products, trolley.specials, trolley.quantities);

            // assert
            Assert.AreEqual((decimal)37.26, total);
        }
    }
}
