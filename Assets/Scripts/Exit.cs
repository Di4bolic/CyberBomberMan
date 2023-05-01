using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur trouve la sortie cachée
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            SceneManager.LoadScene("WinningScene");
        }
    }
}
