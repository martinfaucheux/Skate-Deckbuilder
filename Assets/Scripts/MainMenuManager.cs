using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : CoduckStudio.Utils.Singleton<MainMenuManager>
{
    public CharacterController characterController1;
    public PathContainer pathContainer1;
    public Vector2 startPosition1;
    public float sequenceDuration1 = 10;

    // public CharacterController characterController2;
    // public PathContainer pathContainer2;
    // public Vector2 startPosition2;
    // public float sequenceDuration2 = 10;

    public float timer = 0;

    void Update()
    {
        timer += Time.deltaTime;

        float t1 = sequenceDuration1 > 0 ? timer / sequenceDuration1 : 0;
        PathData pathData1 = pathContainer1.Evaluate(t1);
        characterController1.Move(pathData1);
        if (timer > sequenceDuration1) {
            characterController1.transform.position = startPosition1;
            timer = 0;
        }

        // float t2 = sequenceDuration2 > 0 ? timer / sequenceDuration2 : 0;
        // PathData pathData2 = pathContainer2.Evaluate(t2);
        // characterController2.Move(pathData2);
        // if (timer > sequenceDuration2) {
        //     characterController2.transform.position = startPosition2;
        //     timer = 0;
        // }
    }

    public void OnClick_Ride()
    {
        AudioManager.instance.Stop("Roll");
        AudioManager.instance.Stop("Slide");
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
