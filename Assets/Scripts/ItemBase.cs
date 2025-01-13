public class ItemBase
{
    public string Name { get; set; }

    // we will either buy or sell stuff in the current structure
    // it could be buy/sell values but as things currently stand, not necessary imo.
    public int Value { get; set; }

    public ItemBase(string name, int value)
    {
        Name = name;
        Value = value;
    }

    public string ItemBaseToString()
    {
        return $"item name: {Name}, value: {Value}";
    }
}