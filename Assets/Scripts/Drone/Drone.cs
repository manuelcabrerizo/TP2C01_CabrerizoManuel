using System;
using UnityEngine;

public class Drone : MonoBehaviour, IHelable
{
    public static event Action<float> onLifeChange;
    public static event Action onPlayerKill;

    [SerializeField] private PlayerData playerData;
    private float life;

    private Vector3 lastFrameVelocity;

    private Rigidbody body;

    private void Awake()
    {
        Health.onHealPickup += Heal;

        body = GetComponent<Rigidbody>();
        life = playerData.MaxLife;

        lastFrameVelocity = body.velocity;
    }

    private void OnDestroy()
    {
        Health.onHealPickup -= Heal;
    }

    private void Update()
    {
        lastFrameVelocity = body.velocity;
    }

    public void ResetDrone()
    {
        life = playerData.MaxLife;
        onLifeChange?.Invoke(life / playerData.MaxLife);
        transform.position = playerData.SpawPosition;
        body.velocity = Vector3.zero;
    }

    public void TakeDamage()
    {
        float damage = 10;
        life -= damage;   
        if (life <= 0)
        {
            onPlayerKill?.Invoke();
        }
        onLifeChange?.Invoke(life / playerData.MaxLife);
    }

    public void TakeDamageBaseOnVelocity()
    {
        float magnitude = lastFrameVelocity.magnitude / playerData.MaxSpeed;
        float damage = playerData.MaxDamageRecived * magnitude;
        life -= damage;   
        if (life <= 0)
        {
            onPlayerKill?.Invoke();
        }
        onLifeChange?.Invoke(life / playerData.MaxLife);
    }

    public void Heal(int healAmount)
    {
        life = Mathf.Min(life + healAmount, playerData.MaxLife);
        onLifeChange?.Invoke(life / playerData.MaxLife);
    }
}