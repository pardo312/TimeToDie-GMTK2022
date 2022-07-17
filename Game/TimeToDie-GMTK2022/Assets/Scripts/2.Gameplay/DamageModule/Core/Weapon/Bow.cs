using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponBase
{
    float tensionTime;
    [SerializeField] public Queue<Arrow> arrows = new Queue<Arrow>();
    [SerializeField] private GameObject bullet;
    public override int Attack()
    {
        if (!(currentCooldown - Time.time < 0))
            return -1;
        currentCooldown = Time.time + weaponStats.Cooldown;
        tensionTime = Time.time;
        StartCoroutine(DrawArrow());
        return 2;
    }

    public void DrawProjectil(float force)
    {
        Arrow currentArrow;
        if (arrows.Count > 0)
            currentArrow = arrows.Dequeue();
        else
        {
            currentArrow = Instantiate(bullet).GetComponent<Arrow>();
            currentArrow.SetParent(this);
        }
        currentArrow.Init(Mathf.Clamp(force,0,6));
    }

    IEnumerator DrawArrow()
    {
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DrawProjectil(Mathf.Abs(tensionTime));
                break;
            }
            yield return null;
        }
    }
}
