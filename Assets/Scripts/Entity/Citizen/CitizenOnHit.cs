using UnityEngine;

public class CitizenOnHit : MonoBehaviour
{
    [SerializeField] private CitizenData citizenData;
    [SerializeField] private LayerMask hitLayer;

    Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CheckCollisionLayer(other.gameObject, hitLayer))
        {
            if(citizen.IsImpostor() && !citizen.IsDetected())
            {
                citizen.ImpostorDetected();
            }
            else
            {
                HitCitizen();
            }
        }
    }

    private void HitCitizen()
    {
        citizen.TakeDamage(1, citizenData.MaxLife);
        if(citizen.Life <= 0)
        {
            if(citizen.IsImpostor())
            {
                GameManager.Instance.AlienKill();
            }
            else
            {
                GameManager.Instance.CitizenKill();
            }
            EventManager.Instance.onEntityRelease.Invoke(citizen);
        }
    }
}

