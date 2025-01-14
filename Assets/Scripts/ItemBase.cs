using System;

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

    public override bool Equals(object obj)
    {
        if (obj is ItemBase other)
        {
            return Name == other.Name && Value == other.Value;
        }
        return false;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Value);
    }
}