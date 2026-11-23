using UnityEngine;

public class BlockBehavior : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float max_health = 6f;
    private float health;
    [SerializeField] private Sprite sprite1;
    [SerializeField] private Sprite sprite2;
    [SerializeField] private Sprite sprite3;
    [SerializeField] private Sprite sprite4;
    void Start()
    {
        health = max_health;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void set_texture() {
        if (health > max_health * 0.9)
        {
            GetComponent<SpriteRenderer>().sprite = sprite1;
        }
        else if (health > max_health * 0.66)
        {
            GetComponent<SpriteRenderer>().sprite = sprite2;
        }
        else if (health > max_health * 0.33)
        {
            GetComponent<SpriteRenderer>().sprite = sprite3;
        }
        else {
            GetComponent<SpriteRenderer>().sprite = sprite4;
        }
    }
    public void take_damage(float damage) {
        health -= damage;
        set_texture();
        if (health <= 0) {
            Destroy(this.gameObject);
        }
        Debug.Log("brick damage taken");
    }
}
