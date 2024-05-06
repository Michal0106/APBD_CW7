using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ProductWarehouseApp.Dto;
using ProductWarehouseApp.Repositories;

namespace ProductWarehouseApp.Controllers;

[Route("api/warehouses")]
[ApiController]
public class WarehousesController : ControllerBase
{
    private readonly IWarehousesRepository _warehousesRepository;


    public WarehousesController(IWarehousesRepository warehousesRepository)
    {
        _warehousesRepository = warehousesRepository;
    }

    [HttpPost]
    public async Task<IActionResult> AddProductWarehouse(ProductWarehouseDto productWarehouse)
    {
        if (await _warehousesRepository.ProductNotExist(productWarehouse.IdProduct) 
            || await _warehousesRepository.WarehouseNotExist(productWarehouse.IdWarehouse))
        {
            return NotFound("Product or Warehouse for given id does not exist");
        }

        int idOrder = await _warehousesRepository.OrderNotExist(productWarehouse);
        if (idOrder == -1)
        {
            return NotFound("There is no proper order in database");
        }

        await _warehousesRepository.UpdateFulfilledAt(idOrder);

        var primaryKey = await _warehousesRepository.InsertProductWarehouse(productWarehouse, idOrder);
            
        return Ok(primaryKey);
    }
}
