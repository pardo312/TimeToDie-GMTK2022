using System.Collections;
using UnityEngine;

public class EnemyHurstState : EnemyStateBase
{
    private WaitForSeconds hurtAnimationWaitSeconds;

    public EnemyHurstState(EnemyStateMachine enemy, string name) : base(enemy, name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public override void EnterState()
    {
        enemy.animator.Play(enemy.hashTakeDamage);

        if (hurtAnimationWaitSeconds == null)
            hurtAnimationWaitSeconds = new WaitForSeconds(enemy.animator.GetCurrentAnimatorStateInfo(0).length + 0.5f);

        enemy.StartCoroutine(FinishHurt(hurtAnimationWaitSeconds));
    }

    public override void UpdateState(float deltaTime)
    {
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

    private IEnumerator FinishHurt(WaitForSeconds waitSeconds)
    {
        yield return waitSeconds;
        enemy.SetState(enemy.enemyIdleState);
    }
}
