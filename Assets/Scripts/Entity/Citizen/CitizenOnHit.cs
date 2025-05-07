using UnityEngine;

public class CitizenOnHit : MonoBehaviour
{
    [SerializeField] private CitizenData citizenData;
    [SerializeField] private LayerMask hitLayer;

    Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
        EventManager.Instance.onCitizenDamage.AddListener(OnCitizenDamage);
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

