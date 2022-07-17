using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CubeRollerBehavior : MonoBehaviour
{
    [SerializeField] List<Quaternion> posibleRotations = new List<Quaternion>();
    [SerializeField] float timeBetweenSwaps;
    [SerializeField] float rotationVelocity;
    [SerializeField] int targetRotation;
    [SerializeField] CinemachineVirtualCamera cinemachineVirtualCamera;
    float currentTime = 0.1f;

    private void Start()
    {
        currentTime = timeBetweenSwaps;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTime - Time.time < 0)
        {
            SwapRotation();
            cinemachineVirtualCamera.Priority = 11;
            GameStateMachine.Singleton.SetLevelState(LevelStage.inbetween);
            currentTime = Time.time + timeBetweenSwaps;
        }
    }

    void SwapRotation()
    {
        StartCoroutine(RotateRoutine());
    }

    IEnumerator RotateRoutine()
    {
        int random = Random.Range(0, posibleRotations.Count);
        float time = Time.time + 2;
        while (time - Time.time > 0)
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,posibleRotations[random],rotationVelocity * Time.deltaTime);
            yield return null;
        }
        transform.rotation = posibleRotations[random];
        GameStateMachine.Singleton.SetLevelState(LevelStage.gameMode);
        cinemachineVirtualCamera.Priority = 9;
    }

    [ContextMenu("PutTargetRotation")]
    public void PutTargetRotation()
    {
        transform.rotation = posibleRotations[targetRotation];
    }

    [ContextMenu("GetCurrentRotation")]
    public void GetCurrentRotation()
    {
        posibleRotations.Add(transform.rotation);
    }
}
