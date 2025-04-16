using UnityEngine;

public class RunningState : MonoBehaviour, IState
{
    private Citizen citizen;

    private Vector3 target;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }

    public void OnEnter()
    {
        citizen.Animator.SetInteger("CitizenState", 2);
        citizen.TargetNode = citizen.CurrentNode.GetRandomChild();
        Vector2 offset = Random.insideUnitCircle * 3.0f;
        target = citizen.TargetNode.transform.position + new Vector3(offset.x, 0.0f, offset.y);
    }

    public void OnExit()
    {
        citizen.CurrentNode = citizen.TargetNode;
    }

    public void OnUpdate(float dt)
    {
    }

    public void OnFixedUpdate(float dt)
    {
        Vector3 toTarget = target - transform.position;
        float distToTarget = toTarget.magnitude;
        if(distToTarget > citizen.ReachRadio)
        {
            toTarget.y = 0;
            citizen.Body.AddForce(toTarget.normalized * 30.0f, ForceMode.Acceleration);
            citizen.transform.rotation = Quaternion.LookRotation(toTarget.normalized);
        }
        else
        {
            citizen.SetRandomState();
        }
    }
}
