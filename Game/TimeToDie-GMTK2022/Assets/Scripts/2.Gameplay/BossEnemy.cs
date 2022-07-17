using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using TimeToDie.EnemyModule;

public class BossEnemy : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] BossStateMachine boss;
    public EnemySpawner enemySpawner;

    private void Start()
    {
        StartCoroutine(BoosLoop());
        boss.OnDead += StopAttack;
    }

    IEnumerator BoosLoop()
    {
        GameStateMachine.Singleton.SetLevelState(LevelStage.inbetween);
        cinemachineVirtualCamera.Priority = 100;
        yield return new WaitForSeconds(1);
        boss.MoveToTransform(0);
        yield return new WaitForSeconds(8);
        cinemachineVirtualCamera.Priority = 1;
        GameStateMachine.Singleton.SetLevelState(LevelStage.gameMode);
        yield return new WaitForSeconds(10);
        float currentTime;
        while (true)
        {
            float random = Random.Range(0, 10);
            if (random <= 5)
            {
                AttackWithArrows();
            }
            else
            {
                SpawnEnemies();
            }
            currentTime = Time.time + Random.Range(8, 15);
            yield return new WaitForSeconds(currentTime);
        }
    }

    public void StopAttack()
    {
        StopAllCoroutines();
        StartCoroutine(WinRoutine());
    }

    IEnumerator WinRoutine()
    {
        boss.MoveToTransform(5);
        cinemachineVirtualCamera.Priority = 100;
        yield return new WaitForSeconds(3);
        GameStateMachine.Singleton.SetLevelState(LevelStage.victory);
    }

    public void SpawnEnemies()
    {
        boss.MoveToTransform(3);
        boss.SetState(new BossBulnerableState(boss));
    }

    public void AttackWithArrows()
    {
        boss.MoveToTransform(Random.Range(0, 3));
        
        for (int i = 0; i < Random.Range(1, 3); i++)
        {
            enemySpawner.SpawnHandler();
        }
    }
    
}
