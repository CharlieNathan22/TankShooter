using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonFire : MonoBehaviour
{
    public GameObject shot;
    public Transform shotTransform;
    public float fireRate;

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
        //constantly fire shots in a single direction when game is not paused or over
        if(Time.timeScale == 1)
        {
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
