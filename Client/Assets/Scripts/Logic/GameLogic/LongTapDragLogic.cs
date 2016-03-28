using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LongTapDragLogic {

    private Dictionary<Grid,GameObject> routes = new Dictionary<Grid, GameObject>();

    public void Clear(){
        if(routes.Count > 0){
            foreach(KeyValuePair<Grid,GameObject> kvp in routes){
                GameObject.Destroy(kvp.Value);
            }
            routes.Clear();
        }
    }
    public void MoveCheck(Grid grid){
        if(routes.ContainsKey(grid))
            return;
        CreateRouteCode(grid);
    }
    public void CreateRouteCode(Grid grid){
        GameObject go = ResourcesManager.Instance.LoadOtherPrefab("Selected");
        GameObject prefab = GameObject.Instantiate(go);
        prefab.transform.ResetParent(grid.transform);
        Debug.Log(grid.Index + "  " + grid.name);
        routes.Add(grid,prefab);
    }
	
}
