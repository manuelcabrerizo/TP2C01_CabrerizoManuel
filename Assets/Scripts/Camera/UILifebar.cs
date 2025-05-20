using UnityEngine;

public class UILifebar : MonoBehaviour
{
    [SerializeField] private GameObject lifebar;
    [SerializeField] private Vector3 offset;
    
    void LateUpdate()
    {
        lifebar.transform.position = transform.position + offset;
        lifebar.transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward * -1.0f);
    }
}
