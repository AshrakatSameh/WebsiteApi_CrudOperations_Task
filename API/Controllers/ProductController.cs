using API.Dtos;
using AutoMapper;
using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IGenericRepo<Product> _productRepo;
        private readonly IGenericRepo<Category> _categoryRepo;
        private readonly IMapper _mapper;

        public ProductController(IGenericRepo<Product> productRepo,
            IGenericRepo<Category> categoryRepo,
            IMapper mapper)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<ProductDto>>> GetAllProducts()
        {
            var product = await _productRepo.GetAllWithIncludesAsync(p =>
            p.Category);
            var productToReturn = _mapper.Map<IEnumerable<ProductDto>>(product);
            return Ok(productToReturn);
        }

        //Using Specification pattern for includes
        //[HttpGet("{id}")]
        //public async Task<ActionResult<ProductDto>> GetProduct(int id)
        //{
        //    var product = await _productRepo.GetByIdAsync2(id, p => p.Category);
        //    var productToReturn = _mapper.Map<ProductDto>(product);
        //    return productToReturn;
        //}

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProduct(int id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            var productToReturn = _mapper.Map<ProductDto>(product);
            return productToReturn;
        }

        [HttpPost]
        public async Task<ActionResult> AddProduct(ProductDto productDto)
        {
            var product = new Product
            {
                Name = productDto.Name,
                Description = productDto.Description,
                Price = productDto.Price,
                PictureUrl = productDto.PictureUrl,
                DiscountAmount = productDto.DiscountAmount,
                CategoryId = productDto.CategoryId,
                //should enter an existing ID in category table
                
            };
            await _productRepo.AddAsync(product);
            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        [HttpPut("{id}")]
       
        public async Task<ActionResult> UpdateProduct(int id, ProductDto proDto)
        {
            var existingProduct = await _productRepo.GetByIdAsync(id);

            if (existingProduct == null)
            {
                return NotFound();
            }

            // Update only the allowed properties
            existingProduct.Name = proDto.Name;
            existingProduct.Description = proDto.Description;
            existingProduct.Price = proDto.Price;
            existingProduct.DiscountAmount = proDto.DiscountAmount;

            // Update the product in the repository
            await _productRepo.UpdateAsync(existingProduct);

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            await _productRepo.DeleteAsync(id);
            return NoContent();
        }

        [HttpGet("category")]
        public async Task<ActionResult<IReadOnlyList<Category>>> GetCategory()
        {
            return Ok(await _categoryRepo.GetAllAsync());
        }

    }
}
