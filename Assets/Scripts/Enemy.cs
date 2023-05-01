using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Character
{
    [SerializeField]
    private LayerMask acceptedLayers;

    [SerializeField]
    private List<Vector2> directions;

    [SerializeField]
    private List<bool> directionsWhereCanMove;

    private float chronoMax = 1.2f;
    private float chrono;

    // Start is called before the first frame update
    void Start()
    {
        chrono = chronoMax;

        speed = 250f;
        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);

        directionsWhereCanMove.Add(false);
        directionsWhereCanMove.Add(false);
        directionsWhereCanMove.Add(false);
        directionsWhereCanMove.Add(false);
    }

    // Update is called once per frame
    void Update()
    {
        // Permet d'effectuer le mouvement tout les chronoMax secondes
        if (chrono > 0)
        {
            chrono -= Time.deltaTime;
        }
        else
        {
            CheckThenMove();
            chrono = chronoMax;
        }
    }

    private void CheckThenMove()
    {
        // "Regarde" dans les quatres directions et valide lesquelles sont libres
        for (int i = 0; i < 4; i++)
        {
            if (!Physics2D.Raycast(transform.position, directions[i], 1, acceptedLayers))
            {
                directionsWhereCanMove[i] = true;
            }
            else
            {
                directionsWhereCanMove[i] = false;
            }
        }
        // Prend une direction au hasard
        var chosenDirectionIndex = Random.Range(0, 4);
        while (!directionsWhereCanMove[chosenDirectionIndex])
        {
            chosenDirectionIndex = Random.Range(0, 4);
        }
        // Effectue le mouvement
        Move(directions[chosenDirectionIndex]);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si le joueur entre en collision
        var player = collision.gameObject.GetComponent<Player>();
        if (player != null)
        {
            // Vérifie si le joueur possède un shiled
            if (player.shield)
            {
                player.activateShield();
            }
            // Met fin à la partie
            else
            {
                player.EndGame();
            }
        }
    }
}
