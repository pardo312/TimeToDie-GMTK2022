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
        currentArrow.Init(Mathf.Clamp(force/2,0,6),Mathf.Clamp(RuleOfThree(6,20,force),15,20));
        currentArrow.transform.position = damagePoint.transform.position;
        currentArrow.transform.rotation = damagePoint.transform.rotation;
    }

    public float RuleOfThree(float a, float b, float c)
    {
        return (c * b) / a;
    }

    IEnumerator DrawArrow()
    {
        while (true)
        {
            if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                DrawProjectil(Mathf.Abs(tensionTime - Time.time));
                break;
            }
            yield return null;
        }
    }
}
