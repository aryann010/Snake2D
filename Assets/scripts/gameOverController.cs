using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class gameOverController : MonoBehaviour
{
    public Button playAgainButton;

    private void Awake()
    {
      
        playAgainButton.onClick.AddListener(playAgain);


    }
    
    private void playAgain()
    {
        SceneManager.LoadScene(1);

    }

}
