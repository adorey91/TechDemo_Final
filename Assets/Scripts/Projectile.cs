using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;

    private Character owner;
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;

        Destroy(gameObject, lifeTime);
    }

    public void Setup(Character character)
    {
        owner = character;
    }

    public void OnTriggerEnter(Collider other)
    {
        Character hit = other.GetComponent<Character>();

        if (hit != owner && hit != null)
        {
            hit.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
