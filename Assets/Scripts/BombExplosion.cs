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
                    Destroy(chara.gameObject);
                }
            }
        }
    }
}
