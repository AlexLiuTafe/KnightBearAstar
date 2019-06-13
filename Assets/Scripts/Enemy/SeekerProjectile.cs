using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeekerProjectile : MonoBehaviour
{
    public float speed;
    public float projectileDamage;

    private Transform player;

    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);//Homing Projectile
            transform.up = player.position - transform.position;//LooAt() in 2D
            DestroyProjectile();
            
        }
    }


    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.GetComponent<PlayerMovement>();
        if (col.CompareTag("Player"))
        {
            player.TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        else
        {
            DestroyProjectile();
        }
    }
    void DestroyProjectile()
    {
        Destroy(gameObject, 1.8f);
    }

}
