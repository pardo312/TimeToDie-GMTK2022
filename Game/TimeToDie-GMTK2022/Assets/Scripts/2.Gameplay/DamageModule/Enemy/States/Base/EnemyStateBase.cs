using UnityEngine;

public abstract class EnemyStateBase : IStateBase
{
    public string name;
    protected EnemyStateMachine enemy;
    
    public EnemyStateBase(EnemyStateMachine enemy, string name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public abstract void EnterState();
    public abstract void UpdateState(float deltaTime);
    public abstract void ExitState();
    public abstract void ProcessInput(Vector2 movement, Vector3 look);
    public abstract void ReceivedEvent(PlayerStateMachine.Buttons buttons);
}
