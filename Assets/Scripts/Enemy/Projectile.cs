using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed;
    public float projectileDamage;

    private Transform player;
    private Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        //player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        target = new Vector2(player.position.x, player.position.y);//Checking the player last position!

    }

    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            //transform.position = Vector2.MoveTowards(transform.position, player.position, speed * Time.deltaTime);//Homing Projectile
            transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);
            //Hit the target last stored position
            if (transform.position.x == target.x && transform.position.y == target.y)
            {
                
                DestroyProjectile();
                
            }
            
        }
        



    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerMovement player = col.GetComponent<PlayerMovement>();
        if (col.CompareTag("Player"))
        {
            player.TakeDamage(projectileDamage);
            DestroyProjectile();
        }
        else
        {
            return;
        }
    }
    void DestroyProjectile()
    {
        Destroy(gameObject);
    }

}
