using UnityEngine;

public class DroneBullet : Bullet
{
    private ParticleSystem ps;
    private Rigidbody body;  

    private void Awake()
    {
        ps = GetComponentInChildren<ParticleSystem>();
        body = GetComponent<Rigidbody>();
    }

    public override void OnGet()
    {
        base.OnGet();
        ps.Play();
    }

    public override void OnRelease()
    {
        ps.Stop();
        base.OnRelease();
    }

    public void Move(Vector3 movement)
    {
        body.position += movement;
    }

    public float GetMass()
    {
        return body.mass;
    }
}