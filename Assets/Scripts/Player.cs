using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

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

    private Manager manager;

    [SerializeField]
    private string nameOfTheWinner;

    // Lié aux Inputs
    private Vector2 movementInput = Vector2.zero;
    private bool canPlaceBomb = false;

    // Start is called before the first frame update
    void Start()
    {        
        chronoShield = chronoShieldMax;

        manager = GameObject.Find("Manager").GetComponent<Manager>();

        if (player2UI != null && !manager.isDuel)
        {
            player2UI.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (canPlaceBomb == true)
        {
            SpawnBomb();
        }

        Move(movementInput);        

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

    public void OnMove(InputAction.CallbackContext context)
    {
        movementInput =  context.ReadValue<Vector2>();
    }
    public void OnPlaceBomb(InputAction.CallbackContext context)
    {
        canPlaceBomb = context.action.triggered;
    }

    private void SpawnBomb()
    {
        canPlaceBomb = false;
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

    public void IsKilledByAPlayer()
    {
        manager.nameOfTheWinner = nameOfTheWinner;
        EndGame();
    }

    public void EndGame()
    {
        SceneManager.LoadScene("LoosingScene");
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }
}
