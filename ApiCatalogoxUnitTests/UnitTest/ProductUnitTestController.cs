using APICatalogo.Context;
using APICatalogo.DTO.Mapping;
using APICatalogo.Repositories;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ApiCatalogoxUnitTests.UnitTest;

public class ProductUnitTestController
{

    public IUnitOfWork repository;
    public IMapper mapper;
    public static DbContextOptions<AppDbContext> dbContextOptions { get; }

    public static string connectionString =
        "Server=localhost;Database=apicatalagodb;User id=root;Pwd=1234567";
                                   
    static ProductUnitTestController()
    {
        dbContextOptions = new DbContextOptionsBuilder<AppDbContext>().UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)).Options;
    }

    public ProductUnitTestController()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new ProductDTOMappingProfile());


        });

        mapper = config.CreateMapper();
        var context = new AppDbContext(dbContextOptions);

        repository = new UnitOfWork(context);
    }

}
