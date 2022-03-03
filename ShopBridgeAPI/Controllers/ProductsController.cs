using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShopBridgeAPI.Models;
using ShopBridgeAPI.Models.Dtos;
using ShopBridgeAPI.Repository.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShopBridgeAPI.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;

        public ProductsController(IProductRepository productRepository, IMapper mapper)
        {
            _productRepository = productRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetProducts()
        {
            var objList = _productRepository.GetProducts();
            var objDto = new List<ProductDto>();
            foreach (var obj in objList)
            {
                objDto.Add(_mapper.Map<ProductDto>(obj));
            }
            return Ok(objDto);
        }

        [HttpGet("{productId:int}", Name = "GetProduct")]
        public IActionResult GetProduct(int productId)
        {
            var obj = _productRepository.GetProduct(productId);
            if(obj == null)
            {
                return NotFound();
            }
            var objDto = _mapper.Map<ProductDto>(obj);
            return Ok(objDto);
        }

        [HttpPost]
        public IActionResult CreateProduct([FromBody] ProductDto productDto)
        {
            if(productDto == null)
            {
                return BadRequest(ModelState);
            }
            var productObj = _mapper.Map<Product>(productDto);
            if(!_productRepository.CreateProduct(productObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {productObj.Name}");
                return StatusCode(500, ModelState);
            }
            return CreatedAtRoute("GetProduct", new { productId = productObj.Id }, productObj);
        }

        [HttpPatch("{productId:int}", Name = "UpdateProduct")]
        public IActionResult UpdateProduct(int productId, [FromBody]ProductDto productDto)
        {
            if (productDto == null || productId != productDto.Id)
            {
                return BadRequest(ModelState);
            }
            var productObj = _mapper.Map<Product>(productDto);
            if (!_productRepository.UpdateProduct(productObj))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {productObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{productId:int}", Name = "DeleteProduct")]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRepository.ProductExists(productId))
            {
                return NotFound();
            }
            var productObj = _productRepository.GetProduct(productId);
            if (!_productRepository.DeleteProduct(productObj))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {productObj.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
