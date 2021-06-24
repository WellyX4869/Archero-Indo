using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LevelGenerator : MonoBehaviour
{
    [Header("Environments")]
    [SerializeField] Vector2Int floorSize;
    [SerializeField] GameObject[] floorObject;
    [SerializeField] Transform floorParent;
    [SerializeField] GameObject wallObject;
    [SerializeField] List<Vector2> randomWallPos;
    [SerializeField] Transform wallParent;
    [SerializeField] float floorY = -55f;
    [SerializeField] GameObject openDoor;
    [SerializeField] GameObject closeDoor;
    [SerializeField] GameObject fakeCloseDoor;
    [SerializeField] Transform doorParent;
    [SerializeField] GameObject house;

    [Header("Player")]
    [SerializeField] GameObject player;

    [Header("Enemies")]
    [SerializeField] List<GameObject> enemies;
    [SerializeField] Transform enemiesParent;

    List<List<bool>> isPlacable = new List<List<bool>>();
    GameSession gameSession;
    NavMeshSurface surface;
    int edgeX, edgeY;
    float gridSize;

    public bool isDemo = false;
    
    // Start is called before the first frame update
    void Start()
    {
        gameSession = FindObjectOfType<GameSession>();
        if(gameSession.currentLevel == 1)
            gameSession.SetDifficulty();
        if (isDemo)
            gameSession.maxLevel = 2;
        surface = GetComponent<NavMeshSurface>();
        GenerateLevel();
    }

    void GenerateLevel()
    {
        edgeY = floorSize.y / 2;
        edgeX = floorSize.x / 2;

        #region Generate Floor
        isPlacable.Clear();
        int counter = 0;
        for (int i = -edgeX; i <= edgeX; i++)
        {
            List<bool> tempPlacable = new List<bool>();
            for (int j = -edgeY; j <= edgeY; j++)
            {
                GenerateFloor(counter, i, j);
                tempPlacable.Add(true);
                counter++;
                if (counter == 1)
                {
                    gridSize = floorObject[0].GetComponent<SnapEditor>().GetGridSize();
                }
            }
            isPlacable.Add(tempPlacable);
        }
        #endregion

        #region Generate Wall
        
        #region Generate Side Wall
        for (int i = -edgeX; i<=edgeX; i+=floorSize.x-1)
        {
            for(int j = -edgeY; j<=edgeY; j++)
            {
                GenerateWall(i, j);
                isPlacable[i+edgeX][j+edgeY] = false;
            }
        }
        #endregion
        #region Generate Back Wall
        for (int i = -edgeX+1; i < edgeX; i++)
        {
            if(i <-1 || i > 1)
            {
                GenerateWall(i, -edgeY);
            }
            else if(i == 0)
            {
                var fakeCloseDoorClone = Instantiate(fakeCloseDoor, fakeCloseDoor.transform.position, Quaternion.identity);
                fakeCloseDoorClone.transform.parent = doorParent;
            }
            isPlacable[i + edgeX][0] = false;
        }
        #endregion
        #region Generate Front Wall
        for (int i = -edgeX + 1; i < edgeX; i++)
        {
            if(i <-1 || i > 1)
            {
                GenerateWall(i, edgeY);
            }
            else if(i == 0)
            {
                // Generate OpenDoor and CloseDoor
                var closeDoorClone = Instantiate(closeDoor, closeDoor.transform.position, Quaternion.identity);
                closeDoorClone.transform.parent = doorParent;
                var openDoorClone = Instantiate(openDoor, openDoor.transform.position, Quaternion.identity);
                openDoorClone.transform.parent = doorParent;
                FindObjectOfType<LevelHandler>().GetDoors();
            }
            isPlacable[i + edgeX][floorSize.y-1] = false;
        }
        #endregion
        #region Generate Back Outside Wall
        counter = 0;
        for (int i = -edgeY - 1; i >= -edgeY - 13; i--)
        {
            if (gameSession.currentLevel == 1)
            {
                for (int j = -edgeX; j <= edgeX; j++)
                {
                    GenerateWall(j, i);
                }
            }
            else
            {
                // Generate Back Floor
                for (int j = -edgeX; j <= edgeX; j++)
                {
                    GenerateFloor(counter, j, i);
                    counter++;
                }
               
                // Generate Back Wall 
                GenerateWall(-edgeX, i);
                GenerateWall(edgeX, i);
            }
        }
        #endregion
        #region Generate Front Outside 
        counter = 0;
        for (int i = edgeY + 1; i <= edgeY + 13; i++)
        {
            // Generate Front Floor
            for (int j = -edgeX; j <= edgeX; j++)
            {
                GenerateFloor(counter, j, i);
                counter++;
            }
            GenerateWall(-edgeX, i);
            GenerateWall(edgeX, i);
        }

        if(gameSession.currentLevel == gameSession.maxLevel)
        {
            var houseClone = Instantiate(house, house.transform.position, house.transform.rotation);
            houseClone.transform.parent = transform;
        }
        #endregion
        #region Generate Random Wall
        int randomWallsCount = gameSession.currentLevel - 1; 
        if(randomWallsCount > randomWallPos.Count)
        {
            randomWallsCount = randomWallPos.Count;
        }
        
        // maks wall random count = 20
        List<Vector2> randomPos = new List<Vector2>();
        for(int i = 0; i< randomWallPos.Count; i++)         {
            randomPos.Add(randomWallPos[i]);
        }

        for(int i = 0; i<randomWallsCount; i++) 
        {
            int randomIndex = UnityEngine.Random.Range(0, randomPos.Count);
            Vector2 pos = randomPos[randomIndex];
            GenerateWall((int)pos.x, (int)pos.y);
            isPlacable[(int)pos.x + edgeX][(int)pos.y + edgeY] = false;
            if (pos.x != 0f)
            {
                GenerateWall((int)-pos.x, (int)pos.y);
                isPlacable[(int)-pos.x + edgeX][(int)pos.y + edgeY] = false;
            }
            randomPos.RemoveAt(randomIndex);
        }

        #endregion

        #endregion

        surface.BuildNavMesh();

        SpawnEnemies();

        SpawnPlayer();
        //for (int i = 0; i < isPlacable.Count; i++)
        //{
        //    for (int j = 0; j < isPlacable[i].Count; j++)
        //    {
        //        if (isPlacable[i][j] == false)
        //        {
        //            Debug.Log("Pos(" + (i-edgeX) + "," + (j-edgeY) + ") is not placable");
        //        }
        //    }
        //}
    }

    private void GenerateFloor(int counter, int i, int j)
    {
        var floorTile = Instantiate(floorObject[counter % 2], transform.position, Quaternion.identity);
        floorTile.transform.parent = floorParent;
        floorTile.GetComponent<SnapEditor>().ChangePosition(new Vector3(i, floorY, j));
    }

    private void GenerateWall(int i, int j)
    {
        var wallTile = Instantiate(wallObject, transform.position, Quaternion.identity);
        wallTile.transform.parent = wallParent;
        float posY = floorY + wallTile.transform.localScale.y/2 + floorObject[0].transform.localScale.y / 2;
        wallTile.GetComponent<SnapEditor>().ChangePosition(new Vector3(i, posY, j));
    }

    private void SpawnPlayer()
    {
        var playerClone = Instantiate(player, transform.position, Quaternion.identity);
        CameraMovement.Instance.GetPlayer();
        FindObjectOfType<LevelHandler>().GetPlayer();
        //PlayerHpBar.Instance.SetHP(PlayerData.Instance.currentHp, PlayerData.Instance.maxHp);
    }

    private void SpawnEnemies()
    {
        int maxEnemies = gameSession.currentLevel/2 + 2;
        int checkerEnemies = (gameSession.currentLevel / 3);
        int variantEnemies = checkerEnemies > 3? 3: checkerEnemies;
        int enemiesSpawned = 0;

        if(isDemo)
        {
            variantEnemies = 3;
            maxEnemies += 1;
        }

        while (enemiesSpawned < maxEnemies)
        {
            for (int i = -edgeX+2; i < edgeX-1; i++)
            {
                if (enemiesSpawned >= maxEnemies) break;
                for (int j = -6; j < edgeY; j++)
                {
                    if (enemiesSpawned >= maxEnemies) break;
                    if (!isPlacable[i + edgeX][j + edgeY]) continue;

                    if (UnityEngine.Random.Range(0f, 1f) > 0.95f) // random chance to spawn enemies
                    {
                        if (isPlacable[i + edgeX][j + edgeY])
                        {
                            Vector3 enemyPos = new Vector3(i * gridSize, floorY, j * gridSize);
                            var randomEnemyIndex = UnityEngine.Random.Range(0, variantEnemies);
                            if (isDemo)
                            {
                                randomEnemyIndex = enemiesSpawned % 3;
                            }
                            var enemy = Instantiate(enemies[randomEnemyIndex], enemyPos, Quaternion.identity);
                            enemy.transform.parent = enemiesParent;
                            enemiesSpawned++;
                            isPlacable[i + edgeX][j + edgeY] = false;
                        }
                    }
                }
            }
        }
    }
}
