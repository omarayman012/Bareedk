# Banner CRUD Implementation - Updated to Records

## Summary of Changes

All Banner command classes have been converted to C# records and updated to follow project conventions more closely.

### 1. CreateBannerCommand (Record)

**File**: `BaridikExpress.Application/Features/Banners/Commands/CreateBanner/CreateBannerCommand.cs`

```csharp
public record CreateBannerCommand(
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile Image) : IRequest<Result<Guid>>;
```

**Key Changes**:
- Converted to record with positional parameters
- Image is **required** (not nullable)
- All title/description parameters are nullable

### 2. CreateBannerCommandHandler

**Key Updates**:
- Uses `NormalizeHelper.Normalize()` for automatic title/description fallback
- Image validation is mandatory (400 error if missing)
- Returns empty string for missing descriptions instead of null
- Uses normalized values directly without null coalescing

```csharp
var (titleAr, titleEn) = NormalizeHelper.Normalize(request.TitleAr, request.TitleEn);
var (descriptionAr, descriptionEn) = NormalizeHelper.Normalize(request.DescriptionAr, request.DescriptionEn);
```

### 3. CreateBannerCommandValidator

**Key Updates**:
- Image is marked as `NotNull()` (required)
- Added file extension validation (.jpg, .jpeg, .png, .webp)
- Removed ContentType validation (using extension instead)
- All other fields remain optional with max length validation

### 4. UpdateBannerCommand (Record)

**File**: `BaridikExpress.Application/Features/Banners/Commands/UpdateBanner/UpdateBannerCommand.cs`

```csharp
public record UpdateBannerCommand(
    Guid Id,
    string? TitleAr,
    string? TitleEn,
    string? DescriptionAr,
    string? DescriptionEn,
    IFormFile? Image) : IRequest<Result<bool>>;
```

**Key Changes**:
- Converted to record with positional parameters
- Image is **nullable/optional**
- All other fields are nullable
- Id is required parameter

### 5. UpdateBannerCommandHandler

**Key Updates**:
- Keeps old values if fields are not provided (null/whitespace)
- Uses `NormalizeHelper.Normalize()` after merging with old values
- Only validates title uniqueness if title actually changed
- Only uploads image if a new one is provided
- All updates use the domain `Update()` method

```csharp
var titleAr = string.IsNullOrWhiteSpace(request.TitleAr) ? banner.TitleAr : request.TitleAr;
var titleEn = string.IsNullOrWhiteSpace(request.TitleEn) ? banner.TitleEn : request.TitleEn;
// ... then normalize
(titleAr, titleEn) = NormalizeHelper.Normalize(titleAr, titleEn);
```

**Change Detection**:
```csharp
if (titleAr != banner.TitleAr || titleEn != banner.TitleEn)
{
    // Only validate uniqueness if title changed
}
```

### 6. UpdateBannerCommandValidator

- Image is optional (`When(x => x.Image is not null)`)
- File extension and size validation only applied if image is provided
- All other rules remain the same

### 7. DeleteBannerCommand (Record)

**File**: `BaridikExpress.Application/Features/Banners/Commands/DeleteBanner/DeleteBannerCommand.cs`

```csharp
public record DeleteBannerCommand(List<Guid> Ids) : IRequest<Result<bool>>;
```

**Changes**:
- Converted from class to record
- Simplified single positional parameter

### 8. BannersController Updates

**Update Endpoint Change**:
```csharp
// Creates new record instance with path parameter Id
var updateCommand = new UpdateBannerCommand(
    id,
    command.TitleAr,
    command.TitleEn,
    command.DescriptionAr,
    command.DescriptionEn,
    command.Image);
var result = await mediator.Send(updateCommand);
```

## NormalizeHelper Behavior

The `NormalizeHelper.Normalize()` method:
1. Trims whitespace from both parameters
2. If one is null/empty and the other isn't, copies the non-empty one to both
3. Returns both normalized (never null for non-empty inputs)

**Example**:
```csharp
// Input: titleAr="  Title  ", titleEn=null
// Output: ("Title", "Title")

// Input: titleAr=null, titleEn="English"
// Output: ("English", "English")

// Input: titleAr="???????", titleEn="English"
// Output: ("???????", "English")
```

## Validation Summary

### Create
- **Image**: Required (NotNull)
- **TitleAr**: Optional, max 200 chars
- **TitleEn**: Optional, max 200 chars
- **DescriptionAr**: Optional, max 2000 chars
- **DescriptionEn**: Optional, max 2000 chars
- **File Extension**: Must be .jpg, .jpeg, .png, or .webp

### Update
- **Id**: Required
- **Image**: Optional
- **All text fields**: Optional
- **File Extension**: Only validated if image provided
- **Title Uniqueness**: Only checked if title is being changed

### Delete
- **Ids**: Required, non-empty list

## Endpoint Usage

### Create Banner
```http
POST /api/v1/banners
Content-Type: multipart/form-data

TitleEn: "Summer Sale"
TitleAr: "??? ?????"
Image: [required file]
```

### Update Banner
```http
PUT /api/v1/banners/{id}
Content-Type: multipart/form-data

TitleEn: "Updated Title"  // optional
Image: [optional file]
// If not provided, keeps existing values
```

### Delete Banners
```http
DELETE /api/v1/banners
Content-Type: application/json

{
  "ids": ["guid1", "guid2"]
}
```

## Code Quality

- ? All commands are immutable records
- ? All handlers use IGenericRepository<Banner>
- ? Uses NormalizeHelper for consistent text handling
- ? Proper change detection to avoid unnecessary validations
- ? All error messages localized
- ? Clean separation of concerns
- ? Follows project conventions
- ? .NET 9 compatible
- ? Production-ready

## Compilation Status

All files compile without errors:
- CreateBannerCommand.cs ?
- CreateBannerCommandHandler.cs ?
- CreateBannerCommandValidator.cs ?
- UpdateBannerCommand.cs ?
- UpdateBannerCommandHandler.cs ?
- UpdateBannerCommandValidator.cs ?
- DeleteBannerCommand.cs ?
- DeleteBannerCommandHandler.cs ?
- BannersController.cs ?
