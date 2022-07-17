using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowState: PlayerStateBase
{
    float animDuration;
    int velocityHash;
    int horizontalHash;
    int isDrawingShot;
    int drawArrowHash;
    public PlayerBowState(PlayerStateMachine player, int animHash, float animDuration) : base(player)
    {
        player.RigidBody.velocity = Vector3.zero;
        player.animator.Play(animHash);
        this.animDuration = animDuration;
        velocityHash = Animator.StringToHash("Velocity");
        horizontalHash = Animator.StringToHash("HorizontalVelocity");
        drawArrowHash = Animator.StringToHash("DrawArrow");
        isDrawingShot = Animator.StringToHash("IsDrawing");
        player.StartCoroutine(MovementAfterTime());
        player.animator.SetBool(isDrawingShot, true);
        player.animator.Play(drawArrowHash);
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            player.animator.SetBool(isDrawingShot, false);
            player.SetState(new MovementState(player));
        }
        player.transform.LookAt(new Vector3(look.x, player.transform.position.y, look.z));
    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    IEnumerator MovementAfterTime()
    {
        yield return new WaitForSeconds(animDuration);
        player.animator.SetBool(isDrawingShot, false);
        player.SetState(new MovementState(player));
    }

    public override void UpdateState()
    {

    }
}
