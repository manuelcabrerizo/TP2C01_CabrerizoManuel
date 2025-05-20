using System;

public class Health : Pickable
{
    public static event Action<int> onHealPickup;
    public override void Pickup()
    {
        onHealPickup?.Invoke(10);
        base.Pickup();
    }
}

