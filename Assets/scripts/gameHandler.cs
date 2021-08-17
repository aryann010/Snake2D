using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameHandler : MonoBehaviour
{
    private levelGrid levelGridInstance;
    [SerializeField] private snakeController snakeI;

    void Start()
    {
        levelGridInstance = new levelGrid(20, 20);
        snakeI.setup(levelGridInstance);
        levelGridInstance.Setup(snakeI);

        levelGridInstance.spawnMassGainer();
        levelGridInstance.spawnMassBurner();
    }

}