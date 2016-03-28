using UnityEngine;
using System.Collections;

public enum eEquip{
    Helmet,
    Breastplate,
    Armguard,
    Gaiter,
    Boots,
}
public class Equip : Inventory {
    public eEquip Type;
    public int Level;
	
}
