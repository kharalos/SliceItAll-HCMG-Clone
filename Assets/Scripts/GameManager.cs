using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sliceables;

    [SerializeField]
    private GameObject[] obstacles;

    [SerializeField]
    private GameObject finishPlatform;

    [SerializeField]
    private int levelNumber = 1;
    private int platformNumber = 0;
    private int obstacleNumber = 0;

    [SerializeField]
    private int score = 0;

    [SerializeField]
    private bool test;

    private bool gameOver = false;

    [SerializeField]
    private Text scoreUI;


    [SerializeField]
    private GameObject startPanel, gameOverPanel;
    [SerializeField]
    private Text gameoverText;
    private void Start()
    {
        Time.timeScale = 1;
        if (!test) GenerateLevel();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && startPanel.activeInHierarchy)
        {
            if(gameOver) SceneManager.LoadScene(0);
            startPanel.SetActive(false);
        }
        if (gameOver && !startPanel.activeInHierarchy) startPanel.SetActive(true);           
    }
    private void GenerateLevel()
    {
        System.Random rng = new System.Random();
        int platformLocation = 2;

        platformNumber = levelNumber * 6;
        obstacleNumber = levelNumber * 2;

        // platform listesi oluþtur
        List<string> platformOrder = new List<string>();

        //listeye engelleri ve kesilebilen objeleri koy
        for (int i = 0; i < platformNumber; i++)
        {
            platformOrder.Add("Sliceable");
        }
        for (int i = 0; i < obstacleNumber; i++)
        {
            platformOrder.Add("Obstacle");
        }

        // platformlarý karýlmýþ yeni liste
        var shuffledOrder = platformOrder.OrderBy(a => rng.Next()).ToList();

        for (int i = 0; i < shuffledOrder.Count; i++)
        {
            if (shuffledOrder[i] == "Sliceable")
            {
                Instantiate(sliceables[Random.Range(0, sliceables.Length)], new Vector3(0, 0, platformLocation), Quaternion.identity);
            }
            else
            {
                Instantiate(obstacles[Random.Range(0, obstacles.Length)], new Vector3(0, 0, platformLocation), Quaternion.identity);
            }
            platformLocation += 2;
        }
        Instantiate(finishPlatform, new Vector3(0, 0, platformLocation), Quaternion.identity);
    }

    public void AddScore()
    {
        score++;
        UpdateScore();
    }
    private void UpdateScore()
    {
        scoreUI.text = "Score: " + score;
    }

    public void Defeat()
    {
        Debug.Log("Defeated");
        Time.timeScale = 0;
        gameOverPanel.SetActive(true);
        gameoverText.text = "DEFEAT";
        gameOver = true;
    }
    public void Finish(int scoreMultiplier)
    {
        score *= scoreMultiplier;
        UpdateScore();
        Debug.Log("Won! Score: " + score);
        gameOverPanel.SetActive(true);
        gameoverText.text = "WON!";
        Time.timeScale = 0;
        gameOver = true;
    }
}