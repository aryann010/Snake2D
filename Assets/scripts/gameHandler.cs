using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHandler : MonoBehaviour
{
    [SerializeField] private snakeController snake;
    private LevelGrid levelGrid;
    // Start is called before the first frame update
    void Start()
    {
     
        levelGrid = new LevelGrid(20, 20);
        snake.setup(levelGrid);
        levelGrid.setup(snake);
    }
    
    
        
    

    
}
