using UnityEngine;
using UnityEngine.UI;

public class CitizenOnHit : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;
    [SerializeField] private Canvas lifebar;
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
            lifebar.gameObject.SetActive(true);
            if(citizen.IsImpostor())
            {
                if(citizen.IsDetected())
                {
                    citizen.Life--;
                    frontImage.fillAmount = (float)citizen.Life / (float)citizen.MaxLife;
                    if(citizen.Life <= 0)
                    {
                        citizen.gameObject.SetActive(false);
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
                    citizen.gameObject.SetActive(false);
                }
            }
        }
    }
}