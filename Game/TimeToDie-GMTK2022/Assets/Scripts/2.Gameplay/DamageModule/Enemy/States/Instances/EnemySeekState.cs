using UnityEngine;

public class EnemySeekState : EnemyStateBase
{
    private Vector3 desiredRotation = new Vector3();

    public EnemySeekState(EnemyStateMachine enemy, string name) : base(enemy, name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public override void EnterState()
    {
        enemy.agent.speed = enemy.statistics.MovementSpeed;
        enemy.animator.Play(enemy.hashMovement);
    }

    public override void UpdateState(float deltaTime)
    {
        if (!enemy.IsPlayerAtRange())
        {
            enemy.agent.isStopped = false;
            enemy.agent.SetDestination(GameStateMachine.Singleton.Player.transform.position);
            //AlignBody(GameStateMachine.Singleton.Player.transform.position, deltaTime);
        }
        else
        {
            enemy.agent.isStopped = true;
            enemy.SetState(enemy.enemyAttackState);
        }
    }

    public override void ExitState() { }

    public override void ProcessInput(Vector2 movement, Vector3 look) { }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons) { }

    private void AlignBody(Vector3 targetPosition, float deltaTime)
    {
        desiredRotation.y =  Vector3.SignedAngle(enemy.transform.forward, (targetPosition - enemy.transform.position).normalized, Vector3.up);
        enemy.transform.eulerAngles = Vector3.Lerp(enemy.transform.rotation.eulerAngles, desiredRotation, deltaTime);
    }
}
