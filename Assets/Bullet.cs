using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // velocity

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] private int bulletDmg = 1;

    private Transform target;
    private float debuffRate = 1f; // debuffRate attribute from turret that shot out the bullet

    public void SetTarget(Transform _target)
    {
        target = _target; // setter method for outer access
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (!target)
        {
            Destroy(gameObject);
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized; // between 0 and 1

        rb.velocity = direction * bulletSpeed; // velocity = direction and speed
    }

    public void SetDebuffRate(float debuffRate)
    {
        this.debuffRate = debuffRate;
    }

    private void OnCollisionEnter2D(Collision2D collision) // bullet layer will only collide with enemy layer, similar to facing of turret's gun
    {
        collision.gameObject.GetComponent<Health>().TakeDamage(bulletDmg); // get the collided enemy's health and deal damage
        collision.gameObject.GetComponent<EnemyMovement>().SetMoveSpeedByRate(debuffRate);

        Destroy(gameObject);
    }
}
