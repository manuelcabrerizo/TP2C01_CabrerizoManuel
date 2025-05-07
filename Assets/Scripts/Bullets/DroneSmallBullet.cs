using UnityEngine;

public class DroneSmallBullet : Bullet
{
    private TrailRenderer trailRenderer;
    private void Awake()
    {
        trailRenderer = GetComponent<TrailRenderer>();
    }

    public override void OnGet()
    {
        base.OnGet();
    }
    public override void OnRelease()
    {
        base.OnRelease();
        trailRenderer.Clear();
    }
}