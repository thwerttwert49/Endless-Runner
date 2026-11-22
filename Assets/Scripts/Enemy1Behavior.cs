using UnityEngine;

public class Enemy1Behavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D enemy_rb;
    private BoxCollider2D enemy_collider;
    private GameObject player;
    private bool grounded = false;
    private float movementSpeed = 15;
    private float jump_power = 7;
    private float health;
    void Start()
    {
         player = GameObject.Find("Player");
         enemy_rb = GetComponent<Rigidbody2D>();
         enemy_collider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        enemy_state();
        moveEnemyLateral();
        try_jump();
    }
    private void moveEnemyLateral()
    {

        float directionHorizontal;
        if (player.transform.position.x < transform.position.x) {
            directionHorizontal = -1;
        } else {
            directionHorizontal = 1;
        }
        if (grounded)
        {
            enemy_rb.linearVelocity = new Vector2(enemy_rb.linearVelocity.x + directionHorizontal * movementSpeed * Time.deltaTime, enemy_rb.linearVelocityY);
        }
        else
        {
            enemy_rb.linearVelocity = new Vector2(enemy_rb.linearVelocity.x + directionHorizontal * movementSpeed * 0.2f * Time.deltaTime, enemy_rb.linearVelocityY);
        }
        //if (grounded)
        //{
        flip_enemy(directionHorizontal);
        //}
    }
    private void try_jump() {
        if (grounded == true && player.transform.position.y - 1 > transform.position.y) {
            enemy_rb.linearVelocity = new Vector2(enemy_rb.linearVelocity.x, jump_power);
        }
    }
    private void flip_enemy(float move_x)
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
    private void enemy_state() {
        RaycastHit2D result = Physics2D.BoxCast(
            transform.position,
            enemy_collider.size + new Vector2(0.4f, 0), //radius od player
            0,
            new Vector2(0, -1),
            0.4f,
            ~LayerMask.GetMask("Enemy1")
        );
        if (result.collider != null) {
            if (enemy_rb.linearVelocity.y <= 0f)
            {
                //Debug.Log("ground");
                grounded = true;
                //can_jump = true;
            }
        } else {
            //Debug.Log("air");
            grounded = false;
            //can_jump = false;
        }
    }
    public void take_damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(this.gameObject);
        }
        Debug.Log("enemy damage taken");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Hit: " + collision.gameObject.name);
        if (collision.gameObject.CompareTag("Player")) {
             PlayerBehavior player_behavior = collision.gameObject.GetComponent<PlayerBehavior>();
             player_behavior.take_damage(1f);
        }
    }
}
