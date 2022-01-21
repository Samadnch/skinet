using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infastructure.Data;
using Core.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Core.Interfaces;
using Core.Specifications;
using API.DTOs;
using AutoMapper;
using API.Errors;
using Microsoft.AspNetCore.Http;

namespace API.Controllers
{
    public class ProductsController : BaseApiController
    {
        private readonly IGenericRepository<Product> _productsRepo;
        private readonly IGenericRepository<ProductBrand> _productBrandRepo;
        private readonly IGenericRepository<ProductType> _productTypeRepo;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> ProductsRepo 
                                 ,IGenericRepository<ProductBrand> ProductBrandRepo
                                 ,IGenericRepository<ProductType> ProductTypeRepo
                                 ,IMapper mapper )
        {
            _productsRepo = ProductsRepo;
            _productBrandRepo = ProductBrandRepo;
            _productTypeRepo = ProductTypeRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts(){
            var spec = new ProductsWithTypesAndBrandsSpecification();
            var products = await _productsRepo.ListAsync(spec);
            return Ok(

                _mapper.Map< IReadOnlyList<Product> , IReadOnlyList<ProductToReturnDto> >(products)
            );
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int Id){
              var spec = new ProductsWithTypesAndBrandsSpecification(Id);
            var product = await _productsRepo.GetEntityWithSpec(spec);

            if(product == null ) return NotFound(new ApiResponse(404));
            return _mapper.Map< Product , ProductToReturnDto >(product);
           
        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands(){

            return Ok(await _productBrandRepo.ListAllAsync());
        }

        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProductTypes(){

           return Ok( await _productTypeRepo.ListAllAsync());
        }
    }
}