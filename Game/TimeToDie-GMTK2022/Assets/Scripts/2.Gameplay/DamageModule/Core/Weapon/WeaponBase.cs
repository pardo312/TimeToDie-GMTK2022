using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    [SerializeField] Damage Damage;
    [SerializeField] WeaponStats weaponStats;
    [SerializeField] Transform damagePoint;
    [SerializeField] bool debug;
    protected RaycastHit[] hitResult;
    float currentCooldown;
    public virtual void Attack()
    {
        Debug.Log("attack");
        if (!(currentCooldown - Time.time < 0))
            return;
        hitResult = Physics.BoxCastAll(damagePoint.position, damagePoint.forward, damagePoint.forward);
        foreach (var hit in hitResult)
        {
            IDamageable damageable = hit.collider.GetComponent<IDamageable>();
            if (damageable != null)
            {
                damageable.AddDamage(Damage);
            }
        }
        currentCooldown += Time.time + weaponStats.Cooldown;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(damagePoint.position, Vector3.one);
    }
}
