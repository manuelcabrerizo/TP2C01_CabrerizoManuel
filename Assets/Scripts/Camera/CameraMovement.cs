using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float mouseSpeed = 1.0f;
    [SerializeField] private float maxDistance = 5.0f;
    [SerializeField] private float cameraRadius = 0.5f;
    [SerializeField] private LayerMask wallLayer;

    private float _yaw = 0;
    private float _pitch = 0;
    
    private void Awake()
    {
	    Vector3 targetPosition = target.transform.position;
	    Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
	    direction = Quaternion.AngleAxis(_pitch, Vector3.right) * direction;
	    direction = Quaternion.AngleAxis(_yaw, Vector3.up) * direction;
	    direction.Normalize();
	
	    transform.position = targetPosition + direction * maxDistance;	
	    transform.LookAt(targetPosition, Vector3.up);
    }

    private void Update()
    {
        FollowTarget();
    }
    
    private void FollowTarget()
    {
	    if(Input.GetMouseButtonDown(0))
	    {
                Cursor.lockState = CursorLockMode.Locked;
	    }
	    if(Input.GetKeyDown(KeyCode.Escape))
	    {
                Cursor.lockState = CursorLockMode.None;
	    }

	    if(Cursor.lockState == CursorLockMode.Locked)
	    {	
	        float mouseX = Input.GetAxis("Mouse X");
	        float mouseY = -Input.GetAxis("Mouse Y");

	        _yaw += mouseX * mouseSpeed;
	        _pitch = Mathf.Clamp(_pitch + mouseY * mouseSpeed, -89.0f, 89.0f);
             
	        Vector3 targetPosition = target.transform.position + Vector3.up * 0.5f;
	        Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
	        direction = Quaternion.AngleAxis(_pitch, Vector3.right) * direction;
	        direction = Quaternion.AngleAxis(_yaw, Vector3.up) * direction;
	        direction.Normalize();

            Vector3 cameraPosition = targetPosition + direction * maxDistance;
            RaycastHit hitInfo;
            if (Physics.SphereCast(targetPosition, cameraRadius, direction, out hitInfo, maxDistance, wallLayer))
            {
                cameraPosition = targetPosition + direction * hitInfo.distance;
            }
            else
            {
                // TODO: test if this realy help
                if (Physics.Raycast(targetPosition, direction, out hitInfo, maxDistance, wallLayer))
                {
                    cameraPosition = targetPosition + direction * hitInfo.distance;
                }
            }

            transform.position = cameraPosition;
	        transform.LookAt(targetPosition, Vector3.up);
	    }
    }

    public float GetYaw()
    {
	    return _yaw;
    }
}
