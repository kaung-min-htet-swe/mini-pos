using Microsoft.EntityFrameworkCore;
using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Core.ServiceResponse.ServiceFailure;
using mini_pos.Core.ServiceResponse.ServiceSuccess;
using mini_pos.Features.Products.Dtos;
using ms_sql;

namespace mini_pos.Features.Products.Services;

public class CategoryService(PosContext db) : ICategoryService
{
    public async Task<IServiceResponse<PagedResult<CategoryResponseDto>>> List(CategoryFilter filter)
    {
        try
        {
            var skip = (filter.PageNumber - 1) * filter.Limit;
            var take = filter.Limit;
            var query = db.Categories.AsNoTracking().AsQueryable();

            if (filter.SearchTerm != null) query = query.Where(c => c.Name.Contains(filter.SearchTerm));
            var totalCount = await query.CountAsync();

            var categories = await query
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var categoryResponseDtos = categories.Select(c => new CategoryResponseDto
            (
                c.Id,
                c.Name,
                c.Description
            )).ToList();

            var result =
                new PagedResult<CategoryResponseDto>(categoryResponseDtos, totalCount, filter.PageNumber, filter.Limit);
            return new Ok<PagedResult<CategoryResponseDto>>("Categories retrieved successfully", result);
        }
        catch (Exception e)
        {
            return new InternalServerError<PagedResult<CategoryResponseDto>>(e.Message);
        }
    }

    public async Task<IServiceResponse<CategoryResponseDto>> GetById(Guid id)
    {
        try
        {
            var category = await db.Categories.Include(c => c.Products).FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return new NotFound<CategoryResponseDto>("$Category with {id} not found");

            var products = category.Products
                .Select(p => new ProductResponseDto(p.Id, p.Name, p.Price, p.Sku, p.StockQuantity)).ToList();

            var categoryResponseDto =
                new CategoryResponseDto(category.Id, category.Name, category.Description, products);
            return new Ok<CategoryResponseDto>("Category retrieved successfully", categoryResponseDto);
        }
        catch (Exception e)
        {
            return new InternalServerError<CategoryResponseDto>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Create(CreateCategoryRequestDto request)
    {
        try
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            db.Categories.Add(category);
            var result = await db.SaveChangesAsync();
            if (result > 0) return new Created<ValueTuple>("Category created successfully");

            return new InternalServerError<ValueTuple>("Category creation failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Update(Guid id, CreateCategoryRequestDto request)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return new NotFound<ValueTuple>("$Category with {id} not found");

            category.Name = request.Name;
            category.Description = request.Description;
            category.UpdatedAt = DateTime.Now;

            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Category updated successfully");

            return new InternalServerError<ValueTuple>("Category update failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Delete(Guid id)
    {
        try
        {
            var category = await db.Categories.FirstOrDefaultAsync(c => c.Id == id);
            if (category == null)
                return new NotFound<ValueTuple>("$Category with {id} not found");

            category.DeletedAt = DateTime.Now;
            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Category deleted successfully");

            return new InternalServerError<ValueTuple>("Category deletion failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }
}