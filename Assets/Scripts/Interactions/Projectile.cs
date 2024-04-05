using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    SoundManager soundManager;
    [SerializeField] private float moveSpeed;
    [SerializeField] private int damage;
    [SerializeField] private float lifeTime;

    private Character owner;
    private Rigidbody rb;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.velocity = transform.forward * moveSpeed;
        soundManager = FindAnyObjectByType<SoundManager>();
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
            Debug.Log(hit.name);
            if (hit.name == "Player")
                soundManager.PlayerAttackedAudio();
            else if(hit.name == "Enemy" || hit.name == "Enemy(Clone)")
                soundManager.EnemyAttackedAudio();
                
            hit.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}