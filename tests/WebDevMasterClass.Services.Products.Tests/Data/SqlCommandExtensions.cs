using System.Data.Common;
using Microsoft.Data.SqlClient;

namespace WebDevMasterClass.Services.Products.Tests.Data;

public static class SqlCommandExtensions
{
    public static async Task<int> AddProduct(this DbCommand cmd,
        string name,
        string description,
        decimal price,
        bool isFeatured,
        string imageName)
    {
        var sqlCmd = cmd as SqlCommand;
        if (sqlCmd is null)
            throw new Exception();
    
        sqlCmd.CommandText = "INSERT INTO Products (Name, Description, Price, IsFeatured, ThumbnailUrl, ImageUrl) " +
                             "VALUES (@Name, @Description, @Price, @IsFeatured, @ThumbnailUrl, @ImageUrl); " +
                             "SELECT SCOPE_IDENTITY();";
                          
        sqlCmd.Parameters.AddWithValue("@Name", name);
        sqlCmd.Parameters.AddWithValue("@Description", description);
        sqlCmd.Parameters.AddWithValue("@Price", price);
        sqlCmd.Parameters.AddWithValue("@IsFeatured", isFeatured);
        sqlCmd.Parameters.AddWithValue("@ThumbnailUrl", $"{imageName}_thumbnail.jpg");
        sqlCmd.Parameters.AddWithValue("@ImageUrl", $"{imageName}.jpg");

        var ret = Convert.ToInt32(await sqlCmd.ExecuteScalarAsync());

        sqlCmd.Parameters.Clear();

        return ret;
    }
}