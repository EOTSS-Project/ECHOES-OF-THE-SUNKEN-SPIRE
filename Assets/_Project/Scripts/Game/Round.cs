using UnityEngine;
using TMPro;

public class Round : MonoBehaviour
{
    // how long a round lasts, and the two HUD texts
    [SerializeField] private float roundLength = 120f;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text timeText;

    // the end screen
    [SerializeField] private GameOver gameOver;

    private int score;      // points this round
    private float timeLeft; // seconds until the end
    private bool over;      // true once the round has ended

    public bool IsOver => over;

    void Start()
    {
        timeLeft = roundLength; // a full clock
        ShowHud();
    }

    void Update()
    {
        if (over) return; // ended: nothing changes any more

        // count down, but never below zero
        timeLeft -= Time.deltaTime;
        if (timeLeft < 0f) timeLeft = 0f;
        ShowHud();

        if (timeLeft <= 0f) EndRound("Süre doldu! Kule sulara gömüldü...");
    }

    // a sanctuary deposit calls this (through GameHUD)
    public void AddPoints(int points)
    {
        if (over) return;
        score += points;
    }

    // time's up, the summit or the last death ends the round
    public void EndRound(string message)
    {
        if (over) return;
        over = true;
        Time.timeScale = 0f; // stop the world
        gameOver.Show(message, score);
    }

    // write both numbers on the screen
    void ShowHud()
    {
        scoreText.text = "Puan " + score;
        int seconds = Mathf.CeilToInt(timeLeft);
        int minutes = seconds / 60;
        timeText.text = minutes + ":" + (seconds % 60).ToString("00");
    }
}