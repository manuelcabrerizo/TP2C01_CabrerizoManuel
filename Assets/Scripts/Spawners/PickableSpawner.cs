using UnityEngine;

public class PickableSpawner : Spawner<PickableSpawner, Pickable>
{
    [SerializeField] private Health healthPrefab;
    [SerializeField] private Coin coinPrefab;
    [SerializeField] private int initialHeathCount = 4;
    [SerializeField] private int initialCoinCount = 20;

    protected override void OnAwaken()
    {
        PoolManager.Instance.InitPool(healthPrefab, transform, initialHeathCount);
        PoolManager.Instance.InitPool(coinPrefab, transform, initialCoinCount);
        Pickable.onPickableRelease += OnPickableRelease;
    }

    protected override void OnDestroyed()
    {
        Pickable.onPickableRelease -= OnPickableRelease;
    }

    private void OnPickableRelease(Pickable pickable)
    {
        Pickable test = null;
        if (test = pickable as Health)
        {
            PoolManager.Instance.Release((Health)pickable);
        }
        else if (test = pickable as Coin)
        {
            PoolManager.Instance.Release((Coin)pickable);
        }
    }
}
