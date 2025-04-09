using UnityEngine;

public class DroneVisualRotation : MonoBehaviour
{
    [SerializeField] private float pitchSpeed = 40.0f;
    [SerializeField] private float pitchDamping = 0.4f;
    private float _pitch = 0.0f;
    private const float pitchMax = 20.0f;


    [SerializeField] private float rollSpeed = 40.0f;
    [SerializeField] private float rollDamping = 0.4f;
    private float _roll = 0.0f;
    private const float rollMax = 20.0f;

    
    void Update()
    {
	float x = -Input.GetAxis("Horizontal");
	float z = Input.GetAxis("Vertical");

	_roll += x * rollSpeed * Time.deltaTime;
	_roll = Mathf.Clamp(_roll, -rollMax, rollMax);
	_roll *= Mathf.Pow(rollDamping, Time.deltaTime);
	

	_pitch += z * pitchSpeed * Time.deltaTime;
	_pitch = Mathf.Clamp(_pitch, -pitchMax, pitchMax);
	_pitch *= Mathf.Pow(pitchDamping, Time.deltaTime);

	transform.eulerAngles = new Vector3(
	    _pitch, transform.eulerAngles.y, _roll);
    }
}
