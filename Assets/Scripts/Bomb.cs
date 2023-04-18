using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float maxChrono = 2f;
    private float chrono;

    [SerializeField]
    private GameObject bombExplosion;

    public float range;

    [SerializeField]
    private List<Vector2> directions;
    [SerializeField]
    private LayerMask acceptedLayers;
    [SerializeField]
    private Sprite spriteEndExplosion;
    [SerializeField]
    private Animator animator;


    // Start is called before the first frame update
    void Start()
    {
        chrono = maxChrono;
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
        else
        {
            Case tempCase;
            RaycastHit2D rayCastHit;

            for (int i = 0; i < directions.Count; i++)
            {
                if (rayCastHit = Physics2D.Raycast(transform.position, directions[i], range, acceptedLayers))
                {
                    tempCase = rayCastHit.collider.GetComponent<Case>();
                    if (tempCase != null && tempCase.canBreak)
                    {
                        Destroy(tempCase.gameObject);
                    }

                    var distance = Mathf.Ceil(rayCastHit.distance);
                    InstantiateExplosions(distance, i);
                }
                else
                {
                    InstantiateExplosions(range, i);
                }
                animator.SetTrigger("GoBoomBoom");
                //Destroy(gameObject);
            }            
        }
    }

    private void InstantiateExplosions(float distance, int i)
    {
        for (int j = 1; j <= distance; j++)
        {
            var explosion = Instantiate(bombExplosion, new Vector2(transform.position.x, transform.position.y) + directions[i] * j, Quaternion.identity);
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
            if (j == distance)
            {
                explosion.GetComponent<BombExplosion>().sr.sprite = spriteEndExplosion;
            }
        }
    }
}
