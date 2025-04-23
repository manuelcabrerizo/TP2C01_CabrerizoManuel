using UnityEngine;

public class DroneOnHit : MonoBehaviour
{
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask alienBulletLayer;

    private DroneState state;
    private Rigidbody body;

    private void Awake()
    {
        state = GetComponent<DroneState>();
        body = GetComponent<Rigidbody>();
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
    }

}
