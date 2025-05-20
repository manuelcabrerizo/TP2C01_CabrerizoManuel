using System;
using UnityEngine;

public abstract class Pickable : MonoBehaviour, IPickable, IPooleable
{
    public static event Action<Pickable> onPickableRelease;

    [SerializeField] private float lifeTime = 20;
    private float lifeTimer;
    private Rigidbody body;
    private Collider colision;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        colision = GetComponent<Collider>();
    }

    private void Update()
    {
        if (lifeTimer < 0.0f)
        {
            SendReleaseEvent();
        }
        lifeTimer -= Time.deltaTime;
    }

    public virtual void OnGet()
    {
        gameObject.SetActive(true);
        body.isKinematic = false;
        colision.isTrigger = false;
        lifeTimer = lifeTime;
    }

    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public virtual void Pickup()
    {
        SendReleaseEvent();
    }

    public void SendReleaseEvent()
    {
        onPickableRelease?.Invoke(this);
    }

    public void StartAnimation(Vector3 position)
    {
        Vector2 offset = UnityEngine.Random.insideUnitCircle;
        Vector3 randomUpForce = (Vector3.up + new Vector3(offset.x, 0.0f, offset.y) * 0.25f).normalized * 10.0f;
        body.position = position;
        body.AddForce(randomUpForce, ForceMode.Impulse);
    }

    public void AttrackToPosition(Vector3 position)
    {
        body.isKinematic = true;
        colision.isTrigger = true;
        Vector3 velocity = (position - transform.position);
        body.position += velocity * (6.0f*Time.deltaTime);
    }

}
