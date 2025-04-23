using UnityEngine;
using UnityEngine.UI;

public class CitizenOnHit : MonoBehaviour
{
    [SerializeField] private CitizenData citizenData;
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Image frontImage;

    Citizen citizen;

    private void Awake()
    {
        citizen = GetComponent<Citizen>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CheckCollisionLayer(other.gameObject, hitLayer))
        {
            if(citizen.IsImpostor())
            {
                if(citizen.IsDetected())
                {
                    HitAlien();
                }
                else
                {
                    citizen.ImpostorDetected();
                }
            }
            else
            {
                HitCitizen();
            }
        }
    }

    private void HitCitizen()
    {
        citizen.ApplayDamage(1);
        frontImage.fillAmount = (float)citizen.Life / (float)citizenData.MaxLife;
        if(citizen.Life <= 0)
        {
            EventManager.Instance.onCitizenRelease.Invoke(citizen);
            GameManager.Instance.CitizenKill();
        }
    }

    private void HitAlien()
    {
        citizen.ApplayDamage(1);
        frontImage.fillAmount = (float)citizen.Life / (float)citizenData.MaxLife;
        if(citizen.Life <= 0)
        {
            EventManager.Instance.onCitizenRelease.Invoke(citizen);
            GameManager.Instance.AlienKill();
        }
    }
}

