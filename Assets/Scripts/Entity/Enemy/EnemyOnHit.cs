using UnityEngine;

public class EnemyOnHit : MonoBehaviour
{
    [SerializeField] private LayerMask hitLayer;

    private Enemy enemy;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(Utils.CheckCollisionLayer(other.gameObject, hitLayer))
        {
            enemy.TakeDamage(1, enemy.EnemyData.MaxLife);
            if(enemy.Life == 0)
            {
                EventManager.Instance.onEntityRelease.Invoke(enemy);
            }
        }
    }
}