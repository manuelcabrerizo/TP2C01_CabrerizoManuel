using UnityEngine;

public class DroneRotate : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private CameraMovement cam;
    
    private float yaw = 0.0f;

    private void Update()
    {	
	    float diffYaw = cam.GetYaw() - yaw;

	    if(Mathf.Abs(diffYaw) >= 0.1f)
	    {
	        yaw += diffYaw * playerData.RotationSpeed * Time.deltaTime;

	    }
	    
	    transform.eulerAngles = new Vector3(
	       transform.eulerAngles.x, yaw, transform.eulerAngles.z);	
    }
}
