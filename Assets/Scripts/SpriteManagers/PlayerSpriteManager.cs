using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpriteManager : MonoBehaviour
{
    public SpriteRenderer rend;
    public BoxCollider2D playerBox;
    public float yPos;
    public GameObject[] props;
    public List<float> propPositions = new List<float>();
    public List<SpriteRenderer> propSprites = new List<SpriteRenderer>();

    // Start is called before the first frame update
    void Start()
    {
        playerBox = GameObject.FindGameObjectWithTag("Feet").GetComponent<BoxCollider2D>();
        rend = GetComponentInParent<SpriteRenderer>();
    }

    void Update()
    {
        BoxCollider2D feet = GameObject.FindGameObjectWithTag("Feet").GetComponent<BoxCollider2D>();
        yPos = feet.bounds.center.y;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        PropSpriteManager prop = other.GetComponent<PropSpriteManager>();
        if (prop)
        {
            Color color = prop.rend.color;

            if (yPos < prop.yPos)
            {
                if (rend.sortingOrder <= prop.rend.sortingOrder)
                {
                    rend.sortingOrder = prop.rend.sortingOrder + 1; 
                }
                prop.state = AlphaState.AlphaUp;
            }

            else if (yPos >= prop.yPos)
            {
                if (rend.sortingOrder >= prop.rend.sortingOrder)
                {
                    rend.sortingOrder = prop.rend.sortingOrder - 1; 
                }
                prop.state = AlphaState.AlphaDown;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        PropSpriteManager prop = other.GetComponent<PropSpriteManager>();
        if (prop)
        {
            if (prop.state != AlphaState.AlphaUp)
            {
                prop.state = AlphaState.AlphaUp;
            }
        }
    }
}
