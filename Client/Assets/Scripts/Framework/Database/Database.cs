using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Database<T> : BaseObject where T : Inventory,new ()
{
    protected Dictionary<int,T> _items = new Dictionary<int, T>();
    public Dictionary<int,T> GetItems(){
        return _items;
    }
    public bool HasItem(int id){
        return _items.ContainsKey(id);
    }
    public bool AddItem(T t){
        if(!HasItem(t.ID)){
            _items.Add(t.ID,t);
            return true;
        }
        return false;
    }
    public bool RemoveItem(T t){
        if(HasItem(t.ID)){
            _items.Remove(t.ID);
            return true;
        }
        return false;
    }

    public T GetItem(int id){
        if(HasItem(id)){
            return _items[id];
        }
        return null;
    }





}
