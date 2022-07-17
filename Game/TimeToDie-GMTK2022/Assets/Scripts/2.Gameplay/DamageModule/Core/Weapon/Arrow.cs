using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] Rigidbody RigidBody;
    [SerializeField] float arrowVelocity;
    [SerializeField] Damage damage;
    Bow parent;
    public void Init(float flyTime)
    {
        gameObject.SetActive(true);
        StartCoroutine(FlyArrow(flyTime));
    }
    
    IEnumerator FlyArrow(float flyTime)
    {
        float duration = Time.time + flyTime;
        while (duration - Time.time > 0)
        {
            RigidBody.velocity = transform.forward * arrowVelocity;
            Ray attackRay = new Ray(transform.position,transform.forward);
            Debug.DrawRay(attackRay.origin,attackRay.direction,Color.red);
            foreach (var hit in Physics.RaycastAll(attackRay, 1, LayerMask.GetMask("Enemy")))
            {
                IDamageable contact = hit.collider.GetComponent<IDamageable>();
                if (contact != null)
                {
                    contact.AddDamage(damage);
                    break;
                }
            }
            yield return null;
        }
        parent.arrows.Enqueue(this);
        gameObject.SetActive(false);
    }

    public void SetParent(Bow bow)
    {
        parent = bow;
    }
}
