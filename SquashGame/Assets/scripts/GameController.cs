using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    private int score;
    private bool gameOver;
    public TextMesh scoreText;
    public TextMesh gameStatusText;
    public GameObject controller;
    private RacketController racketController;
    private int gameOverFrames;

    // Use this for initialization
    void Start () {
        gameOverFrames = 0;
        gameOver = false;
        gameStatusText.gameObject.SetActive(false);
        ResetScore();

        //Initialize reference to RacketController.
        racketController = controller.GetComponent<RacketController>();
    }

    private void ResetScore()
    {
        score = 0;
        scoreText.text = score.ToString();
    }

    public void Reset()
    {
        Debug.Log("GC Reset");

        gameOver = false;
        gameStatusText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(true);

        //Reset score
        ResetScore();
    }

    public void RestartGame()
    {
        //Reset the player.
        this.BroadcastMessage("Reset");
    }

    public void AddScore(int value)
    {
        score += value;
        scoreText.text = score.ToString();
    }

    public void InitGameOver()
    {   //This function must not be named "GameOver", otherwise there will be self-recursion.
        gameOver = true;
        gameOverFrames = 0;

        racketController.GameOver();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver)
        {
            gameStatusText.text = "GAME OVER";

            if (gameOverFrames % 30 == 0)
            {
                switch (gameOverFrames % (11 * 30) / 30)
                {
                    case 0:
                    case 1:
                    case 3:
                    case 4:
                        gameStatusText.gameObject.SetActive(true);
                        scoreText.gameObject.SetActive(false);
                        break;
                    case 2:
                    case 7:
                    case 10:
                        gameStatusText.gameObject.SetActive(false);
                        scoreText.gameObject.SetActive(false);
                        break;
                    case 5:
                    case 6:
                    case 8:
                    case 9:
                        gameStatusText.gameObject.SetActive(false);
                        scoreText.gameObject.SetActive(true);
                        break;
                }
            }
            gameOverFrames++;

            if (racketController.IsControllerSwung())
            {   //Change control set.
                RestartGame();
            }
        }
    }
}
