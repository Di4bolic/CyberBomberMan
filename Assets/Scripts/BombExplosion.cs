using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombExplosion : MonoBehaviour
{
    private float maxChrono = 0.35f;
    private float chrono;

    // Start is called before the first frame update
    void Start()
    {
        chrono = maxChrono;
    }

    // Update is called once per frame
    void Update()
    {
        // Détruit l'explosion quand elle est terminée
        if (chrono > 0)
        {
            chrono -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Détruit l'enemi ou met fin à la partie si un des deux acteurs rentre dans le trigger
        var chara = collision.gameObject.GetComponent<Character>();
        var player = collision.gameObject.GetComponent<Player>();
        if (chara != null)
        {
            if (player == null)
            {
                Destroy(chara.gameObject);
            }
            else
            {
                if (player.shield)
                {
                    player.activateShield();
                }
                else
                {
                    player.IsKilledByAPlayer();
                }
            }
        }
    }
}
