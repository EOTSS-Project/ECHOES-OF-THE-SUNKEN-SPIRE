
using UnityEngine;
using TMPro;

public class Round : MonoBehaviour
{
    // how long a round lasts, and the two HUD texts
    [SerializeField] private float roundLength = 300f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    private int score;
    private float timeLeft;

    void Start()
    {
        timeLeft = roundLength;
        ShowHud();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();
    }

    public void AddPoints(int points)
    {
        score += points;
    }

    void ShowHud()
    {
        scoreText.text = "Puan " + score;
        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}
