using UnityEngine;

public class Enemy2Behavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D enemy_rb;
    private GameObject player;
    private float move_speed = 3f;
    private float max_health;
    private float health;

    [SerializeField] private GameObject score_prefab;

    void Start()
    {
        max_health = Random.Range(1, 4);
        health = max_health;
        player = GameObject.Find("Player");
        enemy_rb = GetComponent<Rigidbody2D>();
        transform.localScale *= max_health;
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }
    private void move() {
        Vector2 move_direction = (player.transform.position - transform.position).normalized;
        enemy_rb.linearVelocity = move_direction * move_speed;
    }
    public void take_damage(float damage) {
        health -= damage;
        if (health <= 0) {
            for (int i = 0; i < max_health; ++i)
            {
                GameObject score_pickup = Instantiate(score_prefab, transform.position + new Vector3(Random.Range(-10, 11) * 0.05f, Random.Range(-10, 11) * 0.05f, 0f) , transform.rotation);
            }
            Destroy(this.gameObject);
        }
        Debug.Log("enemy damage taken");
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
         if (collision.gameObject.CompareTag("Player")) {
             PlayerBehavior player_behavior = collision.gameObject.GetComponent<PlayerBehavior>();
             player_behavior.take_damage(1f);
        }
    }
}
