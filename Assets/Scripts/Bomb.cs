using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public Player playerThatDropMe;
    private float maxChrono = 2f;
    private float chrono;
    private float maxChronoBis = 0.35f;
    private float chronoBis;
    private bool hasExplosed = false;

    [SerializeField]
    private GameObject bombExplosionSide;
    [SerializeField]
    private GameObject bombExplosionEnd;

    private GameObject explosion;

    public float range;

    [SerializeField]
    private List<Vector2> directions;
    [SerializeField]
    private LayerMask acceptedLayers;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private Collider2D col2D;

    // Start is called before the first frame update
    void Start()
    {
        chrono = maxChrono;
        chronoBis = maxChronoBis;

        directions.Add(Vector2.up);
        directions.Add(Vector2.down);
        directions.Add(Vector2.left);
        directions.Add(Vector2.right);        
    }

    // Update is called once per frame
    void Update()
    {
        if (chrono > 0)
        {
            chrono -= Time.deltaTime;
        }
        else if(!hasExplosed)
        {
            col2D.enabled = true;
            hasExplosed = true;
            animator.SetBool("Explode", true);
            Case tempCase;
            RaycastHit2D rayCastHit;

            // Vérifie la situation autour de la bombe
            for (int i = 0; i < directions.Count; i++)
            {
                if (rayCastHit = Physics2D.Raycast(transform.position, directions[i], range, acceptedLayers))
                {
                    tempCase = rayCastHit.collider.GetComponent<Case>();
                    var distance = Mathf.Ceil(rayCastHit.distance);
                    if (tempCase != null && tempCase.canBreak)
                    {
                        // Détriut la case dans une direction qui est dans la range de la bombe
                        Destroy(tempCase.gameObject);                        
                        InstantiateExplosions(distance, i);
                    }
                    else
                    {
                        InstantiateExplosions(distance-1f, i);
                    }
                }
                else
                {
                    InstantiateExplosions(range, i);
                }                
            }            
        }
        if (chronoBis > 0)
        {
            if (hasExplosed)
            {
                chronoBis -= Time.deltaTime;
            }            
        }
        else
        {
            DestroyMySelf();
        }
    }

    private void InstantiateExplosions(float distance, int i)
    {
        // Permet d'instancier les lignes d'ecxplosions après avoir vérifier la distance de celles-çi
        for (int j = 1; j <= distance; j++)
        {
            if (j == distance)
            {
                explosion = Instantiate(bombExplosionEnd, new Vector2(transform.position.x, transform.position.y) + directions[i] * j, Quaternion.identity);
            }
            else
            {
                explosion = Instantiate(bombExplosionSide, new Vector2(transform.position.x, transform.position.y) + directions[i] * j, Quaternion.identity);
            }            
            var rotationZ = 0f;
            if (directions[i].x == 0)
            {
                rotationZ = directions[i].y * 90f;
            }
            if (directions[i].x < 0)
            {
                rotationZ = 180f;
            }
            explosion.transform.eulerAngles = new Vector3(0, 0, rotationZ);            
        }
    }

    private void DestroyMySelf()
    {
        playerThatDropMe.bombNumber += 1;
        Destroy(gameObject);
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
