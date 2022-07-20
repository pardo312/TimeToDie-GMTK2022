using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackState: PlayerStateBase
{
    float animDuration;
    public PlayerAttackState(PlayerStateMachine player, int animHash, float animDuration) : base(player)
    {
        player.RigidBody.velocity = Vector3.zero;
        player.animator.Play(animHash);
        this.animDuration = animDuration;
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
        yield return new WaitForSeconds(animDuration);
        player.SetState(new MovementState(player));
    }

    public override void UpdateState()
    {
    }
}
