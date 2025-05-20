using System;
using UnityEngine;

public class CitizenOnHit : MonoBehaviour
{
    public static event Action<Vector3> onAlienKill;
    public static event Action onCitizenKill;

    [SerializeField] private CitizenData citizenData;
    [SerializeField] private LayerMask hitLayer;

    Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
        DroneShoot.onCitizenDamage += OnCitizenDamage;
    }

    private void OnDestroy()
    {
        DroneShoot.onCitizenDamage -= OnCitizenDamage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CheckCollisionLayer(other.gameObject, hitLayer))
        {
            OnCitizenDamage(this.gameObject);
        }
    }

    private void OnCitizenDamage(GameObject gameObject)
    {
        if(gameObject != this.gameObject) return;

        if(citizen.IsImpostor() && !citizen.IsDetected())
        {
            citizen.ImpostorDetected();
        }
        else
        {
            HitCitizen();
        }
    }

    private void HitCitizen()
    {
        citizen.TakeDamage(1, citizenData.MaxLife);
        if(citizen.Life <= 0)
        {
            if(citizen.IsImpostor())
            {
                onAlienKill?.Invoke(transform.position);
            }
            else
            {
                onCitizenKill?.Invoke();
            }
            citizen.SendReleaseEvent();
        }
    }
}

