using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDisableState : PlayerStateBase
{
    public PlayerDisableState(PlayerStateMachine player) : base(player)
    {
    }

    public override void ProcessInput(Vector2 movement, Vector3 look)
    {

    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    public override void UpdateState()
    {
    }
}
