using UnityEngine;

public abstract class BossStateBase
{
    protected BossStateMachine enemy;
    
    public BossStateBase(BossStateMachine enemy)
    {
        this.enemy = enemy;
    }

    public abstract void UpdateState(float deltaTime);
    public abstract void ProcessInput(Vector2 movement, Vector3 look);
}
