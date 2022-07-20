using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class PlayerStateBase
{
    protected PlayerStateMachine player;

    public PlayerStateBase(PlayerStateMachine player)
    {
        this.player = player;
    }

    public abstract void UpdateState();
    public abstract void ProcessInput(Vector2 movement, Vector3 look);
    public abstract void ReceivedEvent(PlayerStateMachine.Buttons buttons);
}
