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

public class GetProductUnitTest : IClassFixture<ProductUnitTestController>
{
    private readonly ProductsController _controller;

    public GetProductUnitTest(ProductUnitTestController controller)
    {
        _controller = new ProductsController(controller.repository, controller.mapper);
    }

    [Fact]
    public async Task GetProductById_OKResult()
    {
        //Arrange
        var prodId = 1;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>() //verifica se o resultado é OkObject result
            .Which.StatusCode.Should().Be(200);// verifica se o códgio de status é 200
    }

    [Fact]
    public async Task GetProductById_Return_NotFound()
    {
        //Arrange
        var prodId = 999;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<NotFoundObjectResult>() //verifica se o resultado é NotFoundObjectResult 
            .Which.StatusCode.Should().Be(404); //verifica se o códgio de status é 404
    }

    [Fact]
    public async Task GetProductById_Return_BadRequest()
    {
        //Arrange
        var prodId = -1;

        //Act
        var data = await _controller.Get(prodId);

        //Assert
        data.Result.Should().BeOfType<BadRequestObjectResult>() //verifica se o resultado é BadRequest 
            .Which.StatusCode.Should().Be(400); //verifica se o códgio de status é 400
    }

    [Fact]
    public async Task GetProduct_Return_ListenOfProductDTO()
    {

        //Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<OkObjectResult>()
            .Which.Value.Should().BeAssignableTo<IEnumerable<ProductDTO>>().And.NotBeNull();
    }

    [Fact]
    public async Task GetProduct_Return_BadRequestResult()
    {

        //Act
        var data = await _controller.Get();

        //Assert
        data.Result.Should().BeOfType<BadRequestResult>();
    }

}
