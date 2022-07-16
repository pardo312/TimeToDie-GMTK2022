using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] Damage Damage;
    [SerializeField] WeaponStats weaponStats;
    [SerializeField] Transform damagePoint;
    [SerializeField] bool debug;
    [SerializeField] protected float animationTime;
    [SerializeField] protected int animationIndex;
    protected RaycastHit[] hitResult;
    float currentCooldown;
    public virtual int Attack()
    {
        Debug.Log("attack");
        if (!(currentCooldown - Time.time < 0))
            return -1;
        StartCoroutine(WaitAnimation(()=>{
            hitResult = Physics.BoxCastAll(damagePoint.position, damagePoint.forward / 2, damagePoint.forward);
            foreach (var hit in hitResult)
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.AddDamage(Damage);
                }
            }
            currentCooldown += Time.time + weaponStats.Cooldown;
        },animationTime));
        return animationIndex;
    }

    public IEnumerator WaitAnimation(Action callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(damagePoint.position, Vector3.one);
    }
}
