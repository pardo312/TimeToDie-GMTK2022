using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] Rigidbody RigidBody;
    [SerializeField] float arrowVelocity;
    [SerializeField] Damage damage;
    Bow parent;
    public void Init(float flyTime, float velocity)
    {
        gameObject.SetActive(true);
        arrowVelocity = velocity;
        StartCoroutine(FlyArrow(flyTime));
    }
    
    IEnumerator FlyArrow(float flyTime)
    {
        float duration = Time.time + flyTime;
        while (duration - Time.time > 0)
        {
            RigidBody.velocity = transform.forward * arrowVelocity + Vector3.down * 1f;
            Ray attackRay = new Ray(transform.position,transform.forward);
            Debug.DrawRay(attackRay.origin,attackRay.direction,Color.red);
            bool impact = false;
            foreach (var hit in Physics.RaycastAll(attackRay, 1, LayerMask.GetMask("Enemy")))
            {
                IDamageable contact = hit.collider.GetComponent<IDamageable>();
                if (contact != null)
                {
                    contact.AddDamage(damage);
                    impact = true;
                    break;
                }
            }
            if (impact)
                break;
            yield return null;
        }
        RigidBody.velocity = Vector3.down * 2f;
        yield return new WaitForSeconds(3);
        parent.arrows.Enqueue(this);
        gameObject.SetActive(false);
    }

    public void SetParent(Bow bow)
    {
        parent = bow;
    }
}
