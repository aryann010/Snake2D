using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    public Button playButton;

    private void Awake()
    {
        playButton.onClick.AddListener(onClick);
    }
    void onClick()
    {
        SceneManager.LoadScene(1);
    }
}
