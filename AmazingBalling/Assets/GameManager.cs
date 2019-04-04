using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onReset;

    public static GameManager instance;
    public GameObject readyPanel;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;

    public bool isRoundActive = false;

    private int score = 0;

    public ShooterRotater shooterRotater;
    public CamFollow cam;

    private void Awake()
    {
        instance = this;
        UpdateUI();
    }
    private void Start()
    {
        StartCoroutine("RoundRoutine");
    }
    public void AddScore(int newScore)
    {
        score += newScore;
        UpdateBestScore();
        UpdateUI();
    }
    void UpdateBestScore()
    {
        if(GetBestScore() < score)
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }
    int GetBestScore()
    {
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;
    }
    void UpdateUI()
    {
        scoreText.text = "Score: " + score;
        bestScoreText.text = "BestScore: " + GetBestScore();
    }
    public void OnBallDestroy()
    {
        UpdateUI();
        isRoundActive = false;
    }
    public void Reset()
    {
        score = 0;
        UpdateUI();

        StartCoroutine("RoundRoutine");
    }
    IEnumerator RoundRoutine()
    {
        //Read
        onReset.Invoke();

        readyPanel.SetActive(true);
        cam.SetTarget(shooterRotater.transform,CamFollow.State.Idle);
        shooterRotater.enabled = false;

        isRoundActive = false;

        messageText.text = "Ready...";

        yield return new WaitForSeconds(3f);

        isRoundActive = true;
        readyPanel.SetActive(false);
        shooterRotater.enabled = true;

        cam.SetTarget(shooterRotater.transform,CamFollow.State.Ready);

        while(isRoundActive == true)
        {
            yield return null;
        }

        readyPanel.SetActive(true);
        shooterRotater.enabled = false;

        messageText.text = "Wait For Next Score";

        yield return new WaitForSeconds(3f);
        Reset();
    }
}
