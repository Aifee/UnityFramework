using UnityEngine;
using System.Collections;

public class MapDatabase {
    private static Vector2 OFFSET = new Vector2(10.2f,8.9f);
    public static Vector4 ROW = new Vector4(-6,5,15,14);
    public ProjectorComponent projector;
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

    public MapDatabase(){
        //InitGrids();
        InitProjector();
    }

//    public Grid GetGrid(GameObject go){
//        if(_items.Count > 0){
//            foreach(Grid grid in _items.Values){
//                if(grid.Prefab == go){
//                    return grid;
//                }
//            }
//        }
//        return null;
//    }


//    public void InitGrids(){
//        if(MapsRoot == null)
//            return;
//        GameObject gridGo = ResourcesManager.Instance.LoadBuilding("Grid");
//
//        int index = 1;
//        for(int i = 0; i < ROW.z; i ++){
//            for(int j = 0; j < ROW.w; j ++){
//                Vector3 pos = new Vector3();
//                if(j % 2 == 0){
//                    pos.x = (i + ROW.x) * OFFSET.x - OFFSET.x / 2;
//                }else{
//                    pos.x = (i + ROW.x) * OFFSET.x;
//                }
//                
//                pos.z = (ROW.y - j) * OFFSET.y;
//                Grid grid = new Grid();
//                grid.Position = pos;
//                grid.ID = index;
//                GameObject gridPrefab = GameObject.Instantiate(gridGo) as GameObject;
//                gridPrefab.name = string.Format("Grid_{0}_{1}",i,j);
//                grid.Name = gridPrefab.name;
//                gridPrefab.transform.ResetParent(MapsRoot);
//                gridPrefab.transform.ResetPosition(pos);
//                GridComponent component = gridPrefab.GetComponent<GridComponent>();
//                component.Data = grid;
//                AddItem(grid);
//                index ++;
//            }
//        }
//    }

    public void InitProjector(){
        GameObject projectorGo = ResourcesManager.Instance.LoadOtherPrefab("Projector");
        GameObject proPrefab = GameObject.Instantiate(projectorGo) as GameObject;
        proPrefab.name = "Projector";
        proPrefab.transform.ResetEulerAngleX(90);
        proPrefab.transform.ResetPositionY(35);
        projector = proPrefab.GetComponent<ProjectorComponent>();
        projector.Hide();
    }
    
}
