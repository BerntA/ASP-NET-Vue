namespace database.Entities;

public class Contact : EntityBase<Guid>
{
    public string Name { get; set; }

    public string Phone { get; set; }

    public string Address { get; set; }
}