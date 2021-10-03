using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float sensitivity = 1f;
    public int count = 0;
    public bool explorationMode = false;
    // Start is called before the first frame update
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            FindObjectOfType<Menu>().IntroText();
        }
        else Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    public void CountUp()
    {
        count++;
    }
    public void Lose()
    {
        FindObjectOfType<Menu>().IntroText(true);
        count = 0;
    }
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }
}
