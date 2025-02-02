using UnityEngine;
using UnityEngine.Serialization;

public class MovePlayer : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;
    [SerializeField]
    private Vector2 moveVector;
    [SerializeField]
    private float speed = 2f;

    public float jumpForse = 300f;
    public int maxJumpValeu = 2;
    [FormerlySerializedAs("jumpValueIteration")] [FormerlySerializedAs("jumpValueItatation")]
    public int maxAllowedJumpIteration = 60;

    public bool OnDown;
    public Transform DownTrig;
    public float checkRadius = 0.5f;
    public LayerMask paltformTrig;


    [Header("Debug")]
    [SerializeField]
    private int jumpCount = 0;
    [FormerlySerializedAs("jumpItaration")] [SerializeField]
    private int jumpIteration = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Walk();
        Jump();
        CheckDownTrig();
    }

    private void Walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(moveVector.x * speed, rb.velocity.y);
    }

    private void CheckDownTrig()
    {
        OnDown = Physics2D.OverlapCircle(DownTrig.position, checkRadius, paltformTrig);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (OnDown || jumpCount < maxJumpValeu))
        {
            jumpCount++;
            rb.AddForce(Vector2.up * jumpForse);
        }

        if (OnDown)
        {
            jumpCount = 0;
            jumpIteration = 0;
        }

        var jumpControl = Input.GetKeyDown(KeyCode.Space) && OnDown;

        if (jumpControl && jumpIteration < maxAllowedJumpIteration)
        {
            jumpIteration++;
            rb.AddForce(Vector2.up * (jumpForse / jumpIteration));
        }
    }
}