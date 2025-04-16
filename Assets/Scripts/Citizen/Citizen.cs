using UnityEngine;

public class Citizen : MonoBehaviour
{
    [SerializeField] private MovementGraph movementGraph;
    public MovementGraph MovementGraph => movementGraph;

    public MovementGraphNode CurrentNode {get; set; }
    public MovementGraphNode TargetNode { get; set; }

    public float ReachRadio = 3.0f;
    public float YPosition = 1.0f;

    private StateMachine fsm;

    private IdleState idleState;
    private WalkingState walkingState;
    private RunningState runningState;

    private ImpostorState impostorState;
    private IState[] states;

    private bool impostor = false;

    private Rigidbody body;

    public Rigidbody Body => body;

    [SerializeField] private GameObject male;
    [SerializeField] private GameObject female;
    [SerializeField] private GameObject alien;
    private Animator animator;

    public Animator Animator => animator;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        idleState = GetComponent<IdleState>();
        walkingState = GetComponent<WalkingState>();
        runningState = GetComponent<RunningState>();
        impostorState = GetComponent<ImpostorState>();

        animator = GetComponentInChildren<Animator>();

        states = new IState[3];
        states[0] = idleState;
        states[1] = walkingState;
        states[2] = runningState;

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
    }

    private void Start()
    {
        // set to start in a random node of the graph
        CurrentNode = movementGraph.GetRandomNode();
        Vector2 offset = Random.insideUnitCircle * 3.0f;
        Vector3 position = CurrentNode.transform.position + new Vector3(offset.x, 0.0f, offset.y);
        position.y = YPosition;
        transform.position = position;

        fsm = new StateMachine();
        fsm.PushState(states[Random.Range(0, states.Length)]);
        impostor = Random.Range(0, 100) < 5;
        if(impostor)
            fsm.ChangeState(impostorState);
    }

    private void Update()
    {
        fsm.Update(Time.deltaTime);
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate(Time.fixedDeltaTime);
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
}
