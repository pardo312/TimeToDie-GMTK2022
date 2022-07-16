using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementState : PlayerStateBase
{
    int velocityHash;
    int horizontalHash;
    public MovementState(PlayerStateMachine player) : base(player)
    {
        this.player = player;
        velocityHash = Animator.StringToHash("Velocity");
        horizontalHash = Animator.StringToHash("HorizontalVelocity");
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {
        player.Indicator.transform.position = look;
        player.RigidBody.velocity = (player.transform.forward * movement.y + player.transform.right * movement.x).normalized * player.Stats.Velocity * 100 * Time.deltaTime;
        player.transform.LookAt(new Vector3(look.x,player.transform.position.y,look.z));
        player.animator.SetFloat(velocityHash, movement.y);
        player.animator.SetFloat(horizontalHash, movement.x);
    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    public override void UpdateState()
    {

    }
}
