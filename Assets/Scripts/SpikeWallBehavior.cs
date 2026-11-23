using UnityEngine;

public class SpikeWallBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Debug.Log("Hit: " + collision.gameObject.name);
         if (collision.gameObject.CompareTag("Player")) {
             PlayerBehavior player_behavior = collision.gameObject.GetComponent<PlayerBehavior>();
             player_behavior.take_damage(3f);
        }
    }
}
