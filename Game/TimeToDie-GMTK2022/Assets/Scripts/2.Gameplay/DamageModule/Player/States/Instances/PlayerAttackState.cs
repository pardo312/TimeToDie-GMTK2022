using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState: PlayerStateBase
{
    public PlayerAttackState(PlayerStateMachine player) : base(player)
    {
        player.animator.Play(player.attackHashes[player.Weapons[player.currentWeapon].Attack()]);
        player.StartCoroutine(MovementAfterTime());
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {

    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    IEnumerator MovementAfterTime()
    {
        Debug.Log(player.animator.GetCurrentAnimatorClipInfo(0).LongLength);
        yield return new WaitForSeconds(player.animator.GetCurrentAnimatorStateInfo(0).length);
        player.SetState(new MovementState(player));
    }

    public override void UpdateState()
    {
    }
}
