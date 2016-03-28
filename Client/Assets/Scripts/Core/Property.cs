
/// <summary>
/// E property.Attribute enumeration
/// </summary>
public enum eProperty{
    /// <summary>
    /// The gold.金币
    /// </summary>
    Gold = 1,
    /// <summary>
    /// The wood.木材
    /// </summary>
    Wood = 2,
    /// <summary>
    /// The stone.石材
    /// </summary>
    Stone = 3,
    /// <summary>
    /// The iron ore.铁矿
    /// </summary>
    IronOre = 4,
    /// <summary>
    /// The currency.现实中货币，可充值的
    /// </summary>
    Currency = 5,
}
public class Property : Inventory
{
    public eProperty Type;
    public int Value;
}
