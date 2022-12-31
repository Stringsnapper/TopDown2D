using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float speed = 10;

    private int damage = 3;
    private void FixedUpdate()
    {
        transform.position += transform.up.normalized * speed * Time.deltaTime;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Enemy enemy = collision.gameObject.GetComponentInParent<Enemy>();
        Explode();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }
    }

    private void Explode()
    {
        // TODO: Trigger explosion simulation
        Destroy(gameObject);
    }

}
