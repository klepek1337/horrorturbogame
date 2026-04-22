using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLives : MonoBehaviour
{
    private const int StartLivesCount = 3;
    private const int MinimumLivesCount = 0;
    private const string LoseSceneName = "LoseScene";

    [SerializeField] private TextMeshProUGUI livesText;

    private int currentLivesCount = StartLivesCount;
    private bool isPlayerDead = false;

    private void Start()
    {
        currentLivesCount = StartLivesCount;
        UpdateLivesText();
    }

    public void TakeDamage(int damageAmount)
    {
        if (isPlayerDead)
        {
            return;
        }

        if (damageAmount <= 0)
        {
            return;
        }

        currentLivesCount -= damageAmount;

        if (currentLivesCount < MinimumLivesCount)
        {
            currentLivesCount = MinimumLivesCount;
        }

        UpdateLivesText();

        if (currentLivesCount == MinimumLivesCount)
        {
            isPlayerDead = true;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            SceneManager.LoadScene(LoseSceneName);
        }
    }

    private void UpdateLivesText()
    {
        if (livesText == null)
        {
            return;
        }

        livesText.text = currentLivesCount.ToString();
    }
}