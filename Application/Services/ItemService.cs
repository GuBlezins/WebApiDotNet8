namespace Application.Services;

using Application.Dtos;
using Application.Interfaces;
using Domain.Entities;
using Domain.Enum;
using Domain.Repositories;

public class ItemService : IItemService
{
    private readonly IRepository<Item> _repository;

    public ItemService(IRepository<Item> repository)
    {
        _repository = repository;
    }

    public async Task<bool> CreateItem(ItemDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
            throw new ArgumentException("O nome do item é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Description))
            throw new ArgumentException("A descrição do item é obrigatória.");

        var item = new Item(dto.Name, dto.Description, EStatus.init, dto.Priority ?? EPriority.noPriority);

        return await _repository.Create(item);
    }

    public async Task<Item?> GetItemById(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.");

        var item = await _repository.GetById(id);

        return item;
    }

    public async Task<List<Item>> GetAllItems()
    {
        var items = await _repository.GetAll();
        return items.ToList();
    }

    public async Task<bool> Delete(Guid id)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id inválido.");

        var item = await _repository.GetById(id);
        if (item == null)
            throw new KeyNotFoundException("Item não encontrado.");

        return await _repository.Delete(id);
    }

    public async Task<bool> Update(ItemDto dto)
    {
        if (dto.Id == null || dto.Id == Guid.Empty)
            throw new ArgumentException("Id inválido para atualização.");

        var existingItem = await _repository.GetById(dto.Id.Value);
        if (existingItem == null)
            throw new KeyNotFoundException("Item não encontrado.");

        existingItem.Name = dto.Name ?? existingItem.Name;
        existingItem.Description = dto.Description ?? existingItem.Description;
        if(dto.Status.HasValue) existingItem.Status = dto.Status.Value;
        if(dto.Priority.HasValue) existingItem.Priority = dto.Priority.Value;

        return await _repository.Update(existingItem, existingItem.Id);
    }
}
