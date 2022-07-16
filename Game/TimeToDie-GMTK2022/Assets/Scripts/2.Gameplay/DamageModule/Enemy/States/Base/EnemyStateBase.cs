using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyStateBase
{
    protected EnemyStateMachine enemy;

    public EnemyStateBase(EnemyStateMachine player)
    {
        this.enemy = player;
    }

    public abstract void UpdateState();
    public abstract void ProcessInput(Vector2 movement, Vector2 look);
    public abstract void ReceivedEvent(PlayerStateMachine.Buttons buttons);
}
