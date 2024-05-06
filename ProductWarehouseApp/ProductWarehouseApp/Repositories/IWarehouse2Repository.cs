using ProductWarehouseApp.Dto;

namespace ProductWarehouseApp.Repositories;

public interface IWarehouses2Repository
{
    Task<int> InsertProductWarehouse(ProductWarehouseDto productWarehouseDto);
}
