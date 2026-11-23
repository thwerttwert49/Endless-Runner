using UnityEngine;
using UnityEngine.SceneManagement; //for loading scene
using System.Collections;
using TMPro;

public class Menu : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private float[] scores = {0f, 0f, 0f, 0f, 0f};
    private TMP_Text score_ui;
    void Start()
    {
        Scene level_scene = SceneManager.GetSceneByName("Level");
        if (level_scene.isLoaded)
        {
            SceneManager.UnloadSceneAsync("Level");
        }
        score_ui = GameObject.Find("HighScores").GetComponent<TMP_Text>();
        Debug.Log("menu");
    }

    public void play_button_clicked() { //playbutton
        //SceneManager.UnloadSceneAsync(gameObject.scene);
        Debug.Log("play");
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level");
        //SceneManager.LoadScene("Level", LoadSceneMode.Additive);
        //SceneManager.UnloadSceneAsync("Menu");
        //StartCoroutine(waitload(5));
    }
    public void exit_button_clicked() { //playbutton
        Application.Quit();
    }
    public void add_score(float new_score)
    {
        score_ui.text = "";
        for (int i = 0; i < 5; ++i)
        {
            score_ui.text += scores[i].ToString() + "\n";
        }
    }
    /*
    private IEnumerator waitload(int time) {
        yield return new WaitForSeconds(time);
        SceneManager.LoadScene("Level1");
        Debug.Log("reloaded");
    }
    */
}
