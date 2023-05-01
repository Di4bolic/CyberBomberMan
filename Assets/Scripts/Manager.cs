using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class Manager : MonoBehaviour
{
    public string nameOfTheWinner;
    public bool isDuel;
    private Manager oldManager;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
        // Permet de changer le texte de fin selon qui gagne la partie
        var gOLT = GameObject.Find("Loosetext");
        if (gOLT != null)
        {
            oldManager = GameObject.Find("Manager").GetComponent<Manager>();
            gOLT.GetComponent<TextMeshProUGUI>().text = oldManager.nameOfTheWinner + " has lost ...";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartTheGameInDuel(bool isIt)
    {
        // Lance une partie en mode duel
        isDuel = isIt;
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
        // Retourne au menu après avoir supprimé les anciens "Manager"
        if (oldManager != null)
        {
            Destroy(oldManager.gameObject);
        }
        Destroy(gameObject);
        Destroy(GameObject.Find("Manager"));
        SceneManager.LoadScene("MainScene");
    }

    public void Retry()
    {
        // Relance une partie dans le mode précédant après avoir supprimé le "Manager" qui ne contient pas les bonnes informations
        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        // Ferme le jeu
        Application.Quit();
    }
}
