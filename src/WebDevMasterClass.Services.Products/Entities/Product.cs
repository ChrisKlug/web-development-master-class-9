namespace WebDevMasterClass.Services.Products.Entities;

public record Product(int Id,
    string Name,
    string Description,
    decimal Price,
    bool IsFeatured,
    string ThumbnailUrl,
    string ImageUrl);