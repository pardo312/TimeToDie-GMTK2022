using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow : WeaponBase
{
    public override int Attack()
    {
        if (base.Attack() != -1)
        {
            StartCoroutine(WaitAnimation(() =>
            {
                //Do in between
            }, animationTime));
            return animationIndex;
        }
        return -1;
    }
}
