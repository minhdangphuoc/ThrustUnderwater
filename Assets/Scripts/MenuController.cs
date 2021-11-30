using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void play()
    {
        SceneManager.LoadScene("TestScene");
    }

    public void quit()
    {
        Application.Quit();
    }
}
