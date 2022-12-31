using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private int health = 3;

    private SpriteRenderer[] sprites;

    [SerializeField]
    private Color defaultColor;

    [SerializeField]
    private Color damageColor;

    [SerializeField]
    private DamageDealer damageDealer;

    

    // Start is called before the first frame update
    void Start()
    {
        damageDealer.enabled = false;
        sprites = GetComponentsInChildren<SpriteRenderer>();

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            StartCoroutine(Die());
        }
    }

    private IEnumerator Die()
    {
        int numberOfFlashes = 3;
        for(int i = 0; i < numberOfFlashes; i++)
        {
            SetColor(damageColor);
            yield return new WaitForSeconds(0.05f);
            SetColor(defaultColor);
            yield return new WaitForSeconds(0.05f);
        }
        Destroy(gameObject);
        
    }

    private void SetColor(Color color)
    {
        foreach(var renderer in sprites)
        {
            renderer.color = color;
        }
    }

    public void EnableDealingDamage()
    {
        damageDealer.enabled = true;
    }

    public void DisableDealingDamage()
    {
        damageDealer.enabled = false;
    }


}
