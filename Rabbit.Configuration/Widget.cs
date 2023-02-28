namespace Rabbit.Configuration;

public class Widget
{
    public Widget(string name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public Guid Id { get; init; }

    public string Name { get; init; } = null!;
}