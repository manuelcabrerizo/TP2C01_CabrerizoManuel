using System;
public class Coin : Pickable
{
    public static event Action onCoinPickup;
    public override void Pickup()
    {
        onCoinPickup?.Invoke();
        base.Pickup();
    }
}

