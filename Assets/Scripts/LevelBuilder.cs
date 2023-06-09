using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LevelBuilder : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabCanBreakCase, prefabCantBreakCase;

    private int difficulty = 1;
    private float caseSelector;

    [SerializeField]
    private List<Case> listCases;
    private int casesCount = 0;

    private int numberOfPlayers;
    [SerializeField]
    private List<GameObject> players;

    [SerializeField]
    private int numberOfEnemies;
    [SerializeField]
    private GameObject enemy;

    [SerializeField]
    private List<int> spawnableCasesIndex;
    private int emptyCasesNear = 0;
    private int minEmptyCasesToSpawnPlayer = 4;
    private int minEmptyCasesToSpawnEnemy = 3;

    private int numberOfPower1;
    [SerializeField]
    private GameObject power1;

    private int numberOfPower2 = 1;
    [SerializeField]
    private GameObject power2;

    private List<int> emptyCasesIndexesWithSomething = new List<int>();

    [SerializeField]
    private GameObject exitPrefab;

    private Manager manager;

    // Start is called before the first frame update
    void Start()
    {
        // Gestion de la souris
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        // Appelle des fonctions de construction du level
        ModeSelection();
        GridSpawn();
        PlacePlayers();
        PlaceEnemies();
        PlacePower(power1, numberOfPower1);
        PlacePower(power2, numberOfPower2);
    }

    private void ModeSelection()
    {
        // Modifie les param�tre de construction en fonction du mode de jeu
        manager = GameObject.Find("Manager").GetComponent<Manager>();
        if (manager.isDuel)
        {
            OnDuel();
        }
        else
        {
            OnSolo();
        }
    }

    private void OnDuel()
    {
        numberOfPlayers = 2;
        numberOfEnemies = 0;
        numberOfPower1 = 7;
        numberOfPower2 = 2;
    }

    private void OnSolo()
    {
        numberOfPlayers = 1;
        numberOfEnemies = 2 * difficulty;
        numberOfPower1 = 4;
        numberOfPower2 = 1;
    }

    private void GridSpawn()
    {
        // Construction de la grille
        for (int i = -6; i < 7; i++)
        {
            // Cr�ation de la grille
            for (int j = -6; j < 7; j++)
            {
                // V�rifier si la case doit �tre incassable ou pas
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
                        // Instancier la case cassable
                        var actualCase = Instantiate(prefabCanBreakCase, new Vector2(j, i), Quaternion.identity).GetComponent<Case>();
                        listCases[casesCount] = actualCase;
                    }
                }

                casesCount++;
            }
        }

        if (!manager.isDuel)
        {
            // Permet de placer la sortie si mode solo
            PlaceExit();
        }        
    }

    private void PlacePlayers()
    {
        // Permet de placer les/le joueur(s)
        if (numberOfPlayers > players.Count)
        {
            numberOfPlayers = players.Count;
        }
        else if (numberOfPlayers <= 0)
        {
            numberOfPlayers = 1;
        }
        spawnableCasesIndex.Clear();
        FindEmptyCases(minEmptyCasesToSpawnPlayer);
        for (int m = players.Count - 1; m >= numberOfPlayers; m--)
        {
            Destroy(players[m]);
            players.RemoveAt(m);
        }
        // On place chaque joueur dans une zone contenant minimum un certains nombre de cases vides et on le place sur une case qui ne contient rien d'autre
        for (int n = 0; n < players.Count; n++)
        {
            var selectedCaseIndex = spawnableCasesIndex[Random.Range(0, spawnableCasesIndex.Count)];
            while (emptyCasesIndexesWithSomething.Contains(selectedCaseIndex))
            {
                selectedCaseIndex = spawnableCasesIndex[Random.Range(0, spawnableCasesIndex.Count)];
            }
            emptyCasesIndexesWithSomething.Add(selectedCaseIndex);
            var xpos = -6 + selectedCaseIndex % 13;
            var ypos = -6 + selectedCaseIndex / 13;
            players[n].transform.position = new Vector2(xpos, ypos);
        }
    }

    private void PlaceEnemies()
    {
        // Similaire � la mani�re de placer un joueur
        spawnableCasesIndex.Clear();
        FindEmptyCases(minEmptyCasesToSpawnEnemy);
        for (int n = 0; n < numberOfEnemies; n++)
        {
            var selectedCaseIndex = spawnableCasesIndex[Random.Range(0, spawnableCasesIndex.Count)];
            while (emptyCasesIndexesWithSomething.Contains(selectedCaseIndex))
            {
                selectedCaseIndex = spawnableCasesIndex[Random.Range(0, spawnableCasesIndex.Count)];
            }
            emptyCasesIndexesWithSomething.Add(selectedCaseIndex);
            var xpos = -6 + selectedCaseIndex % 13;
            var ypos = -6 + selectedCaseIndex / 13;
            Instantiate(enemy, new Vector2(xpos, ypos), Quaternion.identity);
        }

    }

    private void PlacePower(GameObject power, int number)
    {
        // Similaire � la mani�re de placer un joueur
        for (int o = 0; o < number; o++)
        {
            var selectedCaseIndex = Random.Range(0, listCases.Count);
            while (listCases[selectedCaseIndex] != null || emptyCasesIndexesWithSomething.Contains(selectedCaseIndex))
            {
                selectedCaseIndex = Random.Range(0, listCases.Count);
            }
            emptyCasesIndexesWithSomething.Add(selectedCaseIndex);
            var xpos = -6 + selectedCaseIndex % 13;
            var ypos = -6 + selectedCaseIndex / 13;
            Instantiate(power, new Vector2(xpos, ypos), Quaternion.identity);
        }
    }

    private void FindEmptyCases(int minEmptyCasesToSpawn)
    {
        // Permet de lancer la fonction qui va remplir une liste d'index de zones vides contenant au minimum "minEmptyCasesToSpawn"
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
        // Fonction r�cursive permettant de trouver les cases voisines vides permettant de constituer une zone vide
        spawnableCasesIndex.Add(k);
        emptyCasesNear++;

        if (k != 168)
        {
            if (listCases[k + 1] == null && !spawnableCasesIndex.Contains(k + 1) && (k + 1) / 13f != Mathf.Round((k + 1) / 13f))
            {
                SearchNearEmptyCases(k + 1);
            }
        }  
        
        if (k != 0)
        {
            if (listCases[k - 1] == null && !spawnableCasesIndex.Contains(k - 1) && k / 13f != Mathf.Round( k / 13f))
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

    private void PlaceExit()
    {
        // Place la sortie en dessous d'une case contenant un bloc cassable
        var secretCaseIndex = Random.Range(0, listCases.Count);
        if (listCases[secretCaseIndex] != null)
        {
            if (listCases[secretCaseIndex].canBreak)
            {
                var xpos = -6 + secretCaseIndex % 13;
                var ypos = -6 + secretCaseIndex / 13;
                Instantiate(exitPrefab, new Vector2(xpos, ypos), Quaternion.identity);
            }
            else
            {
                PlaceExit();
            }
        }
        else
        {
            PlaceExit();
        }
    }
}
