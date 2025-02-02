using UnityEngine;

public class MovePlayer : MonoBehaviour
{

    public Rigidbody2D rb;
    public Vector2 moveVector;
    public float speed = 2f;

    void Start()
    {
        rb =GetComponent <Rigidbody2D>();
    }

  
    void Update()
    {
        walk();
        Jump();
        ChekDownTrig();
    }


    void walk()
    {
        moveVector.x = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveVector.x * speed, rb.linearVelocity.y);
    }

    public float jumpForse = 300f;
    private int jumpCount = 0;
    public int maxJumpValeu = 2;
    private bool jumpControl;
    private int jumpItaration = 0;
    public int jumpValueItatation = 60;


    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && (OnDown || ++jumpCout < maxJumpValeu))
        {
            rb.AddForce(Vector2.up * jumpForse);
        }

        if (OnDown) {jumpCount = 0;}
        if(Input.GetKeyDown(KeyCode.Space))
        {
            if(OnDown) {jumpControl =true;}
        }
        else {jumpControl = false;}

        if (jumpControl)
        {
            if(jumpItaration++ < jumpValueItatation )
            {
                rb.AddForce(Vector2.up * jumpForse /jumpItaration );
            }
            else {jumpItaration = 0;}
        }

    }



    public bool OnDown;
    public Transform DownTrig;
    public float checkRadius = 0.5f;
    public LayerMask paltformTrig;
    private int jumpCout;

    void ChekDownTrig()
    {
        OnDown = Physics2D.OverlapCircle(DownTrig.position, checkRadius, paltformTrig);
    }
}
