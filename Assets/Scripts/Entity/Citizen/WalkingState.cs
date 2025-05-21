using UnityEngine;

public class WalkingState : MonoBehaviour, IState
{
    [SerializeField] private CitizenData citizenData;
    private Citizen citizen;
    private Vector3 target;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }

    public void OnEnter()
    {
        citizen.Animator.SetInteger("CitizenState", 1);
        citizen.TargetNode = citizen.CurrentNode.GetRandomChild();
        Vector2 offset = Random.insideUnitCircle * 3.0f;
        target = citizen.TargetNode.transform.position + new Vector3(offset.x, 0.0f, offset.y);
    }

    public void OnExit()
    {
        citizen.CurrentNode = citizen.TargetNode;
    }

    public void OnUpdate()
    {
    }

    public void OnFixedUpdate()
    {
        Vector3 toTarget = target - transform.position;
        float distToTarget = toTarget.magnitude;
        if(distToTarget > citizenData.ReachRadio)
        {
            toTarget.y = 0;
            citizen.Body.AddForce(toTarget.normalized * citizenData.walkSpeed, ForceMode.Acceleration);
            citizen.transform.rotation = Quaternion.LookRotation(toTarget.normalized);
        }
        else
        {
            citizen.SetRandomState();
        }
    }
}
