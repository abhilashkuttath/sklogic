using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _repo;

        private readonly IMapper _mapper;

        public ProductsController(IProductRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }
        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductToReturnDto>>> GetProducts([FromQuery] FilterSort queryObj)
        {
            //first iteration
            // var products = await _repo.GetProductsAsync(queryObj);
            // return Ok(products);

            //second

            var products = await _repo.GetProductsAsync(queryObj);
            //can use projection
            // return products.Select(product => new ProductToReturnDto
            // {
            //     Id = product.Id,
            //     Name = product.Name,
            //     Description = product.Description,
            //     ProductBrand = product.ProductBrand.Name
            // }).ToList();

            //last good way using AUtoMapper

            return Ok(_mapper.Map<IReadOnlyList<Product>, IReadOnlyList<ProductToReturnDto>>(products));



        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductToReturnDto>> GetProduct(int id)
        {
            var product = await _repo.GetProductByIdAsync(id);

            return Ok(_mapper.Map<Product, ProductToReturnDto>(product));

            //with out automapper
            // var product = await _repo.GetProductByIdAsync(id);
            // return new ProductToReturnDto
            // {
            //     Id=product.Id,
            //     Name = product.Name
            // }

        }

        [HttpGet("brands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetProductBrands()
        {
            return Ok(await _repo.GetProductBrandsAsync());
        }
        [HttpGet("types")]
        public async Task<ActionResult<IReadOnlyList<ProductType>>> GetProducTypes()
        {
            return Ok(await _repo.GetProductTypesAsync());
        }
    }
}