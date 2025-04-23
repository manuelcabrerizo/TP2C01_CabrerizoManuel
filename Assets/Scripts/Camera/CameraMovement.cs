using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private GameObject target;
    [SerializeField] private LayerMask wallLayer;

    private bool isFirstPerson = false;
    [SerializeField] private GameObject gun;

    private float _yaw = 0;
    private float _pitch = 0;
    
    private void Awake()
    {
	    Vector3 targetPosition = target.transform.position;
	    Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
	    direction = Quaternion.AngleAxis(_pitch, Vector3.right) * direction;
	    direction = Quaternion.AngleAxis(_yaw, Vector3.up) * direction;
	    direction.Normalize();
	
	    transform.position = targetPosition + direction * cameraData.MaxDistance;	
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
        if(Input.GetKeyDown(KeyCode.Alpha1))
	    {
                isFirstPerson = !isFirstPerson;
	    }

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        _yaw += mouseX * cameraData.MouseSpeed;
        _pitch = Mathf.Clamp(_pitch + mouseY * cameraData.MouseSpeed, -89.0f, 89.0f);
            
        Vector3 targetPosition = target.transform.position + Vector3.up * 0.5f;
        Vector3 direction = new Vector3(0.0f, 0.0f, -1.0f);
        direction = Quaternion.AngleAxis(_pitch, Vector3.right) * direction;
        direction = Quaternion.AngleAxis(_yaw, Vector3.up) * direction;
        direction.Normalize();

        if(!isFirstPerson)
        {
            Vector3 cameraPosition = targetPosition + direction * cameraData.MaxDistance;
            RaycastHit hitInfo;
            if (Physics.SphereCast(targetPosition, cameraData.CameraRadius, direction, out hitInfo, cameraData.MaxDistance, wallLayer))
            {
                cameraPosition = targetPosition + direction * hitInfo.distance;
            }
            else
            {
                // TODO: test if this realy help
                if (Physics.Raycast(targetPosition, direction, out hitInfo, cameraData.MaxDistance, wallLayer))
                {
                    cameraPosition = targetPosition + direction * hitInfo.distance;
                }
            }

            transform.position = cameraPosition;
            transform.LookAt(targetPosition, Vector3.up);
        }
        else
        {
            transform.position = gun.transform.position;
            transform.LookAt(transform.position - direction, Vector3.up);
        }
    }

    public float GetYaw()
    {
	    return _yaw;
    }
}
