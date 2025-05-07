using UnityEngine;

public class Enemy : Entity
{
    [SerializeField] private EnemyData enemyData;
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

    public override void Heal(int healAmount)
    {
        life += healAmount;
        if(life > enemyData.MaxLife)
        {
            life = enemyData.MaxLife;
        }
        frontImage.fillAmount = (float)life / (float)enemyData.MaxLife;
    }
}
