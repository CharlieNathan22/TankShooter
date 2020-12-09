using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPickUp : MonoBehaviour
{
    GameController gameController; 

    public GameObject player;
    public float health;
    public float rotationSpeed;
    public Text powerupText;
    private AudioSource pickupSound;

    // Start is called before the first frame update
    void Start()
    {
        pickupSound = GetComponent<AudioSource>();
        // Assign the GameController script in the GameController object
        GameObject gameControllerObject = GameObject.FindWithTag("GameController");
        if (gameControllerObject != null)
        {
            gameController = gameControllerObject.GetComponent<GameController>();
        }
        if (gameController == null)
        {
            Debug.Log("Cannot find 'GameController' script");
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate Cube to look like a Pickup
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * rotationSpeed); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If a player enters the trigger, give the player health and destroy pickup
        if(other.gameObject == player)
        {
            gameController.health += health;
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(StartCountDown(1.5f));
        }
    }
    IEnumerator StartCountDown(float countdownTime)
    {
        pickupSound.Play();
        powerupText.gameObject.SetActive(true);
        powerupText.color = Color.blue;
        powerupText.text = "+20 Health!";
        yield return new WaitForSeconds(countdownTime);
        powerupText.gameObject.SetActive(false);
        Debug.Log("Health pack taken!");
        Destroy(gameObject);
    }

}
