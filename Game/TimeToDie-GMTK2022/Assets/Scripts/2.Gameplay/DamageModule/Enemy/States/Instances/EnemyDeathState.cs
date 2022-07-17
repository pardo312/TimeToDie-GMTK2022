using System.Collections;
using UnityEngine;

public class EnemyDeathState : EnemyStateBase
{
    private WaitForSeconds deathAnimationWaitSeconds;

    public EnemyDeathState(EnemyStateMachine enemy, string name) : base(enemy, name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public override void EnterState()
    {
        enemy.animator.Play(enemy.hashDeath);

        if (deathAnimationWaitSeconds == null)
            deathAnimationWaitSeconds = new WaitForSeconds(enemy.durationDeath + 1f);

        enemy.StartCoroutine(FinishDeath(deathAnimationWaitSeconds));
    }

    public override void UpdateState(float deltaTime)
    {
    }

    public override void ExitState()
    {
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {
    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {
    }

    private IEnumerator FinishDeath(WaitForSeconds waitSeconds)
    {
        yield return waitSeconds;
        enemy.gameObject.SetActive(false);
    }
}
