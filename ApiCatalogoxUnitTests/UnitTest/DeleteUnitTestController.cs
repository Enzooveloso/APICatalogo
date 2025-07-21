using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTest
{
    public class DeleteUnitTestController : IClassFixture<ProductUnitTestController>
    {
        private readonly ProductsController _controller;

        public DeleteUnitTestController(ProductUnitTestController controller)
        {
            _controller = new ProductsController(controller.repository, controller.mapper);
        }

        [Fact]
        public async Task DeleteProductsById_Return_OkResult()
        {
            var prodId = 1;

            var result = await _controller.Delete(prodId);

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task DeleteProductsById_Return_NotFound()
        {
            var prodId = 999;

            var result = await _controller.Delete(prodId) as ActionResult<ProductDTO>;

            result.Should().NotBeNull();
            result.Result.Should().BeOfType<NotFoundObjectResult>();
        }
    }
}
