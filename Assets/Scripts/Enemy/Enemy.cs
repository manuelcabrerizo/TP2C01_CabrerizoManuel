using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected EnemyState state;
    protected SphereCollider collision;
    protected Rigidbody body;

    public EnemyState State => state;
    public SphereCollider Collision => collision;
    public Rigidbody Body => body;

    private void Awake()
    {
        state = GetComponent<EnemyState>();
        collision = GetComponent<SphereCollider>();
        body = GetComponent<Rigidbody>();
    } 

    public virtual void OnAquire()
    {
        state.Reset();
        collision.enabled = true;
        body.position = transform.position;
        body.velocity = Vector3.zero;
    }
    public virtual void OnRelease()
    {
        collision.enabled = false;
    }
}
