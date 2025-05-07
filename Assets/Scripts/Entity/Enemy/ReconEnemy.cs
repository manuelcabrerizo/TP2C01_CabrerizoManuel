using UnityEngine;

public class ReconEnemy : Enemy
{
    [SerializeField] private ReconEnemyData reconEnemyData;
    [SerializeField] private MovementGraph movementGraph;
    private Rigidbody body;
    private Rigidbody target;
    private ParticleSystem ps;
    [SerializeField] float healRadio = 15.0f; 
    private float timeToHeal = 4.0f;
    private float healTimer = 0;
    private MovementGraphNode currentNode;
    private SteeringBehavior arrive;
    private SteeringBehavior face;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        target = null;
        ps = GetComponent<ParticleSystem>();
        arrive = new Arrive(body, null, 10, 20, 5, 10, 0.25f);
        face = new Face(body, null, 2.0f, 4.0f, Mathf.PI*0.1f, Mathf.PI*0.4f, 0.01f);
    }

    public override void OnGet()
    {
        base.OnGet();
        // Set current node
        currentNode = movementGraph.GetRandomNode();
        Vector3 position = currentNode.transform.position;
        position.y = 5.0f;
        transform.position = position;
        target = null;
    }

    private void Update()
    {
        if(healTimer <= 0.0f)
        {
            ps.Play();
            Collider[] colliders = Physics.OverlapSphere(transform.position, healRadio);
            for(int i = 0; i < colliders.Length; ++i)
            {
                GameObject go = colliders[i].gameObject;
                if(go == gameObject)
                {
                    continue;
                }

                if(go.TryGetComponent(out IHelable helable))
                {
                    helable?.Heal(1);
                    if(target == null || !target.gameObject.activeInHierarchy)
                    {
                        Citizen citizen = helable as Citizen;
                        if(citizen != null && citizen.IsImpostor())
                        {
                            target = citizen.GetComponent<Rigidbody>();
                            arrive.SetTarget(target);
                            face.SetTarget(target);
                        }
                    }
                }
            }
            healTimer = timeToHeal;
        }
        healTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if(target != null && target.gameObject.activeInHierarchy)
        {
            SteeringOutput steeringLinear = arrive.GetSteering();
            steeringLinear.linear.y = 0.0f;
            SteeringOutput steeringAngular = face.GetSteering();
            body.AddForce(steeringLinear.linear, ForceMode.Acceleration);
            body.AddTorque(steeringAngular.angular, ForceMode.Acceleration);
        }
    }
}
