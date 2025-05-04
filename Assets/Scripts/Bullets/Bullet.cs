using UnityEngine;

public abstract class Bullet : MonoBehaviour, IPooleable
{
    public virtual void OnGet()
    {
        gameObject.SetActive(true);
    }
    public virtual void OnRelease()
    {
        gameObject.SetActive(false);
    }
}