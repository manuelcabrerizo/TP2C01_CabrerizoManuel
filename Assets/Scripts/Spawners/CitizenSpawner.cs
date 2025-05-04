using UnityEngine;

public class CitizenSpawner : MonoBehaviourSingleton<CitizenSpawner>
{
    [SerializeField] private PoolManager poolManager;

    [SerializeField] Citizen citizenPrefab;

    private void Start()
    {
        poolManager.InitPool(citizenPrefab, transform, 200);
        EventManager.Instance.onCitizenRelease.AddListener(OnCitizenRelease);
    }

    public void Clear()
    {
        poolManager.Clear<Citizen>();
    }

    public void SpawnAll(int count)
    {
        for(int i = 0; i < count; ++i)
        {
            poolManager.Get<Citizen>(transform);
        }
    }

    private void OnCitizenRelease(Citizen citizen)
    {
        poolManager.Release(citizen);
    }
}