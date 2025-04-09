using UnityEngine;

public class EnemyState : MonoBehaviour
{
    [SerializeField] EnemyData enemyData;
    private int life;

    private void Awake()
    {
        life = enemyData.MaxLife;
    }

    public int GetLife()
    {
        return life;
    }

    public void TakeDamage(int damage)
    {
        life -= damage;
    }

    public void Reset()
    {
        life = enemyData.MaxLife;
    }
}
