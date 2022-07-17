using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] protected Damage Damage;
    [SerializeField] protected WeaponStats weaponStats;
    [SerializeField] protected Transform damagePoint;
    [SerializeField] protected Transform ArmVisual;
    [SerializeField] bool debug;
    [SerializeField] protected float animationTime;
    [SerializeField] protected int animationIndex;
    protected RaycastHit[] hitResult;
    protected float currentCooldown;
    public virtual int Attack()
    {
        if (!(currentCooldown - Time.time < 0))
            return -1;
        StartCoroutine(WaitAnimation(()=>{
            hitResult = Physics.BoxCastAll(damagePoint.position, Vector3.one / 2, damagePoint.up);
            foreach (var hit in hitResult)
            {
                IDamageable damageable = hit.collider.GetComponent<IDamageable>();
                if (damageable != null)
                {
                    damageable.AddDamage(Damage);
                }
            }
        },animationTime));
        currentCooldown = Time.time + weaponStats.Cooldown;
        return animationIndex;
    }

    public void EnableVisual(bool state)
    {
        if (state)
        {
            ArmVisual.gameObject.SetActive(true);
        }
        else
        {
            ArmVisual.gameObject.SetActive(false);
        }
    }

    public IEnumerator WaitAnimation(Action callback, float time)
    {
        yield return new WaitForSeconds(time);
        callback?.Invoke();
    }

    private void OnDrawGizmos()
    {
        if (debug)
        {
            Gizmos.DrawCube(damagePoint.position, Vector3.one);
        }
    }
}
