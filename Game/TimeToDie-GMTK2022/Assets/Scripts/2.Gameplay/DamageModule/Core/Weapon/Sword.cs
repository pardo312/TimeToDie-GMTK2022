using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : WeaponBase
{
    [SerializeField] float backForce;
    public override int Attack()
    {
        if (base.Attack() != -1)
        {
            StartCoroutine(WaitAnimation(() =>
            {
                foreach (var hit in hitResult)
                {
                    Rigidbody enemyRigid = hit.collider.GetComponent<Rigidbody>();
                    if (enemyRigid != null)
                    {
                        enemyRigid.AddForce(transform.forward * backForce, ForceMode.Impulse);
                    }
                }
            }, animationTime));
            return animationIndex;
        }
        return -1;
    }
}
