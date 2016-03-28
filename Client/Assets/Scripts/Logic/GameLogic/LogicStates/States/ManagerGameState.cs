using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// @Summary : 游戏经营界面状态
/// </summary>
public class ManagerGameState : GameState
{
    /// <summary>
    /// x 轴移动速率
    /// </summary>
    public float sensitivityX = 0.4f;
    /// <summary>
    /// Y 轴移动速率
    /// </summary>
    public float sensitivityY = 0.2f;
    /// <summary>
    /// 摄像机平滑移动速率
    /// </summary>
    public float smoothSpeed = 5;
    /// <summary>
    /// 摄像机限制移动区域 BoxCollider类型
    /// </summary>
    private BoxCollider moveArea;
    /// <summary>
    /// 摄像机的Y轴高度
    /// </summary>
    private float distance = 43;
    
    /// <summary>
    /// 当前所操作的Transform对象
    /// </summary>
    private Transform cameraRoot;
    /// <summary>
    /// 摄像机所要移动到的距离
    /// </summary>
    private Vector3 idealPos;
    /// <summary>
    /// 拖拽数据
    /// </summary>
    private Gesture dragGesture;
    
    private bool repeat = false;
    private Grid selectGrid;
    private Grid longTapGrid;
    private BuildingComponent selectBuilding;
    private LongTapDragLogic longLogic;

    public ManagerGameState()
    {
        cameraRoot = GameManager.CameraRoot;
        cameraRoot.ResetPositionY(distance);
        idealPos = cameraRoot.position;
        longLogic = new LongTapDragLogic();
    }
    /// <summary>
    /// 状态开始
    /// </summary>
    public override void Start()
    {

    }
    public override void LoadScene()
    {
        ResourcesManager.Instance.LoadScene("Manager",LoadComplete);
    }
    public override void Update(float dt)
    {
        if (Input.GetAxis("Mouse ScrollWheel") != 0) 
        {
            Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
            distance -= smoothSpeed * Input.GetAxis("Mouse ScrollWheel");// gesture.Delta;
            distance = DistanceToMoveArea(distance);
        }
        Apply();
        if(Input.GetKeyDown(KeyCode.Escape)){
            List<ePopupType> list = new List<ePopupType>();
            list.Add(ePopupType.Cancel);
            list.Add(ePopupType.Ok);
            UIManager.OptionPopup("是否退出游戏",list,PopupAction);
        }
    }
    public override void Stop()
    {
        
    }
    public override void OnClick(Gesture gesture)
    {
        GameObject go = gesture.pickedObject;
        if(go == null)
            return;
        if(go.GetComponent<Grid>() != null){
            Grid grid = go.GetComponent<Grid>();
            selectGrid = grid;
            if(grid.Index == selectGrid.Index){
                MoveTo(grid.transform.position);
                Bundle bundle = new Bundle();
                List<eMainPanelShowType> list = new List<eMainPanelShowType>();
                list.Add(eMainPanelShowType.Details);
                if(grid.Type == eBuilding.Tower){
                    MapManager.Instance.Selected.Show(grid.transform.position,true);
                    list.Add(eMainPanelShowType.Tower);
                }else{
                    MapManager.Instance.Selected.Hide();
                    list.Add(eMainPanelShowType.Build);
                }
                bundle.SetValue<List<eMainPanelShowType>>("optionList",list);
                bundle.SetValue<Action<eMainPanelShowType>>("action",OnClickOption);
                UIManager.Instance.SendNotification(MainPanel.MAINPANEL_SHOWOPTIONS,bundle);
            }
        }else if(go.GetComponent<BuildingComponent>() != null){
            BuildingComponent component = go.GetComponent<BuildingComponent>();
            selectBuilding = component;
            if(component.Data.ID == selectBuilding.Data.ID){
                MoveTo(selectBuilding.Data.Prefab.transform.position);
                MapManager.Instance.Selected.Hide();
            }
        }
        longTapGrid = null;
        longLogic.Clear();
    }
    public override void OnLongTap(Gesture gesture)
    {
        if(longTapGrid == null && gesture.pickedObject.GetComponent<Grid>() != null){
            longTapGrid = gesture.pickedObject.GetComponent<Grid>();
            Debug.Log(longTapGrid.Index + "  " + longTapGrid.name);
            longLogic.MoveCheck(longTapGrid);
        }
    }
    public override void OnDragStart(Gesture gesture)
    {
        DatabaseManager.MapDatabase.projector.Hide();
        selectGrid = null;
        selectBuilding = null;
        UIManager.Instance.SendNotification(MainPanel.MAINPANEL_HIDEOPTIONS);
    }
    public override void OnDrag(Gesture gesture)
    {
        if(longTapGrid != null){
            Ray ray = GameManager.MainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if(Physics.Raycast(ray,out hit)){
                GameObject go = hit.collider.gameObject;
                if(go.GetComponent<Grid>() != null){
                    Grid grid = go.GetComponent<Grid>();
                    if(grid != longTapGrid){
                        longLogic.MoveCheck(grid);
                    }
                }
            }
        }else{
            dragGesture = gesture;
        }
    }
    public override void OnDragEnd(Gesture gesture)
    {
        if(dragGesture != null){
            dragGesture.swipeVector = Vector2.zero;
        }
    }
    public override void OnPinch(Gesture gesture)
    {
        base.OnPinch(gesture);
    }
    private void LoadComplete(){
        isLoaded = true;
        UIManager.Instance.Show<MainPanel>();
        moveArea = GameObject.Find("Scenes").GetComponent<BoxCollider>();
        MapManager.Instance.InitGrids();
        MapManager.Instance.InitProjector();
    }
    private void Apply(){
        if(dragGesture != null){
            if(dragGesture.swipeVector.SqrMagnitude() > 0){
                Vector2 screenSpaceMove = new Vector2(sensitivityX * dragGesture.swipeVector.x,sensitivityY * dragGesture.swipeVector.y);
                Vector3 worldSpaceMove = screenSpaceMove.x * cameraRoot.right + screenSpaceMove.y * cameraRoot.forward;
                idealPos -= worldSpaceMove;
            }
        }
        idealPos.y = Mathf.Lerp(idealPos.y,distance,Time.deltaTime * smoothSpeed);
        idealPos = ConstrainToMoveArea(idealPos);
        
        if(smoothSpeed > 0){
            cameraRoot.position = Vector3.Lerp(cameraRoot.position, idealPos, Time.deltaTime * smoothSpeed);
        }else{
            cameraRoot.position = idealPos;
        }
    }
    private Vector3 ConstrainToMoveArea(Vector3 p){
        if(moveArea){
            Vector3 min = moveArea.bounds.min;
            Vector3 max = moveArea.bounds.max;
            
            p.x = Mathf.Clamp(p.x,min.x,max.x);
            p.y = Mathf.Clamp(p.y,min.y,max.y);
            p.z = Mathf.Clamp(p.z,min.z,max.z);
            distance = Mathf.Clamp(distance,min.y,max.y);
        }
        return p;
    }
    private float DistanceToMoveArea(float dis){
        if(moveArea){
            Vector3 min = moveArea.bounds.min;
            Vector3 max = moveArea.bounds.max;
            dis = Mathf.Clamp(dis,min.y,max.y);
        }
        return dis;
    }

    
    public void MoveTo( Vector3 worldPos )
    {
        idealPos = ConstrainToPanningPlane( worldPos );
    }
    public Vector3 ConstrainToPanningPlane( Vector3 p )
    {
        Vector3 lp = new Vector3();
        lp.x = p.x;
        lp.y = distance;
        float ridio = Mathf.Atan(Mathf.PI * 180f / (GameManager.MainCamera.transform.eulerAngles.x / 2));
        lp.z = p.z - distance / ridio;
        return lp;
    }
    
    private void OnClickOption(eMainPanelShowType type){
        if(selectGrid != null){
            if(type == eMainPanelShowType.Tower){
                UIManager.Instance.Show<SelectTowerPanel>();
                UIManager.Instance.SendNotification(SelectTowerPanel.SELECTTOWER_INITDATA,selectGrid.Index);
            }else if(type == eMainPanelShowType.Build){
                Building building = new Building();
                building.ID = selectGrid.Index;
                if(selectGrid.Index == 1001){
                    building.Art = "Castle";
                }else if(selectGrid.Index == 1002){
                    building.Art = "Barracks";
                }else if(selectGrid.Index == 1003){
                    building.Art = "Warehouse";
                }else if(selectGrid.Index == 1004){
                    building.Art = "Technology";
                }else if(selectGrid.Index == 1101 || selectGrid.Index == 1102 ||selectGrid.Index == 1103){
                    building.Art = "Goldmine";
                }else if(selectGrid.Index == 1201 || selectGrid.Index == 1202 ||selectGrid.Index == 1203){
                    building.Art = "FoodFacttory";
                }
                MapManager.Instance.AddBuilding(selectGrid.Index,building);
            }
        }else if(selectBuilding != null){
            
        }
        
    }
    private void PopupAction(ePopupType type){
        if(type == ePopupType.Cancel){
            UIManager.OptionPopup();
        }else if(type == ePopupType.Ok){
            QuitGame();
        }
    }
    private void QuitGame(){
        Application.Quit();
    }
    
    
}
