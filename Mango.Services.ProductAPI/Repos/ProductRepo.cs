using AutoMapper;
using Mango.Services.ProductAPI.Data;
using Mango.Services.ProductAPI.Models;
using Mango.Services.ProductAPI.Models.Dto;
using Mango.Services.ProductAPI.Repos.IRepos;
using Microsoft.EntityFrameworkCore;

namespace Mango.Services.ProductAPI.Services
{
    
        public class ProductRepo : IProductRepo
        {
            private readonly AppDbContext _dbContext;
            private readonly IMapper _mapper;
            public ProductRepo(AppDbContext dbContext, IMapper mapper)
            {
                _dbContext = dbContext;
                _mapper = mapper;

            }


            async Task<IEnumerable<ProductDto>> IProductRepo.GetProducts()
            {
                IEnumerable<Product> products = await _dbContext.Products.ToListAsync();

                return _mapper.Map<IEnumerable<ProductDto>>(products);
            }

            async Task<ProductDto> IProductRepo.GetProductById(int ProductId)
            {
                Product product = await _dbContext.Products.FindAsync(ProductId);

                return _mapper.Map<ProductDto>(product);
            }

            async Task<ProductDto> IProductRepo.CreateUpdateProduct(ProductDto productDto)
            {
                Product product = _mapper.Map<Product>(productDto);
                if (product.ProductId > 0)
                {
                    _dbContext.Products.Update(product);

                }
                else
                    _dbContext.Products.Add(product);
                await _dbContext.SaveChangesAsync();

                return _mapper.Map<ProductDto>(product);

            }

            async Task<bool> IProductRepo.DeleteProduct(int ProductId)
            {
                try
                {
                    Product product = await _dbContext.Products.FirstOrDefaultAsync(a => a.ProductId == ProductId);
                    if (product == null)
                    {
                        return false;

                    }
                    _dbContext.Products.Remove(product);
                    _dbContext.SaveChanges();
                    return true;
                }
                catch (Exception)
                {

                    return false;
                }
            }
        }
    }
