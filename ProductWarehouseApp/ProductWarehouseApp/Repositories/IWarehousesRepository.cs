using ProductWarehouseApp.Dto;

namespace ProductWarehouseApp.Repositories;

public interface IWarehousesRepository
   {
       Task<bool> ProductNotExist(int id);
       Task<bool> WarehouseNotExist(int id);
       Task<int> OrderNotExist(ProductWarehouseDto warehouse);
       Task UpdateFulfilledAt(int idOrder);
       Task<int> InsertProductWarehouse(ProductWarehouseDto productWarehouse, int idOrder);
   }