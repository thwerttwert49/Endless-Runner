using UnityEngine;

public class Level : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private GameObject main_camera;
    private GameObject player;
    private GameObject spike_wall;
    [SerializeField] private GameObject block_prefab;
    private float gen_distance = 0f;
    void Start()
    {
        main_camera = GameObject.Find("Main Camera");
        player = GameObject.Find("Player");
        spike_wall = GameObject.Find("SpikeWall");
    }

    // Update is called once per frame
    void Update()
    {
        //main_camera.transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
        main_camera.transform.position = new Vector3(player.transform.position.x, 0f, -10f);
        spike_wall.transform.position += new Vector3(1f * Time.deltaTime, 0f, 0f);
        if (spike_wall.transform.position.x < main_camera.transform.position.x - 14f) {
            spike_wall.transform.position = new Vector3(main_camera.transform.position.x - 14f, 0f, -5f);
        }
        if (gen_distance - player.transform.position.x < 20f) {
            gen_distance += 20f;
            generate();
        }
    }
    private void generate() {
        //new ceiling and floor
        GameObject floor = Instantiate(block_prefab, new Vector3(gen_distance, -4.5f, 0f) , main_camera.transform.rotation);
        floor.transform.localScale = new Vector2(40f, 1f);
        GameObject ceiling = Instantiate(block_prefab, new Vector3(gen_distance, 4.5f, 0f) , main_camera.transform.rotation);
        ceiling.transform.localScale = new Vector2(40f, 1f);

        int count = Random.Range(0, 5);
        for (int i = 0; i < count; ++i) {
            GameObject block = Instantiate(block_prefab, new Vector3(gen_distance + Random.Range(-20, 20), Random.Range(-4, 4), 0f) , main_camera.transform.rotation);
            block.transform.localScale = new Vector2(Random.Range(-15, 15), Random.Range(-3, 3));
        }
        //destroy old objects
        GameObject[] objects = FindObjectsByType<GameObject>(
            FindObjectsSortMode.None
        );
        foreach (GameObject obj in objects) {
            if (obj.transform.position.x < gen_distance - 100 && obj != this.gameObject) {
                Destroy(obj);
            }
        }
    }
}
