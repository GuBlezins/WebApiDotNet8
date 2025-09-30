using Application.Dtos;
using Domain.Entities;

namespace Application.Interfaces;

public interface IItemService
{
    Task<bool> CreateItem(ItemDto entity);
    Task<Item?> GetItemById(Guid id);
    Task<List<Item>> GetAllItems();
    Task<bool> Delete(Guid id);
    Task<bool> Update(ItemDto entity);
}
