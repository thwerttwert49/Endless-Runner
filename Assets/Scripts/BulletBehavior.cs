using UnityEngine;

public class BulletBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Rigidbody2D bullet_rb;
    void Start()
    {
        bullet_rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy")) {
             Enemy1Behavior enemy1_behavior = collision.gameObject.GetComponent<Enemy1Behavior>();
             if (enemy1_behavior) {
                 enemy1_behavior.take_damage(1f);
                 Destroy(this.gameObject);
             }
             Enemy2Behavior enemy2_behavior = collision.gameObject.GetComponent<Enemy2Behavior>();
             if (enemy2_behavior) {
                 enemy2_behavior.take_damage(1f);
                 Destroy(this.gameObject);
             }
             BlockBehavior block_behavior = collision.gameObject.GetComponent<BlockBehavior>();
             if (block_behavior) {
                 block_behavior.take_damage(1f);
                 Destroy(this.gameObject);
             }
        }
    }
}
