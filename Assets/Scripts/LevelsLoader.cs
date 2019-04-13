using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsLoader : MonoBehaviour
{
    public Texture2D cursorDef;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        Cursor.SetCursor(cursorDef, Vector2.zero, CursorMode.Auto);
    }

    // Play the game scene.
    public void LoadLevels()
    {
        SceneManager.LoadScene("Levels");
    }
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void LoadLevel1Vitaliy()
    {
        SceneManager.LoadScene("Level1Vitaliy");
    }
    public void LoadLevel(int i)
    {
        SceneManager.LoadScene("Level" + i.ToString());
    }

    // Quit the game application. 
    public void QuitGame()
    {
        Debug.Log("Quit!");
        Application.Quit();
    }
}
