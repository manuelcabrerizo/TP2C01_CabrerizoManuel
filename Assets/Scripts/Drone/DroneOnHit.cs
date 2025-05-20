using UnityEngine;

public class DroneOnHit : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask alienBulletLayer;
    [SerializeField] private LayerMask pickableLayer;

    private Drone state;
    private Rigidbody body;

    private void Awake()
    {
        state = GetComponent<Drone>();
        body = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 6.0f);
        foreach(Collider col in colliders)
        {
            Pickable pickable = null;
            if (col.gameObject.TryGetComponent(out pickable))
            {
                pickable.AttrackToPosition(transform.position);
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (Utils.CheckCollisionLayer(collision.gameObject, wallLayer))
        {
            state.TakeDamageBaseOnVelocity();
            body.AddForce(collision.contacts[0].normal * 5.0f, ForceMode.Impulse);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, alienBulletLayer))
        {
            state.TakeDamage();
            Vector3 dir = (transform.position - other.transform.position).normalized;
            body.AddForce(dir * 5.0f, ForceMode.Impulse);
        }

        if (Utils.CheckCollisionLayer(other.gameObject, pickableLayer))
        {
            Pickable pickable = other.gameObject.GetComponent<Pickable>();
            pickable.Pickup();
        }
    }

}
