using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    Vector3 playerPosition;

    public GameObject shot;
    public Transform shotTransform;
    public Transform turretBarrel;
    public float fireRate;
    public GameObject player;

    private float nextFire = 0.0F;
    private AudioSource source;

    // Start is called before the first frame update
    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnTriggerStay(Collider other)
    {
        //if player is in range fire at player
        // is it the player?
        if (other.gameObject == player)
        {
            // remember their position
            playerPosition = player.transform.position;
            turretBarrel.LookAt(playerPosition);

            if (Time.time > nextFire)
            {
                //fire
                nextFire = Time.time + fireRate;
                Instantiate(
                shot,
                shotTransform.position,
                shotTransform.rotation);
                // play the sound
                source.Play();
            }
        }
    }
}
