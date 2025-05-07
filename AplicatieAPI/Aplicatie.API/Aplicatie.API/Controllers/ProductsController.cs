using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;
using Aplicatie.API.Models;
using Aplicatie.API.Data;
using System;
using Microsoft.EntityFrameworkCore;

namespace Aplicatie.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(AppDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

       
        public class ProductUploadDto
        {
            public string Name { get; set; }
            public string Description { get; set; }
            public string Sku { get; set; }
            public decimal Price { get; set; }
            public int CategoryId { get; set; }
            public int? DiscountId { get; set; }
            public IFormFile Image { get; set; }
        }

       
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            try
            {
                var categories = await _context.ProductCategories
                    .Where(c => c.DeletedAt == null)
                    .Select(c => new
                    {
                        c.Id,
                        c.Name
                    })
                    .ToListAsync();

                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching categories.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        
        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.DeletedAt == null)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.SKU,
                        p.Price,
                        p.CategoryId,
                        CategoryName = p.Category.Name,
                        p.ImageUrl
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching products.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                var product = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.Id == id && p.DeletedAt == null)
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.SKU,
                        p.Price,
                        p.CategoryId,
                        CategoryName = p.Category.Name,
                        p.ImageUrl
                    })
                    .FirstOrDefaultAsync();

                if (product == null)
                {
                    return NotFound(new { Error = $"Product with Id {id} not found." });
                }

                return Ok(product);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching the product.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        
        [HttpGet("search")]
        public async Task<IActionResult> SearchProducts([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrEmpty(query))
                {
                    return BadRequest(new { Error = "Search query cannot be empty." });
                }

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.DeletedAt == null && p.Name.ToLower().Contains(query.ToLower()))
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.SKU,
                        p.Price,
                        p.CategoryId,
                        CategoryName = p.Category.Name,
                        p.ImageUrl
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while searching products.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        
        [HttpGet("byCategory")]
        public async Task<IActionResult> GetProductsByCategory([FromQuery] string category)
        {
            try
            {
                if (string.IsNullOrEmpty(category))
                {
                    return BadRequest(new { Error = "Category name cannot be empty." });
                }

                var products = await _context.Products
                    .Include(p => p.Category)
                    .Where(p => p.DeletedAt == null && p.Category.Name.ToLower() == category.ToLower())
                    .Select(p => new
                    {
                        p.Id,
                        p.Name,
                        p.Description,
                        p.SKU,
                        p.Price,
                        p.CategoryId,
                        CategoryName = p.Category.Name,
                        p.ImageUrl
                    })
                    .ToListAsync();

                return Ok(products);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while fetching products by category.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }

       
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadProductImage([FromForm] ProductUploadDto productDto)
        {
            try
            {
                if (productDto.Image == null || productDto.Image.Length == 0)
                {
                    return BadRequest("No image file uploaded.");
                }

                if (string.IsNullOrEmpty(productDto.Name) || string.IsNullOrEmpty(productDto.Sku))
                {
                    return BadRequest("Product name and SKU are required.");
                }

                
                var categoryExists = await _context.ProductCategories.AnyAsync(c => c.Id == productDto.CategoryId);
                if (!categoryExists)
                {
                    return BadRequest($"Category with Id {productDto.CategoryId} does not exist.");
                }

                
                var uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads", "products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(productDto.Image.FileName);
                var filePath = Path.Combine(uploadsFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await productDto.Image.CopyToAsync(stream);
                }

                
                var existingProduct = await _context.Products
                    .Include(p => p.Inventory)
                    .FirstOrDefaultAsync(p => p.SKU == productDto.Sku && p.DeletedAt == null);

                int inventoryId;

                if (existingProduct != null)
                {
                   
                    existingProduct.Inventory.Quantity += 1;
                    await _context.SaveChangesAsync();

                    inventoryId = existingProduct.InventoryId;

                   
                    existingProduct.Name = productDto.Name;
                    existingProduct.Description = productDto.Description;
                    existingProduct.Price = productDto.Price;
                    existingProduct.CategoryId = productDto.CategoryId;
                    existingProduct.DiscountId = productDto.DiscountId;
                    existingProduct.ImageUrl = $"/uploads/products/{fileName}";
                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        Message = "Product updated and inventory incremented successfully",
                        ProductId = existingProduct.Id,
                        InventoryId = inventoryId,
                        FileName = fileName,
                        FilePath = existingProduct.ImageUrl
                    });
                }
                else
                {
                    
                    var productInventory = new ProductInventory
                    {
                        Quantity = 1 
                    };

                    _context.ProductInventories.Add(productInventory);
                    await _context.SaveChangesAsync();

                    
                    var product = new Product
                    {
                        Name = productDto.Name,
                        Description = productDto.Description,
                        SKU = productDto.Sku,
                        Price = productDto.Price,
                        CategoryId = productDto.CategoryId,
                        InventoryId = productInventory.Id,
                        DiscountId = productDto.DiscountId,
                        ImageUrl = $"/uploads/products/{fileName}"
                    };

                    _context.Products.Add(product);
                    await _context.SaveChangesAsync();

                    inventoryId = productInventory.Id;

                    return Ok(new
                    {
                        Message = "Product uploaded successfully",
                        ProductId = product.Id,
                        InventoryId = inventoryId,
                        FileName = fileName,
                        FilePath = product.ImageUrl
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "An error occurred while uploading the product.", Details = ex.Message, StackTrace = ex.StackTrace });
            }
        }
    }
}