using UnityEngine;
using UnityEngine.Events;


public class EventManager : MonoBehaviour
{
    public static EventManager Instance;

    public UnityEvent<RaycastHit, Ray> onEnemyHit;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        onEnemyHit = new UnityEvent<RaycastHit, Ray>();

    }

    private void OnDestroy()
    {
        onEnemyHit.RemoveAllListeners();
    }
}
