using Microsoft.EntityFrameworkCore;
using mini_pos.Core.Consts;
using mini_pos.Core.Dtos;
using mini_pos.Core.ServiceResponse;
using mini_pos.Core.ServiceResponse.ServiceFailure;
using mini_pos.Core.ServiceResponse.ServiceSuccess;
using mini_pos.Features.Users.Dtos;
using ms_sql;

namespace mini_pos.Features.Users.Services;

internal class MerchantService(PosContext db) : IMerchantService
{
    private IQueryable<Admin> MerchantDb =>
        db.Admins.Where(admin => admin.Role == nameof(AdminRole.Merchant)).AsQueryable();

    public async Task<IServiceResponse<PagedResult<MerchantDto>>> List(MerchantFilter filter)
    {
        try
        {
            var skip = filter.PageNumber * filter.Limit;
            var take = filter.Limit;

            var query = db.Admins.AsNoTracking().AsQueryable();
            if (filter.SearchTerm != null)
                query = query.Where(admin =>
                    admin.Role == nameof(AdminRole.Merchant) && admin.Username.Contains(filter.SearchTerm));

            var totalCount = await query.CountAsync();
            var merchants = await query
                .OrderByDescending(admin => admin.CreatedAt)
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            var merchantDtos = merchants
                .Select(merchant => new MerchantDto(
                    merchant.Id,
                    merchant.Username,
                    merchant.Email,
                    merchant.Role
                ))
                .ToList();
            var result = new PagedResult<MerchantDto>(merchantDtos, totalCount, filter.PageNumber, filter.Limit);
            return new Ok<PagedResult<MerchantDto>>("Merchants retrieved successfully", result);
        }
        catch (Exception e)
        {
            return new InternalServerError<PagedResult<MerchantDto>>(e.Message);
        }
    }

    public async Task<IServiceResponse<MerchantDto>> GetById(Guid id)
    {
        try
        {
            var merchant =
                await MerchantDb.FirstOrDefaultAsync(merchant => merchant.Id == id);
            if (merchant is null) return new NotFound<MerchantDto>("$Merchant with {id} not found");

            var merchantDto = new MerchantDto(
                merchant.Id,
                merchant.Username,
                merchant.Email,
                merchant.Role
            );

            return new Ok<MerchantDto>("Merchant retrieved successfully", merchantDto);
        }
        catch (Exception e)
        {
            return new InternalServerError<MerchantDto>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Create(MerchantCreateDto merchantDto)
    {
        try
        {
            var isEmailExists = await MerchantDb.AnyAsync(merchant => merchant.Email == merchantDto.Email);
            if (isEmailExists) return new BadRequest<ValueTuple>($"Email {merchantDto.Email} already exists");

            var hashedPassword = merchantDto.Password;
            var admin = new Admin
            {
                Username = merchantDto.Username,
                Email = merchantDto.Email,
                PasswordHash = hashedPassword,
                Role = nameof(AdminRole.Merchant)
            };
            db.Admins.Add(admin);
            var result = await db.SaveChangesAsync();
            if (result > 0) return new Created<ValueTuple>("Merchant created successfully");

            return new InternalServerError<ValueTuple>("Merchant creation failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }

    public async Task<IServiceResponse<ValueTuple>> Update(Guid id, MerchantCreateDto merchantDto)
    {
        try
        {
            var merchant = await MerchantDb.FirstOrDefaultAsync(admin => admin.Id == id);
            if (merchant is null) return new NotFound<ValueTuple>("$Merchant with {id} not found");

            merchant.Username = merchantDto.Username;
            merchant.Email = merchantDto.Email;
            merchant.UpdatedAt = DateTime.Now;

            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Merchant updated successfully");

            return new InternalServerError<ValueTuple>("Merchant update failed");
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
            var merchant = await MerchantDb.FirstOrDefaultAsync(admin => admin.Id == id);
            if (merchant is null) return new NotFound<ValueTuple>("$Merchant with {id} not found");

            merchant.DeletedAt = DateTime.Now;
            var result = await db.SaveChangesAsync();
            if (result > 0) return new NoContent<ValueTuple>("Merchant deleted successfully");

            return new InternalServerError<ValueTuple>("Merchant deletion failed");
        }
        catch (Exception e)
        {
            return new InternalServerError<ValueTuple>(e.Message);
        }
    }
}