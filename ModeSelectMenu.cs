using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ModeSelectMenu : MonoBehaviour
{
    public void NormalSelect()
    {
        SceneManager.LoadScene("SampleScene");
    }
    public void GoBack()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
