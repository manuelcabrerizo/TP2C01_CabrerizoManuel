using UnityEngine;

public class ImpostorState : MonoBehaviour, IState
{
    private Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }

    public void OnEnter()
    {
        citizen.ConvertToAlien();
    }

    public void OnExit()
    {
    }

    public void OnUpdate(float dt)
    {
        // TODO: attack the player ...
    }

    public void OnFixedUpdate(float dt)
    {
    }
}
