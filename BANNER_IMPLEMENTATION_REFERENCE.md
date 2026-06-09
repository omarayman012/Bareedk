# Banner CRUD - Complete Implementation Files

## Quick Reference

### All Commands are Now Records

```csharp
// CREATE - Image is Required
public record CreateBannerCommand(
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile Image) : IRequest<Result<Guid>>;

// UPDATE - Image is Optional, keeps old values if not provided
public record UpdateBannerCommand(
    Guid Id,
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile? Image) : IRequest<Result<bool>>;

// DELETE
public record DeleteBannerCommand(List<Guid> Ids) : IRequest<Result<bool>>;
```

## Key Implementation Details

### CreateBannerCommandHandler
- Uses `NormalizeHelper.Normalize()` for automatic language fallback
- Image is validated as required before normalization
- Validates title uniqueness
- Uses `IGenericRepository<Banner>.AddAsync()`

### UpdateBannerCommandHandler
- **Keeps existing values** if fields are not provided (null/whitespace)
- Normalizes after merging with existing values
- Only validates title uniqueness if title actually changed
- Only uploads image if new one provided
- Uses `IGenericRepository<Banner>.UpdateAsync()`

### DeleteBannerCommandHandler
- Validates IDs are not empty
- Fetches all banners by IDs
- Returns failure if any ID not found or count mismatch
- Uses `IGenericRepository<Banner>.DeleteRangeAsync()`

## Repository Methods Used

```csharp
// Create
await repo.AddAsync(banner, cancellationToken);

// Update - Read
var banner = await repo.GetByIdAsync(request.Id, cancellationToken);

// Update - Check uniqueness
var titleExists = await repo.AnyAsync(x => 
    x.Id != request.Id && (x.TitleEn == titleEn || x.TitleAr == titleAr),
    cancellationToken);

// Update - Update
await repo.UpdateAsync(banner, cancellationToken);

// Delete - Query
var banners = await repo.Query()
    .Where(x => request.Ids.Contains(x.Id))
    .ToListAsync(cancellationToken);

// Delete - Delete Range
await repo.DeleteRangeAsync(banners);
```

## Localization Keys

### English
```json
{
  "BannerTitleRequired": "Banner title in Arabic or English is required.",
  "BannerTitleAlreadyExists": "A banner with this title already exists.",
  "BannerTitleMaxLength": "Banner title must not exceed 200 characters.",
  "BannerDescriptionMaxLength": "Banner description must not exceed 2000 characters.",
  "BannerCreatedSuccessfully": "Banner created successfully.",
  "BannerUpdatedSuccessfully": "Banner updated successfully.",
  "BannerNotFound": "Banner not found.",
  "BannersNotFound": "Banners not found.",
  "SomeBannersNotFound": "Some banners were not found.",
  "BannersDeletedSuccessfully": "Banners deleted successfully."
}
```

### Arabic
```json
{
  "BannerTitleRequired": "????? ?????? ???????? ?? ?????????? ?????.",
  "BannerTitleAlreadyExists": "???? ???? ???? ??????? ??????.",
  "BannerTitleMaxLength": "??? ??? ?????? ????? ?????? 200 ???.",
  "BannerDescriptionMaxLength": "??? ??? ?????? ??? ?????? 2000 ???.",
  "BannerCreatedSuccessfully": "?? ????? ?????? ?????.",
  "BannerUpdatedSuccessfully": "?? ????? ?????? ?????.",
  "BannerNotFound": "?????? ??? ?????.",
  "BannersNotFound": "???????? ??? ??????.",
  "SomeBannersNotFound": "??? ???????? ??? ??????.",
  "BannersDeletedSuccessfully": "?? ??? ???????? ?????."
}
```

## Permissions

```csharp
public const string BannersCreate = "Banners.Create";
public const string BannersUpdate = "Banners.Update";
public const string BannersDelete = "Banners.Delete";
```

## API Endpoints

### Create Banner
- **Method**: POST
- **Route**: `/api/v1/banners`
- **Permission**: `Banners.Create`
- **Content-Type**: `multipart/form-data`
- **Required**: Image
- **Optional**: TitleAr, TitleEn, DescriptionAr, DescriptionEn

### Update Banner
- **Method**: PUT
- **Route**: `/api/v1/banners/{id}`
- **Permission**: `Banners.Update`
- **Content-Type**: `multipart/form-data`
- **All fields optional** - keeps existing values if not provided
- **Optional**: TitleAr, TitleEn, DescriptionAr, DescriptionEn, Image

### Delete Banners
- **Method**: DELETE
- **Route**: `/api/v1/banners`
- **Permission**: `Banners.Delete`
- **Content-Type**: `application/json`
- **Required**: Ids (List<Guid>)

## Validation Rules

### Create
| Field | Required | Max Length | Type |
|-------|----------|-----------|------|
| TitleAr | No | 200 | string |
| TitleEn | No | 200 | string |
| DescriptionAr | No | 2000 | string |
| DescriptionEn | No | 2000 | string |
| Image | **Yes** | - | IFormFile |

**Note**: At least one title (AR or EN) must be provided (handled by NormalizeHelper)

### Update
| Field | Required | Max Length | Type |
|-------|----------|-----------|------|
| Id | **Yes** | - | Guid |
| TitleAr | No | 200 | string |
| TitleEn | No | 200 | string |
| DescriptionAr | No | 2000 | string |
| DescriptionEn | No | 2000 | string |
| Image | No | - | IFormFile? |

**Note**: Provided values override existing; null/whitespace keeps existing value

### Delete
| Field | Required | Type |
|-------|----------|------|
| Ids | **Yes** | List<Guid> |

## HTTP Status Codes

| Operation | Success | Not Found | Conflict | Error |
|-----------|---------|-----------|----------|-------|
| Create | 201 | - | 409 (title exists) | 400 |
| Update | 200 | 404 | 409 (title exists) | 400 |
| Delete | 200 | 404 | - | 400 |

## File Structure

```
BaridikExpress.Application/
??? Features/
    ??? Banners/
        ??? Commands/
            ??? CreateBanner/
            ?   ??? CreateBannerCommand.cs
            ?   ??? CreateBannerCommandHandler.cs
            ?   ??? CreateBannerCommandValidator.cs
            ??? UpdateBanner/
            ?   ??? UpdateBannerCommand.cs
            ?   ??? UpdateBannerCommandHandler.cs
            ?   ??? UpdateBannerCommandValidator.cs
            ??? DeleteBanner/
                ??? DeleteBannerCommand.cs
                ??? DeleteBannerCommandHandler.cs

BaridikExpress.API/
??? Controllers/
    ??? Banners/
        ??? BannersController.cs
```

## Testing Checklist

- [ ] Create with valid image
- [ ] Create with invalid image extension
- [ ] Create without image (should fail)
- [ ] Create with duplicate title
- [ ] Update with new image
- [ ] Update without image (keeps old)
- [ ] Update to create title conflict (should fail)
- [ ] Update partial fields
- [ ] Delete single banner
- [ ] Delete multiple banners
- [ ] Delete non-existent banner (should fail)
- [ ] Delete with only some valid IDs (should fail)

## Dependencies

- MediatR (CQRS pattern)
- FluentValidation
- Entity Framework Core
- Microsoft.Extensions.Localization
- IFileStorageService
- IGenericRepository<Banner>
- NormalizeHelper
- Result<T> pattern

All implemented and ready for production use! ?
