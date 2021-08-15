using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelGrid:MonoBehaviour
{
    private Vector2Int foodGridPosition;
    GameObject foodGameObject;
    private int width, height;
    private snakeController snake;
    float timer = 0f;


    public LevelGrid(int width,int height)
    {
        this.width = width;
        this.height = height;
        
    }

    public void setup(snakeController snake)
    {
        this.snake = snake;
        spawnMassGainer();
        
    }

    private void spawnMassGainer()
    {
        do { foodGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height)); } while (snake.snakeFullGridPositionList().IndexOf(foodGridPosition)!=-1);

        foodGameObject = new GameObject("food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.massGainer;
        foodGameObject.transform.position = new Vector2(foodGridPosition.x, foodGridPosition.y);
    }
    public bool didSnakeEat(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGridPosition)
        {
            Destroy(foodGameObject);
            
            spawnMassGainer();
            return true;
        }
        else
            return false;
    }

   public Vector2Int validateGridPosition(Vector2Int gridPosition)
    {
        if(gridPosition.x<0)
        {
            gridPosition.x = width - 1;
        }
        if(gridPosition.y<0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.x > width-1)
        {
            gridPosition.x =0;
        }
        if (gridPosition.y > height-1)
        {
            gridPosition.y = 0;
        }

        return gridPosition;
    }

}
