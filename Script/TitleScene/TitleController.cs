using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleController : MonoBehaviour
{

    private void Awake()
    {

        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;

    }

    // Update is called once per frame

    public void GoToStartScene()
    {

        SceneManager.LoadScene(1);

    }


    public void ExitGame()
    {

        Application.Quit();

    }
    public void BackToTitle()
    {
        Debug.Log("Pressed");
        SceneManager.LoadScene(0);
    }

    public void Test()
    {
        Debug.Log("");
    }

}
