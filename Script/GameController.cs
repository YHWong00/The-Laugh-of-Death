using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameController : MonoBehaviour
{
    public static int day = 0;
    public static int score = 0;
    public TextMeshProUGUI txtScore;
    public static GameController instance;
    public GameObject map;
    public GameObject building;
    public List<Sprite> roofs;
    public List<GameObject> npcs;
    public enum Direction { N, T, B, L, R };
    public Direction lastEntranceDir;

    public GameObject player;

    public AudioSource gameControllerAS;
    public AudioClip nightBgm;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }
    public List<int> GenerateDistinctNums(int count, int max)
    {
        List<int> numbers = new List<int>();
        while (numbers.Count < count)
        {
            int number = Random.Range(0, max);
            if (!numbers.Contains(number))
            {
                numbers.Add(number);
            }
        }
        return numbers;
    }
    public void AddScore()
    {
        score++;
        txtScore.text = score.ToString();
    }
    public void BackToTitle()
    {
        SceneManager.LoadScene(0);
    }
    public void StartGame()//开始游戏
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(2);
        Invoke("GetScore", 0.05f);
    }

    public void EndGame()
    {
        //todo 结算界面
        day = GameObject.Find("GlobalVolume").GetComponent<TimeController>().days;
        SceneManager.LoadScene(3); 
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    private void GetScore()
    {
        txtScore = GameObject.Find("score").GetComponent<TextMeshProUGUI>();
        txtScore.text = score.ToString();
    }
}
