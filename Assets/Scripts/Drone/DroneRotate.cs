using UnityEngine;

public class DroneRotate : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    [SerializeField] private CameraMovement cam;
    
    private float _yaw = 0.0f;

    private void Update()
    {	
	    float diffYaw = cam.GetYaw() - _yaw;

	    if(Mathf.Abs(diffYaw) >= 0.1f)
	    {
	        _yaw += diffYaw * playerData.RotationSpeed * Time.deltaTime;

	    }
	    
	    transform.eulerAngles = new Vector3(
	       transform.eulerAngles.x, _yaw, transform.eulerAngles.z);	
    }
}
