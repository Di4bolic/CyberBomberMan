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
        isDuel = isIt;
        SceneManager.LoadScene("GameScene");
    }

    public void GoToMenu()
    {
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
        Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
