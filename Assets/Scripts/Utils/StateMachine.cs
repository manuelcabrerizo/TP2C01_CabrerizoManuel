using System.Collections.Generic;

public interface IState
{
    public void OnEnter();
    public void OnUpdate(float dt);
    public void OnFixedUpdate(float dt);
    public void OnExit();
}

public class StateMachine
{
    private Stack<IState> states;
    public StateMachine()
    {
        states = new Stack<IState>();
    }

    public IState PeekState()
    {
        return states.Peek();
    }

    public bool IsEmpty()
    {
        return states.Count == 0;
    }

    public void PushState(IState state)
    {
        state.OnEnter();
        states.Push(state);
    }

    public void PopState()
    {
        IState state = states.Pop();
        if (state != null)
        {
            state.OnExit();
        }
    }

    public void ChangeState(IState state)
    {
        if (states.Count > 0)
        {
            if (states.Peek() != state)
            {
                PopState();
                PushState(state);
            }
        }
        else
        {
            PushState(state);
        }
    }

    public void Clear()
    {
        while (states.Count > 0)
        {
            PopState();
        }
    }

    public void Update(float dt)
    {
        IState currentState = states.Peek();
        if (currentState != null)
        {
            currentState.OnUpdate(dt);
        }
    }

    public void FixedUpdate(float dt)
    {
        IState currentState = states.Peek();
        if (currentState != null)
        {
            currentState.OnFixedUpdate(dt);
        }
    }
}
