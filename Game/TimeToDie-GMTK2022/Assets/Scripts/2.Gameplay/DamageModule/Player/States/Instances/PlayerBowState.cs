using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBowState: PlayerStateBase
{
    Coroutine routine;
    float animDuration;
    int isDrawingShot;
    int drawArrowHash;
    public PlayerBowState(PlayerStateMachine player, int animHash, float animDuration) : base(player)
    {
        player.RigidBody.velocity = Vector3.zero;
        player.animator.Play(animHash);
        this.animDuration = animDuration;
        drawArrowHash = Animator.StringToHash("DrawArrow");
        isDrawingShot = Animator.StringToHash("Draw");
        player.animator.SetBool(isDrawingShot, true);
        player.animator.Play(drawArrowHash);
        routine = player.StartCoroutine(MovementAfterTime());
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {
        player.transform.LookAt(new Vector3(look.x, player.transform.position.y, look.z));
        Debug.Log("i am in procees input");
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
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            player.animator.SetBool(isDrawingShot, false);
            player.StopCoroutine(routine);
            player.SetState(new MovementState(player));
        }
    }
}
