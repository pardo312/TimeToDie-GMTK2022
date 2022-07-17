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
        if(buttons == PlayerStateMachine.Buttons.Attack)
        {
            int currentHash = player.attackHashes[player.Weapons[player.currentWeapon].Attack()];
            if (currentHash == 2)
            {
                player.SetState(new PlayerBowState(player, currentHash, 6));
            }
            else if (currentHash != -1)
            {
                player.SetState(new PlayerAttackState(player, currentHash, player.WeaponDuration[player.currentWeapon]));
            }
        }
    }

    public override void UpdateState()
    {

    }
}
