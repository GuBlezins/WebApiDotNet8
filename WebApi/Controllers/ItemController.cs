using Application.Dtos;
using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemService _itemService;

    public ItemsController(IItemService itemService)
    {
        _itemService = itemService;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Create(ItemDto dto)
    {
        var result = await _itemService.CreateItem(dto);
        return result ? Ok("Item criado com sucesso") : BadRequest("Erro ao criar item");
    }

    [Authorize]
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var item = await _itemService.GetItemById(id);
        return Ok(item);
    }

    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var items = await _itemService.GetAllItems();
        return Ok(items);
    }

    [Authorize]
    [HttpPut]
    public async Task<IActionResult> Update(ItemDto dto)
    {
        var result = await _itemService.Update(dto);
        return result ? Ok("Item atualizado com sucesso") : BadRequest("Erro ao atualizar item");
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var result = await _itemService.Delete(id);
        return result ? Ok("Item deletado com sucesso") : BadRequest("Erro ao deletar item");
    }
}
