using UnityEngine;

public class IdleState : MonoBehaviour, IState
{
    float timer = 10.0f;
    float timeToWait = 10.0f;

    private Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }

    public void OnEnter()
    {
        citizen.Animator.SetInteger("CitizenState", 0);
        timer = timeToWait;
    }

    public void OnExit()
    {

    }

    public void OnUpdate(float dt)
    {
        if(timer > 0.0f)
        {
            timer -= dt;
        }
        else
        {
            citizen.SetRandomState();
        }
    }

    public void OnFixedUpdate(float dt)
    {
    }
}
