using UnityEngine;
using System.Collections;

public class SelectComponent : MonoBehaviour {

    private Vector3 Position;
    private Vector3 target;
    private Transform tran;
    private bool isPlay = false;
    void Awake(){
        tran = transform;
    }
    void Update(){
        if(isPlay){
            if(target != tran.position){
                Vector3 temp = Vector3.Lerp(tran.position,target,0.3f);
                temp.y = 0.2f;
                tran.position = temp;
            }else{
                isPlay = false;
            }
        }
    }
    public void Hide(){
        gameObject.SetActive(false);
    }
    
    public void Show(Vector3 pos,bool isAni){
        gameObject.SetActive(true);
        if(isAni){
            StartCoroutine(delayMove(MoveTo,0.2f,pos));
        }else{
            pos.y = 0.2f;
            tran.position = pos;
        }

    }
    private IEnumerator delayMove(System.Action<Vector3> action,float delaySeconds,Vector3 pos){
        yield return new WaitForSeconds(delaySeconds);
        isPlay = true;
        action(pos);
    }
    private void ResetPosition(Vector3 pos){
        Position = pos;
        transform.localPosition = Position;
    }
    
    private void MoveTo(Vector3 pos){
        target = pos;
    }
}
