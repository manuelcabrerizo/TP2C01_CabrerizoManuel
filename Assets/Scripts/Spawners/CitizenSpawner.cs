using System.Collections.Generic;
using UnityEngine;

public class CitizenSpawner : MonoBehaviour
{
    [SerializeField] Citizen citizenPrefab;
    [SerializeField] GameObject parent;
    private PoolAllocator<Citizen> citizenPool;
    private List<Citizen> spawnedCitizens;

    private void Start()
    {
        citizenPool = new PoolAllocator<Citizen>(OnCreatePooledObject,
            OnDestroyPooledObject, OnGetFromPool, OnReleaseToPool);

        spawnedCitizens = new List<Citizen>();
        for(int i = 0; i < 100; ++i)
        {
            spawnedCitizens.Add(citizenPool.Get());
        }
        EventManager.Instance.onCitizenRelease.AddListener(OnCitizenRelease);
    }

    private void OnCitizenRelease(Citizen citizen)
    {
        spawnedCitizens.Remove(citizen);
        citizenPool.Release(citizen);
    }

    private Citizen OnCreatePooledObject()
    {
        Citizen citizen = Instantiate(citizenPrefab);
        citizen.transform.SetParent(parent.transform);
        return citizen;
    }

    private void OnReleaseToPool(Citizen citizen)
    {
        citizen.OnRelease();
        citizen.gameObject.SetActive(false);
    }

    private void OnGetFromPool(Citizen citizen)
    {
        citizen.gameObject.SetActive(true);
        citizen.OnAquire();
    }

    private void OnDestroyPooledObject(Citizen citizen)
    {
        Destroy(citizen);
    }
}