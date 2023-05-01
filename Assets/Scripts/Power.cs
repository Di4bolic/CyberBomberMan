using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Power : MonoBehaviour
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
        // Si le power est trigger par un joueur
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            DoStuff(player);
            Destroy(gameObject);
        }
    }

    public virtual void DoStuff(Player player)
    {

    }
}
