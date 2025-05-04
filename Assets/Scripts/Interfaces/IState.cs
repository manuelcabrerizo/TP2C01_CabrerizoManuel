public interface IState
{
    public void OnEnter();
    public void OnUpdate(float dt);
    public void OnFixedUpdate(float dt);
    public void OnExit();
}