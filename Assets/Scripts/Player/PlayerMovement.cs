using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Player Health")]
    public float playerHealth;
    public float playerMaxHealth;
    public Slider playerHealthBar;

    [Header("Movement Variables")]
    public float baseSpeed = 5f;
    public float moveSpeed;
    public static Vector2 motion;
    public static Vector2 direction = new Vector2(0, -1);
    public static bool CanMove = true;

    [Header("Dodge Variables")]
    public float dodgeSpeed;
    public float maxDodgeTime, preDodgeDelay, afterDodgeDelay;
    private float dodgeTimer;
    public static bool isDodging;
    private Vector2 dodgeDirection;

    private Animator anim;
    private SpriteRenderer rend;
    public static Rigidbody2D rb;

    void Start()
    {
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        playerHealthBar = gameObject.transform.GetComponentInChildren<Slider>();

        dodgeTimer = maxDodgeTime;
    }

    void Update()
    {
        float inputH = Input.GetAxisRaw("Horizontal");
        float inputV = Input.GetAxisRaw("Vertical");

        if (CanMove)
        {
            Move(inputH, inputV);

            FaceDirection(inputH, inputV);

            // Run Dodge() which will dodge if Left Shift is pressed
            Dodge();

            //Move using Rigidbody 2D
            rb.MovePosition(rb.position + motion * Time.deltaTime);
        }
        playerHealthBar.value = Mathf.Clamp01(playerHealth / playerMaxHealth); //Clamp Health
    }

    void Move(float inputH, float inputV)
    {
        // Multiply Motion by Move Speed
        motion.x = inputH * moveSpeed;
        motion.y = inputV * moveSpeed;
        // Set our animator float Motion
        anim.SetFloat("Motion", motion.magnitude);
    }

    void Dodge()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            // Store the direction we were facing when the dodge started
            dodgeDirection = direction;
            // Delay movement by the pre-dodge penalty
            StartCoroutine(PreDodgeDelay(preDodgeDelay));
            // Set our dash timer to 0
            dodgeTimer = 0;
        }

        // If Dodge is active, i.e. our dash timer is on and counting up
        if (dodgeTimer < maxDodgeTime)
        {
            direction = dodgeDirection;
            // Motion becomes our last faced direction multiplied by our dash speed
            motion = direction * dodgeSpeed;
            // Count up the dash timer by Time.deltaTime
            dodgeTimer += Time.deltaTime;
            //Set IsDodging to true
            isDodging = true;
        }

        // Else if our dodge time is up and we're still flagged as dodging
        else if (dodgeTimer > maxDodgeTime && isDodging)
        {
            // Stop moving
            motion = Vector2.zero;
            // Delay movement by post-dodge penalty
            StartCoroutine(AfterDodgeDelay(afterDodgeDelay));
            // Set isDodging to false
            isDodging = false;
        }
    }

    void FaceDirection(float inputH, float inputV)
    {
        // If we are currently sensing any input
        if ((inputH != 0 || inputV != 0) && !isDodging)
        {
            // Set our Direction variable as Motion (i.e. the last direction we travelled in based on inputs) normalised into -1, 0 or 1
            direction = motion.normalized;
        }

        // As long as our x and y direction aren't both 0
        if (!(direction.x == 0 && direction.y == 0))
        {
            // Set our animator floats for x and y
            anim.SetFloat("Horizontal", direction.x);
            anim.SetFloat("Vertical", direction.y);

            // Face left or right depending on which way we x direction we last input
            rend.flipX = direction.x < 0;
        }
    }
    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    #region Enumerators
    IEnumerator PreDodgeDelay(float delay)
    {
        CanMove = false;
        motion = Vector2.zero;
        anim.SetTrigger("DodgeRoll");
        yield return new WaitForSeconds(delay);
        CanMove = true;
    }

    IEnumerator AfterDodgeDelay(float delay)
    {
        CanMove = false;
        direction = dodgeDirection;
        yield return new WaitForSeconds(delay);
        CanMove = true;
    }
    #endregion
}