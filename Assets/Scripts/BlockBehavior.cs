using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float health;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    public void take_damage(float damage) {
        health -= damage;
        if (health <= 0) {
            Destroy(this.gameObject);
        }
        Debug.Log("enemy damage taken");
    }
}
