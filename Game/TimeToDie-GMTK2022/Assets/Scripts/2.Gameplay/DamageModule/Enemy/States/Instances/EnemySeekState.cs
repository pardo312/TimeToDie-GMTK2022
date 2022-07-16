using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySeekState : EnemyStateBase
{
    float timeBetweenSearch;
    int searchingRoute = 0;
    public EnemySeekState(EnemyStateMachine enemy) : base(enemy)
    {
        this.enemy = enemy;
        NavMesh.CalculatePath(enemy.transform.position, enemy.targetPosition.transform.position, NavMesh.AllAreas, enemy.path);
    }

    public override void ProcessInput(Vector2 movement, Vector2 look)
    {

    }

    public override void ReceivedEvent(PlayerStateMachine.Buttons buttons)
    {

    }

    public override void UpdateState()
    {
        Ray seekPlayer = new Ray(enemy.headPosition.position,GameStateMachine.Singleton.player.transform.position - enemy.headPosition.position);
        Debug.DrawRay(seekPlayer.origin, seekPlayer.direction, Color.green, 0.1f);
        RaycastHit result;
        Physics.Raycast(seekPlayer, out result);
        if (result.collider != null)
        {
            if (result.collider.CompareTag("Player"))
            {
                AlignBody(GameStateMachine.Singleton.player.transform.position);
            }
            else
            {
                if (timeBetweenSearch - Time.time > 0 && searchingRoute < enemy.path.corners.Length)
                {
                    if (enemy.path.corners[searchingRoute].magnitude < 0.2f)
                    {
                        searchingRoute += 1;
                    }
                    AlignBody(enemy.path.corners[searchingRoute]);
                    for (int i = 0; i < enemy.path.corners.Length - 1; i++)
                        Debug.DrawLine(enemy.path.corners[i], enemy.path.corners[i + 1], Color.red);
                }
                else
                {
                    SearchRouteToPlayer();
                    searchingRoute = 0;
                }
            }
        }
        else
        {
            enemy.animator.SetFloat(enemy.hashVelocity, 0);
            enemy.animator.SetFloat(enemy.hashTurn, 0);
        }
    }

    public void AlignBody(Vector3 targetPosition)
    {
        float angle =  Vector3 .SignedAngle(enemy.transform.forward, (targetPosition - enemy.transform.position).normalized, Vector3.up);
        Debug.Log(angle);
        if (Mathf.Abs(angle) > 10)
        {
            enemy.animator.SetFloat(enemy.hashVelocity, 0.02f);
            if (angle > 0)
            {
                enemy.animator.SetFloat(enemy.hashTurn, 1);
            }
            else
            {
                enemy.animator.SetFloat(enemy.hashTurn, -1);
            }
        }
        else
        {
            enemy.animator.SetFloat(enemy.hashTurn, 0);
            enemy.animator.SetFloat(enemy.hashVelocity, 1);
        }
    }

    public void SearchRouteToPlayer()
    {
        NavMesh.CalculatePath(enemy.transform.position, GameStateMachine.Singleton.player.transform.position, NavMesh.AllAreas, enemy.path);
        timeBetweenSearch = Time.time + 1;
    }
}
