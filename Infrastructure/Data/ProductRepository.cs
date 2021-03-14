using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class ProductRepository : IProductRepository
    {
        private readonly StoreContext _context;
        public ProductRepository(StoreContext context)
        {
            _context = context;
        }

        public async Task<IReadOnlyList<ProductBrand>> GetProductBrandsAsync()
        {
            return await _context.ProductBrands.ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IReadOnlyList<Product>> GetProductsAsync(FilterSort queryObj)
        {
            var query = _context.Products
            .Include(p => p.ProductType)
            .Include(p => p.ProductBrand)
            .AsQueryable();
            if (!string.IsNullOrEmpty(queryObj.Search))
                query = query.Where(x => x.Name.ToLower().Contains(queryObj.Search));
            if (queryObj.brandId.HasValue)
                query = query.Where(v => v.ProductBrandId == queryObj.brandId);
            if (queryObj.typeId.HasValue)
                query = query.Where(v => v.ProductTypeId == queryObj.typeId);

            if (queryObj.SortBy == "priceAsc")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Name) : query.OrderByDescending(v => v.Name);
            if (queryObj.SortBy == "priceDesc")
                query = (queryObj.IsSortAscending) ? query.OrderBy(v => v.Price) : query.OrderByDescending(v => v.Price);



            return await query.ToListAsync();

        }

        public async Task<IReadOnlyList<ProductType>> GetProductTypesAsync()
        {
            return await _context.ProductTypes.ToListAsync();
        }
    }
}