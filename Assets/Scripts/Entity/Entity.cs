using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour, IPooleable, IDamagable, IHelable
{
    public static event Action<Entity> onEntityRelease;

    [SerializeField] private Canvas lifebar;
    [SerializeField] protected Image frontImage;

    protected int life;

    public int Life => life;

    public virtual void OnGet()
    {
        gameObject.SetActive(true);
        lifebar.gameObject.SetActive(false);
    }

    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage, int maxLife)
    {
        lifebar.gameObject.SetActive(true);
        life -= damage;
        if(life < 0)
        {
            life = 0;
        }
        frontImage.fillAmount = life / (float)maxLife;
    }

    public abstract void Heal(int healAmount);

    public void SendReleaseEvent()
    {
        onEntityRelease?.Invoke(this);
    }
}