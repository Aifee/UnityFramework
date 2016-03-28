using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum eBuilding
{
    /// <summary>
    /// 城堡
    /// </summary>
    Castle = 0,
    /// <summary>
    /// 兵营
    /// </summary>
    Barracks = 1,
    /// <summary>
    /// 仓库
    /// </summary>
    Warehouse = 2,
    /// <summary>
    /// 科技
    /// </summary>
    Technology = 3,
    /// <summary>
    /// 金矿
    /// </summary>
    Goldmine = 4,
    /// <summary>
    /// 食品厂
    /// </summary>
    FoodFacttory = 5,
    /// <summary>
    /// 防御塔
    /// </summary>
    Tower = 6,

}
/// <summary>
/// Building.All base classes, the scene appears in every building containing some of this data
/// </summary>
public class Building : Inventory 
{
    public eBuilding Type;
    public int Level;
    public int CD;
    public Dictionary<eProperty,int> Expends;
    public float Attack;
    public float Defense;
    public float AttackSpped;
    public float Range;
    public bool IsDemolish;
    public bool IsTransposition;
    public Dictionary<eProperty,int> Produce;
    public int OutputOre;
    public int Value;
    public int MaxValue;
    public int OutputSoldiers;
    public int Soldiers;
    public int MaxSoldiers;
    public string Art;
    public GameObject Prefab;
}
