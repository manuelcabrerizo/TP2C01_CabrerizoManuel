using UnityEngine;
using UnityEngine.UI;

public class DroneState : MonoBehaviour
{
    // TODO: use playerData
    [SerializeField] private PlayerData playerData;
    private float life;

    private Vector3 lastFrameVelocity;

    private DroneMovement movement;

    private void Start()
    {
        movement = GetComponent<DroneMovement>();
        life = playerData.MaxLife;

        lastFrameVelocity = movement.Body.velocity;
    }

    private void Update()
    {
        lastFrameVelocity = movement.Body.velocity;
    }

    public void TakeDamageBaseOnVelocity()
    {
        float magnitude = lastFrameVelocity.magnitude / playerData.MaxSpeed;
        float damage = playerData.MaxDamageRecived * magnitude;
        life -= damage;   
        if (life <= 0)
        {
            life = playerData.MaxLife;
            transform.position = playerData.SpawPosition;
            movement.Body.velocity = Vector3.zero;
        }
        EventManager.Instance.onTakeDamage.Invoke(life / playerData.MaxLife);
    }
}