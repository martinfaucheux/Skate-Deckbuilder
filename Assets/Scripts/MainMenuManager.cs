using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : CoduckStudio.Utils.Singleton<MainMenuManager>
{
    public void OnClick_Ride()
    {
        FindFirstObjectByType<CoduckStudio.Fade>().HideGame(false, () => {
            SceneManager.LoadScene("GameScene");
        });
    }

    public void OnClick_Settings()
    {
        
    }

    public void OnClick_Credits()
    {
        
    }

    public void OnClick_Exit()
    {
        FindFirstObjectByType<CoduckStudio.Fade>().HideGame(false, () => {
            Application.Quit();
        });
    }
}
