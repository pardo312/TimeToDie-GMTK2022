using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDisableState : EnemyStateBase
{
    public EnemyDisableState(EnemyStateMachine enemy) : base(enemy)
    {

    }

    public override void ProcessInput(Vector2 movement, Vector2 look)
    {

    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    public override void UpdateState()
    {
    }
}
