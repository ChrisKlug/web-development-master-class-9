using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebDevMasterClass.Services.Products.Tests.Data;
internal static class SqlCommandExtensions
{
    public static async Task<int> AddProduct(this SqlCommand cmd,
                                            string name,
                                            string description,
                                            decimal price,
                                            bool isFeatured,
                                            string imageName)
    {
        cmd.CommandText = "INSERT INTO Products (Name, Description, Price, IsFeatured, ThumbnailUrl, ImageUrl) " +
                          "VALUES (@Name, @Description, @Price, @IsFeatured, @ThumbnailUrl, @ImageUrl); " +
                          "SELECT SCOPE_IDENTITY();";

        cmd.Parameters.AddWithValue("@Name", name);
        cmd.Parameters.AddWithValue("@Description", description);
        cmd.Parameters.AddWithValue("@Price", price);
        cmd.Parameters.AddWithValue("@IsFeatured", isFeatured);
        cmd.Parameters.AddWithValue("@ThumbnailUrl", $"{imageName}_thumbnail.jpg");
        cmd.Parameters.AddWithValue("@ImageUrl", $"{imageName}.jpg");

        var ret = Convert.ToInt32(await cmd.ExecuteScalarAsync());

        cmd.Parameters.Clear();

        return ret;
    }
}
