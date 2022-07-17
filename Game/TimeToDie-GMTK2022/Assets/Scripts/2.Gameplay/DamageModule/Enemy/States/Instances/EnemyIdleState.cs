using UnityEngine;

public class EnemyIdleState : EnemyStateBase
{
    private float idleTime;

    public EnemyIdleState(EnemyStateMachine enemy, string name) : base(enemy, name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public override void EnterState()
    {
        idleTime = 0;
        enemy.animator.Play(enemy.hashIdle);
    }

    public override void UpdateState(float deltaTime)
    {
        idleTime += deltaTime;
        if (idleTime > enemy.statistics.WaitTimeBetweenAttacks)
            enemy.SetState(enemy.enemySeekState);

    }

    public override void ExitState()
    {
        enemy.StopAllCoroutines();
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {

    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }
}
