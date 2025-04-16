using UnityEngine;
using UnityEngine.UI;

public class CitizenOnHit : MonoBehaviour
{
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
                    citizen.Life--;
                    frontImage.fillAmount = (float)citizen.Life / (float)citizen.MaxLife;
                    if(citizen.Life <= 0)
                    {
                        GameManager.Instance.AlienKill();
                        EventManager.Instance.onCitizenRelease.Invoke(citizen);
                    }
                }
                else
                {
                    citizen.ImpostorDetected();
                }
            }
            else
            {
                citizen.Life--;
                frontImage.fillAmount = (float)citizen.Life / (float)citizen.MaxLife;
                if(citizen.Life <= 0)
                {
                    GameManager.Instance.CitizenKill();
                    EventManager.Instance.onCitizenRelease.Invoke(citizen);
                }
            }
        }
    }
}