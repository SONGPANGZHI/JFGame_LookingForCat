using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager2D : MonoBehaviour
{
    public static GameManager2D Instance;

    [Header("游戏设置")]
    public int totalRounds = 5;
    public float roundTime = 30f;
    public int perfectScore = 100;
    public int goodScore = 50;

    [Header("UI引用")]
    public TMP_Text scoreText;
    public TMP_Text timeText;
    public TMP_Text roundText;
    public TMP_Text messageText;
    public GameObject gameOverPanel;
    public TMP_Text finalScoreText;
    public Button nextRoundButton;

    [Header("游戏对象")]
    public CupController2D playerCup;
    public TargetCup2D targetCup;

    private int currentRound = 1;
    private int totalScore = 0;
    private float currentTime;
    private bool roundActive = true;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Start()
    {
        StartNewRound();
    }

    void Update()
    {
        if (roundActive)
        {
            currentTime -= Time.deltaTime;
            UpdateUI();

            if (currentTime <= 0)
            {
                EndRound(false);
            }
        }
    }

    public void StartNewRound()
    {
        currentTime = roundTime;
        roundActive = true;
        playerCup.ResetCup();
        targetCup.ResetTarget();
        nextRoundButton.gameObject.SetActive(false);
        messageText.text = "第 " + currentRound + " 轮开始！";

        StartCoroutine(HideMessageAfterSeconds(2f));
    }

    public void PerfectPour()
    {
        if (roundActive)
        {
            AddScore(perfectScore);
            messageText.text = "完美！+" + perfectScore + "分";
            EndRound(true);
        }
    }

    void EndRound(bool success)
    {
        roundActive = false;

        if (success)
        {
            messageText.text = "成功完成第 " + currentRound + " 轮！";
        }
        else
        {
            messageText.text = "时间到！第 " + currentRound + " 轮结束";

            // 根据准确度给分
            float accuracy = 1f - Mathf.Abs(targetCup.GetWaterPercentage() - 1f);
            if (accuracy > 0.8f)
            {
                AddScore(Mathf.RoundToInt(goodScore * accuracy));
                messageText.text += "\n不错！+" + Mathf.RoundToInt(goodScore * accuracy) + "分";
            }
        }

        if (currentRound >= totalRounds)
        {
            ShowGameOver();
        }
        else
        {
            nextRoundButton.gameObject.SetActive(true);
        }
    }

    public void NextRound()
    {
        currentRound++;
        StartNewRound();
    }

    void AddScore(int points)
    {
        totalScore += points;
        UpdateUI();
    }

    void UpdateUI()
    {
        scoreText.text = "总分: " + totalScore;
        timeText.text = "时间: " + Mathf.CeilToInt(currentTime);
        roundText.text = "轮次: " + currentRound + "/" + totalRounds;
    }

    void ShowGameOver()
    {
        gameOverPanel.SetActive(true);
        finalScoreText.text = "游戏结束！\n最终分数: " + totalScore;
    }

    public void RestartGame()
    {
        currentRound = 1;
        totalScore = 0;
        gameOverPanel.SetActive(false);
        StartNewRound();
    }

    IEnumerator HideMessageAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        messageText.text = "";
    }
}
