using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MovePlayer : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Vector2 moveVector;
    [SerializeField] private float speed = 2f;
    private int ClickTime;


    public float jumpForse = 300f;
    public int maxJumpValeu = 2;
    public int maxAllowedJumpIteration = 60;

    public bool OnDown;
    public Transform DownTrig;
    public float checkRadius = 0.5f;
    public LayerMask paltformTrig;
    private int jumpIteration = 0;
    private bool faceRight = true;
    private bool lockLounge = false;
    public Transform topCheck;
    private float topCheckRadius;
    public LayerMask Roof;
    public Collider2D posseStand;
    public Collider2D posseSquad;
    private bool jumpLock = false;
    public Transform CheckLoader;
    public float checkRadiusLoaderTrig = 0.04f;
    public LayerMask laderMask;
    public bool chekedLader = false;

    private int jumpCount = 0;

    [FormerlySerializedAs("jumpItaration")] [SerializeField]
    private int lungeImpulse = 5000;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Walk();
        Reflect();
        Jump();
        CheckPlatformTrig();
        Lunge();
        SqatMove();
        ClimbLadder();
    }

    private void Walk()
    {
        moveVector.x = Input.GetAxisRaw("Horizontal");
        rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
    }


    void Reflect()
    {
        if ((moveVector.x > 0 && !faceRight) || (moveVector.x < 0 && faceRight))
        {
            transform.localScale *= new Vector2(-1, 1);
            faceRight = !faceRight;
        }
    }

    private void CheckPlatformTrig()
    {
        OnDown = Physics2D.OverlapCircle(DownTrig.position, checkRadius, paltformTrig);
    }


    private void Jump()
    {
        if (Input.GetKey(KeyCode.S))
        {
            Physics2D.IgnoreLayerCollision(7, 8, true);
            Invoke(nameof(IgnoreLaerOff), 1f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && (OnDown || jumpCount < maxJumpValeu) && !jumpLock)
        {
            jumpCount++;
            rb.AddForce(Vector2.up * jumpForse);
        }

        if (OnDown)
        {
            jumpCount = 0;
            jumpIteration = 0;
        }

        var jumpControl = Input.GetKeyDown(KeyCode.Space) && OnDown && !jumpLock;

        if (jumpControl && jumpIteration < maxAllowedJumpIteration)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
            jumpIteration++;
            rb.AddForce(Vector2.up * (jumpForse / jumpIteration));
        }
    }

    private void IgnoreLaerOff()
    {
        Physics2D.IgnoreLayerCollision(7, 8, false);
    }

    private void Lunge()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !lockLounge)
        {
            lockLounge = true;
            Invoke("LoungeLock", 1f);
            rb.linearVelocity = new Vector2(0, 0);
            if (transform.localScale.x < 0)
            {
                rb.AddForce(Vector2.left * lungeImpulse);
            }
            else
            {
                rb.AddForce(Vector2.right * lungeImpulse);
            }
        }
    }

    private void LoungeLock()
    {
        lockLounge = false;
    }

    private void SqatMove()
    {
        if (Input.GetKey(KeyCode.S))
        {
            transform.localScale = new Vector2(transform.localScale.x, 1);
            posseStand.enabled = false;
            posseSquad.enabled = true;
            jumpLock = true;
        }
        else if (!Physics2D.OverlapCircle(topCheck.position, topCheckRadius, Roof))
        {
            transform.localScale = new Vector2(transform.localScale.x, 2);
            posseStand.enabled = true;
            posseSquad.enabled = false;
            jumpLock = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        chekedLader = IsLadder(collision);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        chekedLader = IsLadder(other);
    }

    private bool IsLadder(Collider2D collision)
    {
        return collision.gameObject.layer == 9;
    }

    private void ClimbLadder()
    {
        gameObject.transform.GetComponent<Rigidbody2D>().bodyType =
            chekedLader ? RigidbodyType2D.Kinematic : RigidbodyType2D.Dynamic;
    }
}