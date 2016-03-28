using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BuildingDatabase : Database<Building> {

	public bool HasType(eBuilding type){
        if(_items.Count <= 0) return false;
        foreach(Building item in _items.Values){
            if(type == item.Type){
                return true;
            }
        }
        return false;
    }

    public IList<Building> GetTypeBuildings(eBuilding type){
        List<Building> list = new List<Building>();
        foreach(Building item in _items.Values){
            if(type == item.Type){
                list.Add(item);
            }
        }
        return list;
    }
}
