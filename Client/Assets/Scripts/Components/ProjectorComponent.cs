using UnityEngine;
using System.Collections;

public enum IProjector{
    Green,
    Red,
}

public class ProjectorComponent : MonoBehaviour  {

    private IProjector Type;
    private Vector3 Position;
    private Projector projector;
    private Material greenMaterial;
    private Material redMeterial;
    private Vector3 target;
    private Transform tran;

    void Awake(){
        tran = transform;
        projector = gameObject.GetComponent<Projector>();
        greenMaterial = ResourcesManager.Instance.LoadMaterial("GreenProjector");
        redMeterial = ResourcesManager.Instance.LoadMaterial("RedProjector");
    }
    void Update(){
        if(target != tran.position){
            Vector3 temp = Vector3.Lerp(tran.position,target,0.3f);
            temp.y = 35;
            tran.position = temp;
        }
    }
    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Show(IProjector type,Vector3 pos){
        gameObject.SetActive(true);
        ResetType(type);
        StartCoroutine(delayMove(MoveTo,0.2f,pos));
        //MoveTo(pos);
    }
    private IEnumerator delayMove(System.Action<Vector3> action,float delaySeconds,Vector3 pos){
        yield return new WaitForSeconds(delaySeconds);
        action(pos);
    }
    private void ResetPosition(Vector3 pos){
        Position = pos;
        transform.localPosition = Position;
    }
    private void ResetType(IProjector type){
        Type = type;
        if(type == IProjector.Green){
            projector.material = greenMaterial;
        }else{
            projector.material = redMeterial;
        }
    }

    private void MoveTo(Vector3 pos){
        target = pos;
    }

}
