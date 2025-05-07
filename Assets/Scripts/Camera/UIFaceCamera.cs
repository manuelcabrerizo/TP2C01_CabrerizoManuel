using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward * -1.0f);
    }
}
