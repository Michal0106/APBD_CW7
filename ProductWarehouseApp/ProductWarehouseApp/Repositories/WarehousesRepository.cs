using System.Data;
using System.Data.Common;
using Microsoft.Data.SqlClient;
using ProductWarehouseApp.Dto;

namespace ProductWarehouseApp.Repositories;

public class WarehousesRepository : IWarehousesRepository
{
    private readonly IConfiguration _configuration;

    public WarehousesRepository(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<bool> ProductNotExist(int id)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "select 1 from Product where IdProduct = @idProduct";

            command.Parameters.AddWithValue("@idProduct", id);

            await connection.OpenAsync();

            if (await command.ExecuteScalarAsync() is not null)
            {
                return false;
            }

            return true;
        }
    }
    public async Task<bool> WarehouseNotExist(int id)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "select 1 from Warehouse where IdWarehouse = @idWarehouse";

            command.Parameters.AddWithValue("@idWarehouse", id);

            await connection.OpenAsync();

            if (await command.ExecuteScalarAsync() is not null)
            {
                return false;
            }

            return true;
        }
    }
    public async Task<int> OrderNotExist(ProductWarehouseDto productWarehouse)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "select IdOrder from [Order] where IdProduct = @idProduct " +
                                  "and Amount = @amount and FulfilledAt IS NULL " +
                                  "and CreatedAt<@createdAt";

            command.Parameters.AddWithValue("@idProduct", productWarehouse.IdProduct);
            command.Parameters.AddWithValue("@amount", productWarehouse.Amount);
            command.Parameters.AddWithValue("@createdAt", productWarehouse.CreatedAt);

            await connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();
            if (result is not null)
            {
                return (int)result;
            }

            return -1;
        }
    }
    public async Task UpdateFulfilledAt(int IdOrder)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "update [Order] set FulfilledAt = @fulfilledAt where IdOrder = @idOrder";

            command.Parameters.AddWithValue("@fulfilledAt", DateTime.Now);
            command.Parameters.AddWithValue("@idOrder", IdOrder);
            
            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }
    
    public async Task<decimal> GetPrice(int productId)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "select Price from Product where IdProduct = @idProduct";

            command.Parameters.AddWithValue("@idProduct", productId);

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            if (result is not null)
            {
                return (decimal)result;
            }

            throw new Exception("Price not found!");
        }
    }
    
    public async Task<int> InsertProductWarehouse(ProductWarehouseDto productWarehouse, int orderId)
    {
        await using (var connection = new SqlConnection(_configuration.GetConnectionString("Default")))
        {
            var command = connection.CreateCommand();

            command.CommandText = "insert into Product_Warehouse(IdWarehouse, IdProduct, IdOrder, Amount, Price, CreatedAt) " +
                                  "values (@warehouseId, @productId, @orderId, @amount, @price, @createdAt);" +
                                  "SELECT SCOPE_IDENTITY();";

            decimal price = await GetPrice(productWarehouse.IdProduct);

            command.Parameters.AddWithValue("@warehouseId", productWarehouse.IdWarehouse);
            command.Parameters.AddWithValue("@productId", productWarehouse.IdProduct);
            command.Parameters.AddWithValue("@orderId", orderId);
            command.Parameters.AddWithValue("@amount", productWarehouse.Amount);
            command.Parameters.AddWithValue("@price", productWarehouse.Amount * price);
            command.Parameters.AddWithValue("@createdAt", DateTime.UtcNow);

            await connection.OpenAsync();
            int primaryKey = Convert.ToInt32(await command.ExecuteScalarAsync());

            return primaryKey;
        }
    }

}