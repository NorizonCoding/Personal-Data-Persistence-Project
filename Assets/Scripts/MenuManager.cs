using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField inputField;

    public TextMeshProUGUI highScoreTMP;

    void Awake()
    {
        if (SessionManager.instance == null) StartCoroutine(LaunchDelay());
        else UpdateHighScore();
    }

    IEnumerator LaunchDelay()
    {
        yield return new WaitForSeconds(0.1f);
        UpdateHighScore();
    }

    public void StartButton()
    {

        SessionManager.instance.curPlayerName = inputField.text;
        if (SessionManager.instance.playerScores.Count > 0)
        {
            SessionManager.instance.highestPlayer = SessionManager.instance.playerScores[0];
        }
        
        SceneManager.LoadScene(1);
    }

    // Updates the score of the high score display
    // int calledFromStartup: determines whether the function call came from startup (the awake method) or from going back to the main menu
    void UpdateHighScore()
    {
        if (SessionManager.instance != null)
        { 
            highScoreTMP.text = SessionManager.instance.ListToStr();
        }
       // Code to handle the display of the high score
        // Order depends on the score of the player
        // If a player with the same name plays twice their highest score gets displayed
        // A Dictionary will be used with Name/Score being the key/value 
    }

    public void Exit()
    {
        SessionManager.instance.Exit();
    }
}
