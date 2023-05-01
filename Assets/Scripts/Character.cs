using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D rb;

    public float speed = 30f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Move(Vector2 direction)
    {
        // Calcule et Action de mouvement
        rb.AddForce(direction * speed, ForceMode2D.Force);
    }
}
