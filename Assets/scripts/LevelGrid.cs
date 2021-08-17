using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class levelGrid
{
    private int width;
    private int height;
    private Vector2Int massGainerGridPosition;
    private Vector2Int massBurnerGridPosition;
    private Vector2Int shieldGridPosition;
    private GameObject massGainer;
    private GameObject massBurner;
    private GameObject shield;
    private snakeController snake;
    

    public levelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void Setup(snakeController snake)
    {
        this.snake = snake;
    }

    public void spawnMassGainer()
    {
        do
        {
            massGainerGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (snake.GetAllGridPosition().Contains(massGainerGridPosition));

        massGainer = new GameObject("massGainer", typeof(SpriteRenderer));
        massGainer.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.massGainer;
        massGainer.transform.position = new Vector3(massGainerGridPosition.x, massGainerGridPosition.y);
    }

    public void spawnMassBurner()
    {
        List<Vector2Int> OccupiedPositions = new List<Vector2Int>();
        OccupiedPositions = snake.GetAllGridPosition();
        OccupiedPositions.Add(massGainerGridPosition);

        do
        {
            massBurnerGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (OccupiedPositions.Contains(massBurnerGridPosition));

        massBurner = new GameObject("massBurner", typeof(SpriteRenderer));
        massBurner.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.massBurner;
        massBurner.transform.position = new Vector3(massBurnerGridPosition.x, massBurnerGridPosition.y);

    }

    public void spawnShield()
    {
        List<Vector2Int> OccupiedPositions = new List<Vector2Int>();
        OccupiedPositions = snake.GetAllGridPosition();
        OccupiedPositions.Add(massGainerGridPosition);
        OccupiedPositions.Add(massBurnerGridPosition);

        do
        {
            shieldGridPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));
        } while (OccupiedPositions.Contains(shieldGridPosition));

        shield = new GameObject("shield", typeof(SpriteRenderer));
        shield.GetComponent<SpriteRenderer>().sprite = gameAssets.Instance.shield;
        shield.transform.position = new Vector3(shieldGridPosition.x, shieldGridPosition.y);
    }

    public bool hasEatenFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == massGainerGridPosition)
        {
            
            Object.Destroy(massGainer);
            spawnMassGainer();
            Object.Destroy(massBurner);
            spawnMassBurner();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool hasEatenBurner(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == massBurnerGridPosition)
        {
            
            Object.Destroy(massBurner);
            spawnMassBurner();
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool hasShielded(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == shieldGridPosition)
        {
            Object.Destroy(shield);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void destroyPowerUps()
    {
        Object.Destroy(shield);
    }

    public Vector2Int validateGridPosition(Vector2Int gridPosition)
    {
        if (gridPosition.x < 0)
        {
            gridPosition.x = width-1 ;
        }
        if (gridPosition.y < 0)
        {
            gridPosition.y = height - 1;
        }
        if (gridPosition.x > width -1)
        {
            gridPosition.x = 0;
        }
        if (gridPosition.y > height - 1)
        {
            gridPosition.y = 0;
        }
        return gridPosition;
    }
}