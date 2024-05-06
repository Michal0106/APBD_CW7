using Microsoft.AspNetCore.Mvc;
using ProductWarehouseApp.Dto;
using ProductWarehouseApp.Repositories;

namespace ProductWarehouseApp.Controllers;


[Route("api/warehouses2")]
[ApiController]
public class Warehouses2Controller : ControllerBase
{
    private IWarehouses2Repository _warehouses2Repository;
    public Warehouses2Controller(IWarehouses2Repository warehouses2Repository)
    {
        _warehouses2Repository = warehouses2Repository;
    }

    [HttpPost]
    public async Task<int> AddProductToWarehouses(ProductWarehouseDto productWarehouseDto)
    {
        return await _warehouses2Repository.InsertProductWarehouse(productWarehouseDto);
    }
}