using System;
using UnityEngine;

public class Citizen : Entity
{
    [SerializeField] private CitizenData citizenData;
    [SerializeField] private MovementGraph movementGraph;
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
        fsm.Update();
    }

    private void FixedUpdate()
    {
        fsm.FixedUpdate();
    }

    public override void OnGet()
    {
        base.OnGet();
        male.SetActive(false);
        female.SetActive(false);
        alien.SetActive(false);
        particles.Stop();
        int type = UnityEngine.Random.Range(0, 2);
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
        Vector2 offset = UnityEngine.Random.insideUnitCircle * 3.0f;
        Vector3 position = CurrentNode.transform.position + new Vector3(offset.x, 0.0f, offset.y);
        position.y = 1.0f;
        transform.position = position;
        body.velocity = Vector3.zero;
        
        fsm.Clear();
        fsm.PushState(states[UnityEngine.Random.Range(0, states.Length)]);
        impostor = false;
        detected = false;
        life = citizenData.MaxLife; 
    }

    public override void OnRelease()
    {
        fsm.Clear();
        base.OnRelease();
    }

    public void MakeImpostor()
    {
        impostor = true;
        particles.Play();
    }

    public void SetRandomState()
    {
        fsm.ChangeState(states[UnityEngine.Random.Range(0, states.Length)]);
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

    public override void Heal(int healAmount)
    {
        if(impostor && detected)
        {
            life += healAmount;
            if(life > citizenData.MaxLife)
            {
                life = citizenData.MaxLife;
            }
            frontImage.fillAmount = (float)life / (float)citizenData.MaxLife;
        }
    }
}