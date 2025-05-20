using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private EnemyData enemyData;
    [SerializeField] private LayerMask hitLayer;
    public EnemyData EnemyData => enemyData;
    private void Awake()
    {
        life = enemyData.MaxLife;
        OnAwaken();
    } 
    public override void OnGet()
    {
        base.OnGet();
        life = enemyData.MaxLife;
    }
    public override void OnRelease()
    {
        base.OnRelease();
    }
    protected virtual void OnAwaken() {}

    private void OnTriggerEnter(Collider other)
    {
        if (Utils.CheckCollisionLayer(other.gameObject, hitLayer))
        {
            OnDamage(this.gameObject);
        }
    }

    public override void Heal(int healAmount)
    {
        life += healAmount;
        if(life > enemyData.MaxLife)
        {
            life = enemyData.MaxLife;
        }
        frontImage.fillAmount = (float)life / (float)enemyData.MaxLife;
    }

    protected virtual void OnDamage(GameObject gameObject)
    {
        if (gameObject != this.gameObject) return;
        TakeDamage(1, EnemyData.MaxLife);
        if (Life == 0)
        {
            SendReleaseEvent();
        }
    }
}
