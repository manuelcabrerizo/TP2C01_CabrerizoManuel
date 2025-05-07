using UnityEngine;

public abstract class Bullet : MonoBehaviour, IPooleable
{
    protected int damage;
    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }
}