using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    private float maxChrono = 2f;
    private float chrono;

    [SerializeField]
    private List<Vector2> directions;
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
                if (rayCastHit = Physics2D.Raycast(transform.position, directions[i], 1f))
                {
                    tempCase = rayCastHit.collider.GetComponent<Case>();
                    if (tempCase != null && tempCase.canBreak)
                    {
                        Destroy(tempCase.gameObject);
                    }
                }
                Destroy(gameObject);
            }            
        }
    }
}
