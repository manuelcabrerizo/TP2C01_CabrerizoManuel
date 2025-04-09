using UnityEngine;

public class DroneMovement : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private Rigidbody body;

    public Rigidbody Body => body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
	    Movement();
    }

    private void Movement()
    {
        float x = Input.GetAxis("Horizontal");
	    float y = 0;
	    float z = Input.GetAxis("Vertical");

	    if(Input.GetKey(KeyCode.Space))
	    {
	        y = 1.0f;
	    }
	    else if(Input.GetKey(KeyCode.LeftControl))
	    {
	        y = -1.0f;
	    }

	    Vector3 forward = transform.forward;
	    Vector3 right = transform.right;
	    Vector3 up = Vector3.up;
	
	    forward.y = 0;
	    right.y = 0;
	    forward.Normalize();
	    right.Normalize();
	
        Vector3 direction = forward * z + right * x + up * y;
        if (direction.sqrMagnitude < 0.01f)
        {
            return;
        }
        direction.Normalize();

        body.AddForce(direction * playerData.Speed, ForceMode.Acceleration);

	    Vector3 velocity = body.velocity;
	    if(velocity.sqrMagnitude > (playerData.MaxSpeed * playerData.MaxSpeed))
	    {
            body.velocity = velocity.normalized * playerData.MaxSpeed;
	    }
    }
}

