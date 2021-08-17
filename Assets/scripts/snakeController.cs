using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class snakeController : MonoBehaviour
{
    public enum direction
    {
        Up,
        Down,
        Left,
        Right
    }

    private bool alive;
    private bool shieldActive;
    private direction gridMoveDirection;
    private Vector2Int gridPosition;
    private Vector3 snakeHeadRotation;
    public int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;
    private float sheildTimer;
    public float sheildTimerMax;
    public float gridMoveTimermax;
    private float gridMoveTimer;
    private levelGrid levelGrid;
    public scoreController sc;
    public gameOverController goc;


    public void setup(levelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        gridMoveDirection = direction.Up;
        gridMoveTimermax = 0.1f;
        gridMoveTimer = gridMoveTimermax;
        snakeHeadRotation = new Vector3(0, 0, 0);
        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
        alive = true;
        shieldActive = false;
        sheildTimerMax = 10f;
        sheildTimer = sheildTimerMax;
    }

    private void Update()
    {
        if (alive)
        {
            inputHandler();
            handleGridMovement();
        }

    }

    public void inputHandler()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            if (gridMoveDirection != direction.Down)
            {
                gridMoveDirection = direction.Up;
                snakeHeadRotation.Set(0, 0, 0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            if (gridMoveDirection != direction.Right)
            {
                gridMoveDirection = direction.Left;
                snakeHeadRotation.Set(0, 0, 90);
            }

        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            if (gridMoveDirection != direction.Up)
            {
                gridMoveDirection = direction.Down;
                snakeHeadRotation.Set(0, 0, 180);
            }

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            if (gridMoveDirection != direction.Left)
            {
                gridMoveDirection = direction.Right;
                snakeHeadRotation.Set(0, 0, -90);
            }

        }

    }

    public void handleGridMovement()
    {
        sheildTimer -= Time.deltaTime;

        if (sheildTimer <= 0)
        {
            sheildTimer = sheildTimerMax;
            destroyPowerUps();
            SpawnSheild();
        }

        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimermax)
        {
            gridMoveTimer -= gridMoveTimermax;

            updateSnakeMovePositions();
            updateGridPosition();
            hasEaten();
            updateSnakeMovePositionList();
            updateSnakeTransform();
            updateSnakeBodyParts();
            isDead();



        }
    }

    private void updateSnakeMovePositions()
    {
        SnakeMovePosition snakeMovePreviousPosition = null;
        if (snakeMovePositionList.Count > 0)
        {
            snakeMovePreviousPosition = snakeMovePositionList[0];
        }

        SnakeMovePosition snakeMovePosition = new SnakeMovePosition(snakeMovePreviousPosition, gridPosition, gridMoveDirection);
        snakeMovePositionList.Insert(0, snakeMovePosition);
    }

    private void updateGridPosition()
    {
        Vector2Int gridMoveDirectionVector;

        switch (gridMoveDirection)
        {
            default:
            case direction.Up: gridMoveDirectionVector = new Vector2Int(0, +1); break;
            case direction.Down: gridMoveDirectionVector = new Vector2Int(0, -1); break;
            case direction.Right: gridMoveDirectionVector = new Vector2Int(+1, 0); break;
            case direction.Left: gridMoveDirectionVector = new Vector2Int(-1, 0); break;
        }

        gridPosition += gridMoveDirectionVector;
        gridPosition = levelGrid.validateGridPosition(gridPosition);
    }

    private void hasEaten()
    {
        if (levelGrid.hasEatenFood(gridPosition))
        {
            sc.increaseScore(1);
            snakeBodySize++;
            CreateSnakeBodyPart();
        }

        if (levelGrid.hasEatenBurner(gridPosition))
        {
            if (snakeBodySize <= 0)
            {
                GameOver();
                return;
            }
            snakeBodySize--;
            DestroySnakeBodyPart();
        }

        if (levelGrid.hasShielded(gridPosition))
        {
            StartCoroutine(ActivateShield());
        }
    }

    private void updateSnakeMovePositionList()
    {
        if (snakeMovePositionList.Count >= snakeBodySize + 1)
        {
            snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
        }
    }

    private void updateSnakeTransform()
    {
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
        transform.eulerAngles = snakeHeadRotation;
    }

    private void updateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private void isDead()
    {
        foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
        {
            Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
            if (gridPosition == snakeBodyPartGridPosition)
            {
                GameOver();
            }
        }
    }


    private void GameOver()
    {
        if (shieldActive) { return; }

        alive = false;
        goc.gameObject.SetActive(true);
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetAllGridPosition()
    {
        List<Vector2Int> allGridPosition = new List<Vector2Int>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            allGridPosition.Add(snakeMovePosition.gridPosition);
        }
        return allGridPosition;
    }


    private class SnakeMovePosition
    {
        public SnakeMovePosition previousSnakeMovePosition;
        public Vector2Int gridPosition;
        public direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return direction.Right;
            }
            else
            {
                return previousSnakeMovePosition.direction;
            }
        }

    }



    private class SnakeBodyPart
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public GameObject SnakeBody;

        public SnakeBodyPart(int bodyIndex)
        {
            SnakeBody = new GameObject("SnakeBody", typeof(SpriteRenderer));
            SnakeBody.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.Body;
            SnakeBody.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = SnakeBody.transform;
        }

        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.gridPosition;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.gridPosition.x, snakeMovePosition.gridPosition.y);

            float angle;

            switch (snakeMovePosition.direction)
            {
                default:
                case direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case direction.Up: angle = 45; break;
                        case direction.Down: angle = -45; break;
                    }
                    break;
                case direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case direction.Left: angle = 45; break;
                        case direction.Right: angle = -45; break;
                    }
                    break;
                case direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 0; break;
                        case direction.Up: angle = -45; break;
                        case direction.Down: angle = 45; break;
                    }
                    break;
                case direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default: angle = 90; break;
                        case direction.Left: angle = -45; break;
                        case direction.Right: angle = 45; break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);

        }

    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void DestroySnakeBodyPart()
    {
        Object.Destroy(snakeBodyPartList[snakeBodyPartList.Count - 1].SnakeBody);
        snakeBodyPartList.RemoveAt(snakeBodyPartList.Count - 1);
    }


    IEnumerator ActivateShield()
    {
        shieldActive = true;
        yield return new WaitForSeconds(5f);
        shieldActive = false;
    }

    private void SpawnSheild()
    {
        levelGrid.spawnShield();
    }

    private void destroyPowerUps()
    {
        levelGrid.destroyPowerUps();
    }

}