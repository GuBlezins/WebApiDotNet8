using Domain.Enum;

namespace Domain.Entities;

public class Item : Entity
{
    public Item(string name, string description, EStatus status, EPriority priority)
    {
        Name = name;
        Description = description;
        Status = status;
        Priority = priority;
    }

    public string Name { get; set; }
    public string Description { get; set; }
    public EStatus Status { get; set; }
    public EPriority Priority { get; set; }
}
