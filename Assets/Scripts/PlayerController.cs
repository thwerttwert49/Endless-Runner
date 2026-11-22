//=========================================
//Name: Elijah Wright
//Date: 9/10/2026
//Desc: Player controller
//Atch: Player
//=========================================
// fix player grounded detection
// fix player movement
// 3 pieces of art
// 3 obstacles
// 3 attribute collectible
// score collectible
// display score
// save up to 5 high scores
// can restart level after loss
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D player_rb;
    private BoxCollider2D player_collider;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jump_power;
    [SerializeField] private float max_health;
    private float health;
    [SerializeField] private int max_air_jumps;
    private int air_jumps;
    private bool can_jump = false;
    private bool grounded = false;
    private float last_a_press = 0f;
    private float last_d_press = 0f;
    private float last_dash = 0f;
    private float dash_cooldown = 0.4f;
    private bool can_dash = false;
    void Start()
    {
        air_jumps = max_air_jumps;
        player_rb = GetComponent<Rigidbody2D>();
        player_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        player_state();
        movePlayerLateral();
        if (Input.GetKeyDown(KeyCode.Space) && can_jump)
        {
            player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, jump_power);
        }
        if (Input.GetKeyDown(KeyCode.S) && player_rb.linearVelocity.y < 0f)
        {
            player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, -5);
        }
        if (transform.position.y < -10 && player_rb.linearVelocity.y > -10) //fast fall
        {
            transform.position = new Vector2(0, 5);
            player_rb.linearVelocity = new Vector2(0, 0);
        }
        if (can_dash)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - last_a_press < 0.2 && Time.time - last_dash > dash_cooldown)
                {
                    player_rb.linearVelocity = new Vector2(-5, player_rb.linearVelocity.y);
                    last_dash = Time.time;
                }
                last_a_press = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - last_d_press < 0.2 && Time.time - last_dash > dash_cooldown)
                {
                    player_rb.linearVelocity = new Vector2(5, player_rb.linearVelocity.y);
                    last_dash = Time.time;
                }
                last_d_press = Time.time;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        string tag = collision.gameObject.tag;

        if (collision.gameObject.CompareTag("Collectible"))
        {
            Debug.Log("collect");
            CollectibleBehavior collectible_behavior = collision.gameObject.GetComponent<CollectibleBehavior>();
            if (collectible_behavior.type == "dash") //checks collectible type
            {
                can_dash = true;
            }
            Destroy(collision.gameObject);
        }
    }
    private void movePlayerLateral()
    {
        float inputHorizontal = Input.GetAxisRaw("Horizontal");
        if (grounded)
        {
            player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x + inputHorizontal * movementSpeed * Time.deltaTime, player_rb.linearVelocityY);
        }
        else
        {
            player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x + inputHorizontal * movementSpeed * 0.2f * Time.deltaTime, player_rb.linearVelocityY);
        }
        //if (grounded)
        //{
        flip_player(inputHorizontal);
        //}
    }
    private void flip_player(float move_x)
    {
        if (move_x > 0f)
        {
            transform.eulerAngles = new Vector3(0f, 0f, 0f);
        }
        else if (move_x < 0f)
        {
            transform.eulerAngles = new Vector3(0f, 180f, 0f);
        }
    }
    private void player_state() {
        RaycastHit2D result = Physics2D.BoxCast(
            transform.position,
            player_collider.size + new Vector2(0.4f, 0), //radius od player
            0,
            new Vector2(0, -1),
            0.4f,
            ~LayerMask.GetMask("Player")
        );
        if (result.collider != null) {
            if (player_rb.linearVelocity.y <= 0f)
            {
                //Debug.Log("ground");
                grounded = true;
                can_jump = true;
            }
        } else {
            //Debug.Log("air");
            grounded = false;
            can_jump = false;
        }
    }
}
