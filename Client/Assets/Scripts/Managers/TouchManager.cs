using UnityEngine;
using System.Collections;

public class TouchManager : MonoBehaviour{

    void OnEnable(){
        EasyTouch.On_TouchStart += HandleOn_TouchStart;
        EasyTouch.On_SimpleTap += HandleOn_SimpleTap;
        EasyTouch.On_LongTap += HandleOn_LongTap;
        EasyTouch.On_Pinch += HandleOn_Pinch;
        EasyTouch.On_DragStart += HandleOn_DragStart;
        EasyTouch.On_Drag += HandleOn_Drag;
        EasyTouch.On_DragEnd += HandleOn_DragEnd;
    }

    void Update(){

    }
    void OnDisable(){
        UnsubscribeEvent();
    }
    void OnDestroy(){
        UnsubscribeEvent();
    }
    void UnsubscribeEvent(){
        EasyTouch.On_TouchStart -= HandleOn_TouchStart;
        EasyTouch.On_SimpleTap -= HandleOn_SimpleTap;
        EasyTouch.On_LongTap -= HandleOn_LongTap;
        EasyTouch.On_Pinch -= HandleOn_Pinch;
        EasyTouch.On_DragStart -= HandleOn_DragStart;
        EasyTouch.On_Drag -= HandleOn_Drag;
        EasyTouch.On_DragEnd -= HandleOn_DragEnd;
    }

    void HandleOn_TouchStart (Gesture gesture)
    {
        
    }
    void HandleOn_SimpleTap (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnClick(gesture);
    }
    void HandleOn_LongTap (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnLongTap(gesture);
    }
    void HandleOn_Pinch (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnPinch(gesture);
    }
    void HandleOn_DragStart (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnDragStart(gesture);
    }
    void HandleOn_Drag (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnDrag(gesture);
    }
    void HandleOn_DragEnd (Gesture gesture)
    {
        GameStatesManager.Instance.CurrentState.OnDragEnd(gesture);
    }


//    void OnFingerDown(FingerDownEvent fingerDown){
//        GameStatesManager.Instance.CurrentState.OnFingerDown(fingerDown);
//    }
//    void OnFingerUp(FingerUpEvent fingerUp){
//        GameStatesManager.Instance.CurrentState.OnFingerUp(fingerUp);
//    }
//
//    void OnDrag(DragGesture gesture){
//        GameStatesManager.Instance.CurrentState.OnDrag(gesture);
//    }
//    void OnPinch(PinchGesture gesture){
//        GameStatesManager.Instance.CurrentState.OnPinch(gesture);
//    }

}
