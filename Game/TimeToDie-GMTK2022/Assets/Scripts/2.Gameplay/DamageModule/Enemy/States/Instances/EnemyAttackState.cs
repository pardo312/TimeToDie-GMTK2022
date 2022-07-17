using System.Collections;
using UnityEngine;

public class EnemyAttackState : EnemyStateBase
{
    private WaitForSeconds attackAnimationWaitSeconds;

    public EnemyAttackState(EnemyStateMachine enemy, string name) : base(enemy, name)
    {
        this.enemy = enemy;
        this.name = name;
    }

    public override void EnterState()
    {
        Attack();
    }

    public override void UpdateState(float deltaTime) { }

    public override void ExitState() 
    {
        enemy.StopAllCoroutines();
    }

    public override void ProcessInput(Vector2 movement, Vector3 look) { }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons) { }

    private void Attack()
    {
        Vector3 playerPosition = GameStateMachine.Singleton.Player.transform.position;
        playerPosition.y = enemy.transform.position.y;
        enemy.transform.LookAt(playerPosition);

        if (enemy.weapon.Attack() != -1)
        {
            enemy.animator.Play(enemy.hashAttack);

            if (attackAnimationWaitSeconds == null)
                attackAnimationWaitSeconds = new WaitForSeconds(enemy.durationAttack);
                
            enemy.StartCoroutine(FinishAttack(attackAnimationWaitSeconds));
        }
    }

    private IEnumerator FinishAttack(WaitForSeconds waitSeconds)
    {
        yield return waitSeconds;
        enemy.SetState(enemy.enemyIdleState);
        
    }
}
