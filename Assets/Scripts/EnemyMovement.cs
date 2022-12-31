using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{

    private Transform target;

    [SerializeField]
    private float speed;

    private bool moving;

    // Start is called before the first frame update
    void Start()
    {
        target = FindObjectOfType<Player>().transform;
        StartCoroutine(MoveToTarget());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator MoveToTarget()
    {
        moving = true;
        while(moving)
        {
            var direction = (target.position - transform.position).normalized;
            transform.up = new Vector2(direction.x, direction.y);
            transform.position += direction * speed * Time.deltaTime;
            yield return new WaitForFixedUpdate();
        }

    }
}
