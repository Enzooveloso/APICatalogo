using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTest
{
    public class PutProductunitTests : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;

        public PutProductunitTests(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task PutProduct_Return_OkResult()
        {
            var prodId = 12;

            var updateProductDTO = new ProductDTO
            {
                ProductID = prodId,
                Name = "Produto att test",
                Description = "Description Test",
                ImageUrl = "test.png",
                CategoryId = 1,

            };
            var result = await _controller.Put(prodId, updateProductDTO) as ActionResult<ProductDTO>;

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }
        [Fact]
        public async Task PutProduct_Return_BadRequest()
        {
            var prodId = 1000;
            var myProduct = new ProductDTO
            {
                ProductID = 1,
                Name = "Produto att test",
                Description = "Description Test",
                ImageUrl = "test.png",
                CategoryId = 2,
            };

            var data = await _controller.Put(prodId, myProduct);

            data.Result.Should().BeOfType<BadRequestResult>().Which.StatusCode.Should().Be(400);

        }
    }
}
