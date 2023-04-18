using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabCanBreakCase, prefabCantBreakCase;

    public int difficulty;
    private float caseSelector;

    [SerializeField]
    private List<Case> listCases;
    private int casesCount = 0;

    [SerializeField]
    private GameObject player;
    [SerializeField]
    private GameObject enemy;
    [SerializeField]
    private List<int> spawnableCasesIndex;
    private int emptyCasesNear = 0;
    private int minEmptyCasesToSpawnPlayer = 4;
    //private int minEmptyCasesToSpawnEnemy = 3;


    // Start is called before the first frame update
    void Start()
    {
        GridSpawn();
        SpawnPlayer();
        //var results = spawnableCasesIndex.OrderByDescending(n => n);
        //for (int m = 0; m < spawnableCasesIndex.Count; m++)
        //{
        //    Debug.Log(spawnableCasesIndex[m]);
        //}
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void GridSpawn()
    {
        for (int i = -6; i < 7; i++)
        {
            // Création de la grille
            for (int j = -6; j < 7; j++)
            {
                // Vérifier si la case doit être incassable ou pas
                if (Mathf.Abs(i % 2) == 1 && Mathf.Abs(j % 2) == 1)
                {
                    // Instancier la case incassable
                    var actualCase = Instantiate(prefabCantBreakCase, new Vector2(j, i), Quaternion.identity).GetComponent<Case>();
                    listCases[casesCount] = actualCase;
                }
                else
                {
                    caseSelector = Random.Range(0, difficulty + 1);
                    if (caseSelector == 1)
                    {
                        // Instancier la case incassable
                        var actualCase = Instantiate(prefabCanBreakCase, new Vector2(j, i), Quaternion.identity).GetComponent<Case>();
                        listCases[casesCount] = actualCase;
                    }
                }

                casesCount++;
            }
        }
    }

    private void SpawnPlayer()
    {
        FindEmptyCases(minEmptyCasesToSpawnPlayer);
        var selectedCase = spawnableCasesIndex[Random.Range(0, spawnableCasesIndex.Count)];
        var xpos = -6 + selectedCase % 13;
        var ypos = -6 + selectedCase / 13;
        Instantiate(player, new Vector2(xpos, ypos), Quaternion.identity);
    }

    private void FindEmptyCases(int minEmptyCasesToSpawn)
    {
        for (int k = 0; k < listCases.Count; k++)
        {
            if (listCases[k] == null && !spawnableCasesIndex.Contains(k))
            {
                emptyCasesNear = 0;
                SearchNearEmptyCases(k);
                if (minEmptyCasesToSpawn > emptyCasesNear)
                {
                    for (int l = 0; l < emptyCasesNear; l++)
                    {
                        spawnableCasesIndex.RemoveAt(spawnableCasesIndex.Count-1);                        
                    }
                }                
            }
        }
    }

    private void SearchNearEmptyCases(int k)
    {
        spawnableCasesIndex.Add(k);
        emptyCasesNear++;

        if (k != 168)
        {
            if (listCases[k + 1] == null && !spawnableCasesIndex.Contains(k + 1) && k % 12 != 0)
            {
                SearchNearEmptyCases(k + 1);
            }
        }  
        
        if (k != 0)
        {
            if (listCases[k - 1] == null && !spawnableCasesIndex.Contains(k - 1) && k % 12 != 1)
            {
                SearchNearEmptyCases(k - 1);
            }
        }

        if (k <= 155)
        {
            if (listCases[k + 13] == null && !spawnableCasesIndex.Contains(k + 13))
            {
                SearchNearEmptyCases(k + 13);
            }
        }

        if (k >= 13)
        {
            if (listCases[k - 13] == null && !spawnableCasesIndex.Contains(k - 13))
            {
                SearchNearEmptyCases(k - 13);
            }
        }        
    }
}
