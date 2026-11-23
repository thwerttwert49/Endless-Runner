//=========================================
//Name: Elijah Wright
//Date: 9/10/2026
//Desc: Player controller
//Atch: Player
//=========================================

// 3 pieces of art 1:30
// 3 obstacles 3:00
// gun 4:30
// 3 attribute collectible 5:30
// score 6:00
// score collectible 6:30
// display score 7:00
// save up to 5 high scores 7:30
// can restart level after loss 8:00
// fix player movement 9:00
// other + polish 10:00
// ========================================
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private Rigidbody2D player_rb;
    private BoxCollider2D player_collider;
    [SerializeField] private float movementSpeed;
    [SerializeField] private float jump_power;
    private float max_health = 3f;
    private float health;
    [SerializeField] private int max_air_jumps;
    private int air_jumps;
    private bool can_jump = false;
    private bool grounded = false;
    private float last_a_press = 0f;
    private float last_d_press = 0f;
    private float last_dash = 0f;
    private float dash_cooldown = 0.4f;
    private bool can_dash = true;
    private float invincible_until = 0f;
    private float score = 0f;
    //bullet
    [SerializeField] private GameObject bullet_prefab;
    private float shoot_cooldown = 0.5f;
    private float shoot_cooldown_until = 0f;
    private float bullet_upgrade_until = 0f;
    private float shoot_upgrade_until = 0f;
    //
    void Start()
    {
        air_jumps = max_air_jumps;
        player_rb = GetComponent<Rigidbody2D>();
        player_collider = GetComponent<BoxCollider2D>();
        health = max_health;
    }

    // Update is called once per frame
    void Update()
    {
        player_state();
        movePlayerLateral();
        try_jump();
        try_dash();
        try_shoot();
        if (Time.time > invincible_until) { //changes transparency back when not invincible
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 255);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision) //pickups
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
            else if (collectible_behavior.type == "bullet_upgrade") //checks collectible type
            {
                bullet_upgrade_until = Time.time + 10f;
            }
            else if (collectible_behavior.type == "shoot_upgrade") //checks collectible type
            {
                shoot_upgrade_until = Time.time + 10f;
            }
            else if (collectible_behavior.type == "heal") //checks collectible type
            {
                health += 1;
                if (health > max_health) {
                    health = max_health;
                }
            }
            else if (collectible_behavior.type == "score") //checks collectible type
            {
                score += 50f;
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
    private void try_jump()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (grounded == true)
            {
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, jump_power);
            }
            else if (grounded == false && air_jumps > 0) {
                air_jumps -= 1;
                player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, jump_power);
            }
        }
    }
    private void try_dash()
    {
        if (can_dash)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (Time.time - last_a_press < 0.2 && Time.time - last_dash > dash_cooldown)
                {
                    player_rb.linearVelocity = new Vector2(-10, player_rb.linearVelocity.y);
                    last_dash = Time.time;
                }
                last_a_press = Time.time;
            }
            if (Input.GetKeyDown(KeyCode.D))
            {
                if (Time.time - last_d_press < 0.2 && Time.time - last_dash > dash_cooldown)
                {
                    player_rb.linearVelocity = new Vector2(10, player_rb.linearVelocity.y);
                    last_dash = Time.time;
                }
                last_d_press = Time.time;
            }
        }
    }
    private void try_fast_fall()
    {
        if (Input.GetKeyDown(KeyCode.S) && player_rb.linearVelocity.y < 0f) //fast fall
        {
            player_rb.linearVelocity = new Vector2(player_rb.linearVelocity.x, -5);
        }
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
    private void try_shoot()
    {
        if (Input.GetMouseButton(0) && Time.time > shoot_cooldown_until)
        {
            if (shoot_upgrade_until > Time.time)
            {
                shoot_cooldown_until = Time.time + shoot_cooldown * 0.25f;
            }
            else
            {
                shoot_cooldown_until = Time.time + shoot_cooldown;
            }

            Vector3 mouse_world_position = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            Vector3 shoot_direction = (mouse_world_position - transform.position).normalized;
            GameObject bullet = Instantiate(bullet_prefab, transform.position + shoot_direction * 0.5f, transform.rotation);
            BulletBehavior bullet_behavior = bullet.GetComponent<BulletBehavior>();
            Rigidbody2D bullet_rb = bullet.GetComponent<Rigidbody2D>();
            if (bullet_upgrade_until > Time.time)
            {
                bullet.transform.localScale = new Vector2(0.4f, 0.4f);
                bullet_rb.linearVelocity = shoot_direction * 50f;
                bullet_behavior.change_damage(3f);
            }
            else
            {
                bullet.transform.localScale = new Vector2(0.1f, 0.1f);
                bullet_rb.linearVelocity = shoot_direction * 35f;
            }

            //bullet_behavior.fire(shoot_direction * 5);
        }
    }
    private void player_state()
    {
        RaycastHit2D result = Physics2D.BoxCast(
            transform.position,
            player_collider.size + new Vector2(0.4f, 0), //radius od player
            0,
            new Vector2(0, -1),
            0.4f,
            ~LayerMask.GetMask("Player")
        );
        if (result.collider != null)
        {
            if (player_rb.linearVelocity.y <= 0f)
            {
                //Debug.Log("ground");
                grounded = true;
                can_jump = true;
                air_jumps = max_air_jumps;
            }
        }
        else
        {
            //Debug.Log("air");
            grounded = false;
            can_jump = false;
        }
    }
    public void take_damage(float damage)
    {
        if (Time.time > invincible_until)
        {
            health -= damage;
            Debug.Log("damage taken");
            GetComponent<SpriteRenderer>().color = new Color32(255, 255, 255, 120);
            invincible_until = Time.time + 1.5f;
            if (health <= 0) {
                Destroy(this.gameObject);
            }
        }
    }
}
