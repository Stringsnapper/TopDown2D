using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShredder : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var projectile = collision.GetComponent<Projectile>();
        if(projectile != null)
        {
            Destroy(projectile.gameObject);
        }
    }
}
