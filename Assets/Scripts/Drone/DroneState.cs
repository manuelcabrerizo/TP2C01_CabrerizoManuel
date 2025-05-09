using UnityEngine;

public class DroneState : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    private float life;

    private Vector3 lastFrameVelocity;

    private Rigidbody body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        life = playerData.MaxLife;

        lastFrameVelocity = body.velocity;
    }

    private void Update()
    {
        lastFrameVelocity = body.velocity;
    }

    public void ResetDrone()
    {
        life = playerData.MaxLife;
        EventManager.Instance.onTakeDamage.Invoke(life / playerData.MaxLife);
        transform.position = playerData.SpawPosition;
        body.velocity = Vector3.zero;
    }

    public void TakeDamage()
    {
        float damage = 10;
        life -= damage;   
        if (life <= 0)
        {
            GameManager.Instance.PlayerKill();
        }
        EventManager.Instance.onTakeDamage.Invoke(life / playerData.MaxLife);
    }

    public void TakeDamageBaseOnVelocity()
    {
        float magnitude = lastFrameVelocity.magnitude / playerData.MaxSpeed;
        float damage = playerData.MaxDamageRecived * magnitude;
        life -= damage;   
        if (life <= 0)
        {
            GameManager.Instance.PlayerKill();
        }
        EventManager.Instance.onTakeDamage.Invoke(life / playerData.MaxLife);
    }
}