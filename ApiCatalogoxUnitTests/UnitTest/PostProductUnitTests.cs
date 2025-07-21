using APICatalogo.Controllers;
using APICatalogo.DTO;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTest;

public class PostProductUnitTests : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller;

    public PostProductUnitTests(ProductUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task PostProduct_Return_CreatedStatusCode()
    {
        var newProduct = new ProductDTO
        {
            Name = "Test",
            Description = "TestDescription",
            Price = 1,
            ImageUrl = "Test.jpg",
            CategoryId = 1
        };

        var data = await _controller.Post(newProduct);

        var createdResult = data.Result.Should().BeOfType<CreatedAtRouteResult>();
        createdResult.Subject.StatusCode.Should().Be(201);
    }

    [Fact]
    public async Task PostProduct_Return_BadRequest()
    {
        ProductDTO prod = null;

        var data = await _controller.Post(prod);

        var badRequestResult = data.Result.Should().BeOfType<BadRequestResult>();
        badRequestResult.Subject.StatusCode.Should().Be(400);
    }
}
