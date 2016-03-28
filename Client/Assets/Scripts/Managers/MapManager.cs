using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapManager : Singleton<MapManager> {
    private static Vector2 OFFSET = new Vector2(10.2f,8.9f);
    public static Vector4 ROW = new Vector4(-6,5,15,14);
    public SelectComponent Selected;
    private Dictionary<int,Grid> grids = new Dictionary<int, Grid>();
    private Dictionary<int,Building> buildings = new Dictionary<int, Building>();
    private Transform mapsRoot;
    public Transform MapsRoot{
        get{
            if(mapsRoot == null)
                mapsRoot = GameObject.Find("Scenes").transform;
            return mapsRoot;
        }
    }
    private BoxCollider moveArea;
    public BoxCollider MoveArea{
        get{
            if(moveArea == null)
                moveArea = MapsRoot.GetComponent<BoxCollider>();
            return moveArea;
        }
    }
    public void InitGrids(){
        int index = 0;
        Grid grid = null;
        for(int i = 0; i < 12; i ++){
            for(int j = 0; j < 14; j ++){
                string gridName = string.Format("TowerGrids/Grid_{0}_{1}",i,j);
                //Debug.Log(gridName);
                grid = MapsRoot.FindChild(gridName).GetComponent<Grid>();
                grid.Index = index;
                grids.Add(index,grid);
                index ++;
            }
        }
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Castle").GetComponent<Grid>();
        grid.Index = 1001;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Barracks").GetComponent<Grid>();
        grid.Index = 1002;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Warehouse").GetComponent<Grid>();
        grid.Index = 1003;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Technology").GetComponent<Grid>();
        grid.Index = 1004;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Goldmine1").GetComponent<Grid>();
        grid.Index = 1101;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Goldmine2").GetComponent<Grid>();
        grid.Index = 1102;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_Goldmine3").GetComponent<Grid>();
        grid.Index = 1103;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_FoodFacttory1").GetComponent<Grid>();
        grid.Index = 1201;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_FoodFacttory2").GetComponent<Grid>();
        grid.Index = 1202;
        grids.Add(grid.Index,grid);
        grid = MapsRoot.FindChild("BuildingGrids/Grid_FoodFacttory3").GetComponent<Grid>();
        grid.Index = 1203;
        grids.Add(grid.Index,grid);
    }

    public void InitProjector(){
        GameObject go = ResourcesManager.Instance.LoadOtherPrefab("Selected");
        GameObject prefab = GameObject.Instantiate(go) as GameObject;
        prefab.name = "Selected";
        prefab.transform.ResetPositionY(0.1f);
        Selected = prefab.GetComponent<SelectComponent>();
        Selected.Hide();
    }

    public void AddBuilding(int gridId,Building data){
        if(!buildings.ContainsKey(gridId)){
            Grid grid = grids[gridId];
            GameObject go = ResourcesManager.Instance.LoadBuilding(data.Art);
            GameObject prefab = GameObject.Instantiate(go);
            prefab.GetComponent<BuildingComponent>().Data = data;
            prefab.transform.ResetParent(grid.transform);
            data.Prefab = prefab;
            buildings.Add(gridId,data);
        }else{
            Debug.Log("this grid has building:" + data.ID);
        }
    }
}
