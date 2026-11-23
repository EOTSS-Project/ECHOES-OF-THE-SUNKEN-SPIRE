using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOver : MonoBehaviour
{
    // the big text in the middle of the panel
    [SerializeField] private TMP_Text resultText;

    // the Round calls this when the round ends
    public void Show(string message, int score)
    {
        // the best score is kept on this computer
        int best = PlayerPrefs.GetInt("echoes_best", 0);
        if (score > best) best = score;
        PlayerPrefs.SetInt("echoes_best", best);

        resultText.text = message + "\nPuan " + score + "\nEn iyi " + best;
        gameObject.SetActive(true); // the panel appears

        // unlock and show cursor so the player can click Play Again
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // the Play again button calls this
    public void Restart()
    {
        // the Round stopped time: start it again first
        Time.timeScale = 1f;
        // load the scene we are in once more: a fresh round
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}