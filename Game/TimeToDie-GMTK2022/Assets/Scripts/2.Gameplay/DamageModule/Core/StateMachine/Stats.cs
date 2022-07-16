using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stats
{
    [SerializeField] private float velocity;
    public float Velocity { get => velocity; set => velocity = value;}
}
