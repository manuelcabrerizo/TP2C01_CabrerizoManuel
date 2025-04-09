using UnityEngine;

public class DroneOnHit : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;

    private DroneState state;

    private void Awake()
    {
        state = GetComponent<DroneState>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, wallLayer))
        {
            state.TakeDamageBaseOnVelocity();
        }
    }

}
