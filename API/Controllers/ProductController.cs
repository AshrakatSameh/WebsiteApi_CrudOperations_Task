using API.Dtos;
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

        public ProductController(IGenericRepo<Product> productRepo,
            IGenericRepo<Category> categoryRepo)
        {
            _productRepo = productRepo;
            _categoryRepo = categoryRepo;
        }

        [HttpGet]
        public async Task<ActionResult<IReadOnlyList<Product>>> GetAllProducts()
        {
            return Ok(await _productRepo.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            return await _productRepo.GetByIdAsync(id);
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
                CategoryId = productDto.CategoryId,
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
