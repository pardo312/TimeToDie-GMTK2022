using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class CubeRollerBehavior : MonoBehaviour
{
    [SerializeField] List<Quaternion> posibleRotations = new List<Quaternion>();
    [SerializeField] float timeBetweenSwaps;
    [SerializeField] float rotationVelocity;
    
    float currentTime = 0.1f;

    // Update is called once per frame
    void Update()
    {
        if (currentTime - Time.time < 0)
        {
            SwapRotation();
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
        while (transform.rotation != posibleRotations[random])
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation,posibleRotations[random],rotationVelocity * Time.deltaTime);
            yield return null;
        }
    }

    [ContextMenu("GetCurrentRotation")]
    public void GetCurrentRotation()
    {
        posibleRotations.Add(transform.rotation);
    }
}
