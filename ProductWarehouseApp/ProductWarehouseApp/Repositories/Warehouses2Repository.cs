using System.Data;
using System.Data.SqlClient;
using ProductWarehouseApp.Dto;

namespace ProductWarehouseApp.Repositories;

public class Warehouses2Repository : IWarehouses2Repository
{
    private IConfiguration _configuration;
    public Warehouses2Repository(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<int> InsertProductWarehouse(ProductWarehouseDto productWarehouseDto)
    {
        await using (var con = new SqlConnection(_configuration.GetConnectionString("Default")))
        {

            using var com = new SqlCommand("AddProductToWarehouse", con)
            {
                CommandType = CommandType.StoredProcedure
            };

            com.Parameters.AddWithValue("@IdProduct", productWarehouseDto.IdProduct);
            com.Parameters.AddWithValue("@IdWarehouse", productWarehouseDto.IdWarehouse);
            com.Parameters.AddWithValue("@Amount", productWarehouseDto.Amount);
            com.Parameters.AddWithValue("@CreatedAt", productWarehouseDto.CreatedAt);

            await con.OpenAsync();

            var primaryKey = Convert.ToInt32(await com.ExecuteScalarAsync());
            return primaryKey;
        }
    }
}