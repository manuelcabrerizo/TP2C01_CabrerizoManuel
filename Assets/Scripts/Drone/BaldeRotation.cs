using UnityEngine;

public class BaldeRotation : MonoBehaviour
{
    void Update()
    {
        transform.Rotate(Vector3.up, 2000.0f * Time.deltaTime);
    }
}
