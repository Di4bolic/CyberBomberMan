using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Player : Character
{
    [SerializeField]
    private GameObject prefabBomb;

    public int bombNumber = 5;

    public float range = 1;
    public bool shield = false;

    private bool isActivated = false;
    private float chronoShieldMax = 2f;
    private float chronoShield;

    public TextMeshProUGUI rangeText;

    [SerializeField]
    private GameObject shieldImage;
    [SerializeField]
    private TextMeshProUGUI shieldText;

    [SerializeField]
    private GameObject player2UI;

    // Start is called before the first frame update
    void Start()
    {
        if (player2UI != null)
        {
            player2UI.SetActive(false);
        }
        chronoShield = chronoShieldMax;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Z))
        {
            Move(Vector2.up);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(Vector2.down);
        }
        if (Input.GetKey(KeyCode.Q))
        {
            Move(Vector2.left);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(Vector2.right);
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            SpawnBomb();
        }

        if (isActivated)
        {            
            if (chronoShield > 0)
            {
                chronoShield -= Time.deltaTime;
            }
            else
            {
                shield = false;
                shieldImage.SetActive(false);
                shieldText.text = "Shield : Inactive";
            }
        }
    }

    private void SpawnBomb()
    {
        if (bombNumber > 0)
        {
            bombNumber -= 1;
            var createdBomb = Instantiate(prefabBomb, new Vector2(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y)), Quaternion.identity);
            createdBomb.GetComponent<Bomb>().playerThatDropMe = this;
            createdBomb.GetComponent<Bomb>().range = range;
        }        
    }

    public void activateShield()
    {
        isActivated = true;
        shieldText.text = "Shield : Active";
    }

    public void ShowShieldUI()
    {
        shieldImage.SetActive(true);
        shieldText.text = "Shield : Inactive";
    }
}
