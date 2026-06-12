using UnityEngine.SceneManagement;
using UnityEngine;
using Unity.VisualScripting;

public class MainMenuManager : MonoBehaviour
{
   
    void Start()
    {
        
    }

    
    void Update()
    {
        
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void NewGame()
    {
        SceneManager.LoadSceneAsync(1);
    }
}
