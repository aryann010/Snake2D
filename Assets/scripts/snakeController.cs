using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class snakeController : MonoBehaviour
{
    private Vector2Int moveDirection;
    private Vector2Int gridPosition;
    private float timerMax;
    private float timer;
    private LevelGrid levelGrid;
    private int bodySize;
    private List<snakeMovePosition> snakeMovePositionList;
    private List<snakeBodyPart> snakeBodyPartList;
    GameObject bodyGameObject;

   


    public void setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        timerMax = 0.5f;
        timer = timerMax;
        moveDirection = new Vector2Int(0, 1);
        snakeMovePositionList = new List<snakeMovePosition>();
        bodySize = 0;
        snakeBodyPartList = new List<snakeBodyPart>();
    }
   

    private void Update()
    {
        
        var rotate = transform.rotation.eulerAngles;
        


        if (Input.GetKeyDown(KeyCode.W))
        {
            if (moveDirection.y != -1)
            {
                moveDirection.x = 0;
                moveDirection.y = 1;
                
                rotate.z = 0;
                
            }
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (moveDirection.x != 1)
            {
                moveDirection.x = -1;
                moveDirection.y = 0;
               
                rotate.z = 90;
                

            }
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            if (moveDirection.y != 1)
            {
                moveDirection.x = 0;
                moveDirection.y = -1;
               
                rotate.z = 180;

            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (moveDirection.x != -1)
            {
                moveDirection.x = 1;
                moveDirection.y = 0;
               
                rotate.z = 270;
                 
            }
        }
        
        transform.rotation = Quaternion.Euler(rotate);

        timer += Time.deltaTime;
        if(timer>=timerMax)
        {
            timer -= timerMax;
            snakeMovePosition snakeMovePosition = new snakeMovePosition(gridPosition,moveDirection);
             snakeMovePositionList.Insert(0, snakeMovePosition);

            gridPosition += moveDirection;
            gridPosition = levelGrid.validateGridPosition(gridPosition);
         
            bool snakeAteFood = levelGrid.didSnakeEat(gridPosition);
            if (snakeAteFood)
            {
                bodySize++;
                createSnakeBody();
            }


            if (snakeMovePositionList.Count>=bodySize+1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
                
            }

           /* foreach (snakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
               Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition)
                {
                    Debug.Log("dead");
               }
            }*/

            transform.position = new Vector2(gridPosition.x, gridPosition.y);
            for(int i=0; i<snakeBodyPartList.Count;i++)
            {
               // Vector3 snakeBodyPosition = new Vector3(snakeMovePositionList[i].x, snakeMovePositionList[i].y);
                snakeBodyPartList[i].setSnakeMovePosition(snakeMovePositionList[i]);
            }
        }
        
       
    }
    private void createSnakeBody()
    {
        
        snakeBodyPartList.Add(new snakeBodyPart(snakeBodyPartList.Count));
    }
  
    public Vector2Int getGridPosition()
    {
        return gridPosition;
    }
    public List<Vector2Int> snakeFullGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        foreach(snakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.getGridPosition());
        }
        
        return gridPositionList;
    }

   private class snakeBodyPart
    {
        private snakeMovePosition snakeMovePosition;
        private Transform transform;

        public snakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("snakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.body;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -1-bodyIndex;
            transform = snakeBodyGameObject.transform;
        }
        public void setSnakeMovePosition(snakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.getGridPosition().x, snakeMovePosition.getGridPosition().y);
            
        }
        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.getGridPosition();
        }
    }
    private class snakeMovePosition
    {
        private Vector2Int gridPosition;
        private Vector2Int direction;

        public snakeMovePosition(Vector2Int gridPosition,Vector2Int direction )
        {
            this.gridPosition = gridPosition;
            this.direction = direction;
        }
        public Vector2Int getGridPosition()
        {
            return gridPosition;
        }
        public Vector2Int getDirection()
        {
            return direction;
        }
    }

}
