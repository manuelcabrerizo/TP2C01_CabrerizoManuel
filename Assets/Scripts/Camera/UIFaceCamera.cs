using UnityEngine;

public class UIFaceCamera : MonoBehaviour
{
    private Camera cam;

   private void Awake()
    {
	    if(cam == null)
	    {
	        cam = Camera.main;
	    }
    }
    void LateUpdate()
    {
        transform.rotation = Quaternion.LookRotation(cam.transform.forward * -1.0f);
    }
}
