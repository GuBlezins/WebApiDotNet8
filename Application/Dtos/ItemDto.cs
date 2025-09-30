using Domain.Enum;

namespace Application.Dtos;

public class ItemDto
{

    public Guid? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public EStatus? Status { get; set; } = EStatus.init;
    public EPriority? Priority { get; set; } = EPriority.noPriority;
}
