//----------------------------------------------
//            liuaf threeKingdoms Project
// Copyright © 2015-2017 threeKingdoms
// Created by : Liu Aifei (329737941@qq.com)
//--------------------------------------------
using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

//public class GameStartState : FSMState {
//
//    /// <summary>
//    /// x 轴移动速率
//    /// </summary>
//    public float sensitivityX = 0.4f;
//    /// <summary>
//    /// Y 轴移动速率
//    /// </summary>
//    public float sensitivityY = 0.2f;
//    /// <summary>
//    /// 摄像机平滑移动速率
//    /// </summary>
//    public float smoothSpeed = 5;
//    /// <summary>
//    /// 摄像机限制移动区域 BoxCollider类型
//    /// </summary>
//    private BoxCollider moveArea;
//    /// <summary>
//    /// 摄像机的Y轴高度
//    /// </summary>
//    private float distance = 43;
//
//    /// <summary>
//    /// 当前所操作的Transform对象
//    /// </summary>
//    private Transform cameraRoot;
//    /// <summary>
//    /// 摄像机所要移动到的距离
//    /// </summary>
//    private Vector3 idealPos;
//    /// <summary>
//    /// 拖拽数据
//    /// </summary>
//    private Gesture dragGesture;
//
//    private bool repeat = false;
//    private Grid selectGrid;
//    private BuildingComponent selectBuilding;
//
//
//    public GameStartState(StateID id, FSMControl control)
//        : base(id, control) {
//        cameraRoot = GameManager.CameraRoot;
//        cameraRoot.ResetPositionY(distance);
//        idealPos = cameraRoot.position;
//    }
//    public override void Enter() {
//        UIManager.Instance.DestroyAll();
//        ResourcesManager.Instance.LoadScene("Start",OnLoadSceneComplete);
//        Debug.Log("Enter Start State");
//    }
//    public override void Exit() {
//        
//    }
//    public override void Update(float fDelta) {
//        Apply();
//        if(Input.GetKeyDown(KeyCode.Escape)){
//            List<ePopupType> list = new List<ePopupType>();
//            list.Add(ePopupType.Cancel);
//            list.Add(ePopupType.Ok);
//            UIManager.OptionPopup("是否退出游戏",list,PopupAction);
//        }
//    }
//    private void Apply(){
//        if(dragGesture != null){
//            if(dragGesture.DeltaMove.SqrMagnitude() > 0){
//                Vector2 screenSpaceMove = new Vector2(sensitivityX * dragGesture.DeltaMove.x,sensitivityY * dragGesture.DeltaMove.y);
//                Vector3 worldSpaceMove = screenSpaceMove.x * cameraRoot.right + screenSpaceMove.y * cameraRoot.forward;
//                idealPos -= worldSpaceMove;
//            }
//        }
//        idealPos.y = Mathf.Lerp(idealPos.y,distance,Time.deltaTime * smoothSpeed);
//        idealPos = ConstrainToMoveArea(idealPos);
//        
//        if(smoothSpeed > 0){
//            cameraRoot.position = Vector3.Lerp(cameraRoot.position, idealPos, Time.deltaTime * smoothSpeed);
//        }else{
//            cameraRoot.position = idealPos;
//        }
//    }
//    /// <summary>
//    /// 加载场景完成
//    /// </summary>
//    private void OnLoadSceneComplete() {
//        UIManager.Instance.Show<MainPanel>();
//        moveArea = GameObject.Find("Scenes").GetComponent<BoxCollider>();
//        MapManager.Instance.InitGrids();
//        MapManager.Instance.InitProjector();
//    }
//
//    private Vector3 ConstrainToMoveArea(Vector3 p){
//        if(moveArea){
//            Vector3 min = moveArea.bounds.min;
//            Vector3 max = moveArea.bounds.max;
//            
//            p.x = Mathf.Clamp(p.x,min.x,max.x);
//            p.y = Mathf.Clamp(p.y,min.y,max.y);
//            p.z = Mathf.Clamp(p.z,min.z,max.z);
//            distance = Mathf.Clamp(distance,min.y,max.y);
//        }
//        return p;
//    }
//    private float DistanceToMoveArea(float dis){
//        if(moveArea){
//            Vector3 min = moveArea.bounds.min;
//            Vector3 max = moveArea.bounds.max;
//            dis = Mathf.Clamp(dis,min.y,max.y);
//        }
//        return dis;
//    }
//    public override void OnFingerDown(Gesture downEvent)
//    {
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
//        RaycastHit hitInfo;
//        if(Physics.Raycast(ray,out hitInfo))
//        {
//            GameObject go = hitInfo.collider.gameObject;
//            if(go.GetComponent<Grid>() != null){
//                selectGrid = go.GetComponent<Grid>();
//            }else if(go.GetComponent<BuildingComponent>() != null){
//                selectBuilding = go.GetComponent<BuildingComponent>();
//            }
//        }
//    }
//    public override void OnFingerUp(Gesture upEvent)
//    {
//        if(selectGrid == null && selectBuilding == null)
//            return;
//
//        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//从摄像机发出到点击坐标的射线
//        RaycastHit hitInfo;
//        if(Physics.Raycast(ray,out hitInfo))
//        {
//            GameObject go = hitInfo.collider.gameObject;
//            if(selectGrid != null && go.GetComponent<Grid>() != null){
//                Grid grid = go.GetComponent<Grid>();
//                if(grid.Index == selectGrid.Index){
//                    MoveTo(grid.transform.position);
//                    Bundle bundle = new Bundle();
//                    List<eMainPanelShowType> list = new List<eMainPanelShowType>();
//                    list.Add(eMainPanelShowType.Details);
//                    if(grid.Type == eBuilding.Tower){
//                        MapManager.Instance.Selected.Show(grid.transform.position);
//                        list.Add(eMainPanelShowType.Tower);
//                    }else{
//                        MapManager.Instance.Selected.Hide();
//                        list.Add(eMainPanelShowType.Build);
//                    }
//                    bundle.SetValue<List<eMainPanelShowType>>("optionList",list);
//                    bundle.SetValue<Action<eMainPanelShowType>>("action",OnClickOption);
//                    UIManager.Instance.SendNotification(MainPanel.MAINPANEL_SHOWOPTIONS,bundle);
//                }
//            }else if(selectBuilding != null && go.GetComponent<BuildingComponent>() != null){
//                BuildingComponent component = go.GetComponent<BuildingComponent>();
//                if(component.Data.ID == selectBuilding.Data.ID){
//                    MoveTo(selectBuilding.Data.Prefab.transform.position);
//                    MapManager.Instance.Selected.Hide();
//                }
//            }
//        }
//    }
//    public override void OnDrag(Gesture gesture)
//    {
//        dragGesture = (gesture.State == GestureRecognitionState.Ended) ? null : gesture;
//        DatabaseManager.MapDatabase.projector.Hide();
//        selectGrid = null;
//        selectBuilding = null;
//        UIManager.Instance.SendNotification(MainPanel.MAINPANEL_HIDEOPTIONS);
//    }
//    public override void OnPinch(PinchGesture gesture)
//    {
//        distance -= gesture.Delta;
//        distance = DistanceToMoveArea(distance);
//    }
//
//    public void MoveTo( Vector3 worldPos )
//    {
//        idealPos = ConstrainToPanningPlane( worldPos );
//    }
//    public Vector3 ConstrainToPanningPlane( Vector3 p )
//    {
//        Vector3 lp = new Vector3();
//        lp.x = p.x;
//        lp.y = distance;
//        float ridio = Mathf.Atan(Mathf.PI * 180f / (GameManager.MainCamera.transform.eulerAngles.x / 2));
//        lp.z = p.z - distance / ridio;
//        return lp;
//    }
//
//    private void OnClickOption(eMainPanelShowType type){
//        if(selectGrid != null){
//            if(type == eMainPanelShowType.Tower){
//                UIManager.Instance.Show<SelectTowerPanel>();
//                UIManager.Instance.SendNotification(SelectTowerPanel.SELECTTOWER_INITDATA,selectGrid.Index);
//            }else if(type == eMainPanelShowType.Build){
//                Building building = new Building();
//                building.ID = selectGrid.Index;
//                if(selectGrid.Index == 1001){
//                    building.Art = "Castle";
//                }else if(selectGrid.Index == 1002){
//                    building.Art = "Barracks";
//                }else if(selectGrid.Index == 1003){
//                    building.Art = "Warehouse";
//                }else if(selectGrid.Index == 1004){
//                    building.Art = "Technology";
//                }else if(selectGrid.Index == 1101 || selectGrid.Index == 1102 ||selectGrid.Index == 1103){
//                    building.Art = "Goldmine";
//                }else if(selectGrid.Index == 1201 || selectGrid.Index == 1202 ||selectGrid.Index == 1203){
//                    building.Art = "FoodFacttory";
//                }
//                MapManager.Instance.AddBuilding(selectGrid.Index,building);
//            }
//        }else if(selectBuilding != null){
//
//        }
//
//    }
//    private void PopupAction(ePopupType type){
//        if(type == ePopupType.Cancel){
//            UIManager.OptionPopup();
//        }else if(type == ePopupType.Ok){
//            QuitGame();
//        }
//    }
//    private void QuitGame(){
//        Application.Quit();
//    }
//
//
//}
