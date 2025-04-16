using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField] private CitizenData citizenData;
    [SerializeField] private MovementGraph movementGraph;
    [SerializeField] private Canvas lifebar;
    [SerializeField] private GameObject male;
    [SerializeField] private GameObject female;
    [SerializeField] private GameObject alien;

    public MovementGraphNode CurrentNode {get; set; }
    public MovementGraphNode TargetNode { get; set; }
    private StateMachine fsm;
    private IdleState idleState;
    private WalkingState walkingState;
    private RunningState runningState;
    private ImpostorState impostorState;
    private IState[] states;
    private Rigidbody body;
    public Rigidbody Body => body;
    private Animator animator;
    public Animator Animator => animator;
    private ParticleSystem particles;
    private int life;
    public int Life => life;
    private bool impostor = false;
    private bool detected = false;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        idleState = GetComponent<IdleState>();
        walkingState = GetComponent<WalkingState>();
        runningState = GetComponent<RunningState>();
        impostorState = GetComponent<ImpostorState>();
        particles = GetComponent<ParticleSystem>();
        animator = GetComponentInChildren<Animator>();
        states = new IState[3];
        states[0] = idleState;
        states[1] = walkingState;
        states[2] = runningState;
        fsm = new StateMachine();
    }

    private void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate(Time.fixedDeltaTime);
    }

    public void OnAquire()
    {
        lifebar.gameObject.SetActive(false);
        male.SetActive(false);
        female.SetActive(false);
        alien.SetActive(false);
        particles.Stop();
        int type = Random.Range(0, 2);
        switch(type)
        {
            case 0:
            {
                male.SetActive(true);
                animator = male.GetComponentInChildren<Animator>();
            }break;
            case 1:
            {
                female.SetActive(true);
                animator = female.GetComponentInChildren<Animator>();
            }break;
        }
        // set to start in a random node of the graph
        CurrentNode = movementGraph.GetRandomNode();
        Vector2 offset = Random.insideUnitCircle * 3.0f;
        Vector3 position = CurrentNode.transform.position + new Vector3(offset.x, 0.0f, offset.y);
        position.y = 1.0f;
        transform.position = position;
        // if its an impostor make it an alien
        fsm.Clear();
        fsm.PushState(states[Random.Range(0, states.Length)]);
        impostor = Random.Range(0, 100) < 10;
        if(impostor)
        {
            GameManager.Instance.AlienHasSpawn();
            particles.Play();
        }
        life = citizenData.MaxLife; 
    }

    public void OnRelease()
    {
        
    }

    public void SetRandomState()
    {
        fsm.ChangeState(states[Random.Range(0, states.Length)]);
    }

    public void ConvertToAlien()
    {
        male.SetActive(false);
        female.SetActive(false);
        alien.SetActive(true);
        animator = alien.GetComponentInChildren<Animator>();
    }

    public bool IsImpostor()
    {
        lifebar.gameObject.SetActive(true);
        return impostor;
    }

    public bool IsDetected()
    {
        return detected;
    }

    public void ImpostorDetected()
    {
        detected = true;
        fsm.ChangeState(impostorState);
    }

    public void ApplayDamage(int damage)
    {
        life -= damage;
    }

}