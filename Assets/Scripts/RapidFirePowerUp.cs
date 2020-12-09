using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RapidFirePowerUp : MonoBehaviour
{
    public GameObject player;
    public float timeToLast;
    public float newFireRate;
    public Text reloadText;
    public Text powerUpText;
    public float rotationSpeed;

    PlayerController playerController;

    private AudioSource pickupSound;
    // Start is called before the first frame update
    void Start()
    {
        pickupSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Rotate Cube to look like a Pickup
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime * rotationSpeed); ;
    }

    private void OnTriggerEnter(Collider other)
    {
        //If a player enters the trigger, give the player Rapid fire, wait 5 seconds and then destroy pickup
        if (other.gameObject == player)
        {
            playerController = other.gameObject.GetComponent<PlayerController>();
            if (playerController == null)
            {
                Debug.Log("Cannot find 'PlayerController' script");
            }
            gameObject.GetComponent<MeshRenderer>().enabled = false;
            gameObject.GetComponent<BoxCollider>().enabled = false;
            gameObject.transform.localScale = new Vector3(0, 0, 0);
            StartCoroutine(StartCountDown(timeToLast));
        }
    }

    IEnumerator StartCountDown(float countdownTime)
    {
        pickupSound.Play();
        Debug.Log("rapid fire started");
        powerUpText.gameObject.SetActive(true);
        powerUpText.color = Color.red;
        powerUpText.text = "RAPID FIRE!";
        reloadText.gameObject.SetActive(false);
        float oldFireRate = playerController.fireRate;
        playerController.SetfireRate(newFireRate);
        yield return new WaitForSeconds(1);
        powerUpText.gameObject.SetActive(false);
        yield return new WaitForSeconds(countdownTime -1);
        Debug.Log("rapid fire ended" + playerController.fireRate);
        playerController.SetfireRate(oldFireRate);
        Debug.Log(playerController.fireRate);
        powerUpText.gameObject.SetActive(false);
        Destroy(gameObject);
    }
}
