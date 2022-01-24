using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_Controller : MonoBehaviour
{
    public string Sandbox_Antone = "Antone_Playground";
    public string Sandbox_Devan = "Devan_Playground";

    public string GameplayLevel = "";

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadDungeonScene()
    {
         UnityEngine.SceneManagement.SceneManager.LoadScene(GameplayLevel);
    }
    public void LoadAntonSandbox()
    {
         UnityEngine.SceneManagement.SceneManager.LoadScene(Sandbox_Antone);
    }
    public void LoadDevanSandbox()
    {
         UnityEngine.SceneManagement.SceneManager.LoadScene(Sandbox_Devan);
    }

    public void ExitGame()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();
    }
}
