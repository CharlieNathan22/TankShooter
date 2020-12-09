using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float jumpHeight;
    public float playerSpeed;
    public float fireRate;
    public Transform[] wheelsToRotate;
    public Transform cannon;
    public Text reloadText;

    public GameObject missile;
    public Transform shotTransform;
    public GameObject explosion;

    AudioSource[] sounds;
    AudioSource missileSound;
    AudioSource reloadSound;

    private float verticalVelocity = 0;
    private float y = 0.0f;
    private float rotationSpeed = 120f;
    private float nextFire = 0.0F;
    private bool timerIsRunning = false;


    // Start is called before the first frame update
    void Start()
    {
        reloadText.gameObject.SetActive(false);
        //Get sounds from Audiosources
        sounds = GetComponents<AudioSource>();
        missileSound = sounds[0];
        reloadSound = sounds[1];
    }
    // Update is called once per frame
    void Update()
    {
        //If game isn't paused or over
        if(Time.timeScale == 1f)
        {
            // Rotate front part of tank by X axis on mouse
            float rotation = Input.GetAxis("Mouse X");
            cannon.Rotate(0, 4.0f * rotation, 0);

            float updown = Input.GetAxis("Mouse Y");
            // clamp allowed rotation to 30
            if (y + updown > 20 || y + updown < -20)
            {
                updown = 0;
            }
            y += updown;
            // Rotate camera by Y axis on mouse
            Camera.main.transform.RotateAround(transform.position,
            transform.right,
            -updown);
            Camera.main.transform.LookAt(transform);

            //Changing tank direction by left and right keyboard
            float rotateTank = Input.GetAxis("Horizontal");
            transform.Rotate(0, 0.2f * rotateTank, 0);

            // moving forwards and backwards
            float forwardSpeed = Input.GetAxis("Vertical") * playerSpeed;
            // apply gravity
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
            CharacterController characterController
            = GetComponent<CharacterController>();

            Vector3 speed = new Vector3(0, verticalVelocity, forwardSpeed);
            // transform this absolute speed relative to the player's current rotation
            // i.e. we don't want them to move "north", but forwards depending on where
            // they are facing
            speed = transform.rotation * speed;
            // what is deltaTime?
            // move at a different speed to make up for variable framerates
            characterController.Move(speed * Time.deltaTime);

            //rotate each wheel when moving
            if (forwardSpeed > 0)
            {
                for (int i = 0; i < wheelsToRotate.Length; i++)
                {
                    wheelsToRotate[i].Rotate(Vector3.right * Time.deltaTime * rotationSpeed);
                }
            }
            else if (forwardSpeed < 0)
            {
                for (int i = 0; i < wheelsToRotate.Length; i++)
                {
                    wheelsToRotate[i].Rotate(Vector3.left * Time.deltaTime * rotationSpeed);
                }
            }

            // if left click fire bullet
            if (Input.GetButton("Fire1"))
            {
                if (Time.time > nextFire)
                {
                    nextFire = Time.time + fireRate;
                    Instantiate(
                    missile,
                    shotTransform.position,
                    shotTransform.rotation);
                    missileSound.Play();
                    GameObject explosionEffect = Instantiate(explosion, shotTransform.position, Quaternion.identity);
                    explosionEffect.GetComponent<ParticleSystem>().Play();
                    reloadSound.Play();
                    timerIsRunning = true;
                }
                Debug.Log(fireRate);
            }

            //if currently reloading display reload time
            if (timerIsRunning)
            {
                float timeRemaining = nextFire - Time.time;
                if (timeRemaining > 0)
                {
                    reloadText.gameObject.SetActive(true);
                    DisplayTime(timeRemaining);
                }
                else
                {
                    Debug.Log("Time has run out!");
                    timeRemaining = 0;
                    reloadText.gameObject.SetActive(false);
                    timerIsRunning = false;
                    reloadSound.Stop();
                }
            }
        }
    }
    void DisplayTime(float timeToDisplay)
    {
        // Display time
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);
        reloadText.text = "Reloading! " + seconds;
    }

    public void SetfireRate(float newFireRate)
    {
        Debug.Log("Fire rate changed");
        fireRate = newFireRate;
    }
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //if bullet hits player then destroy bullet
        if(hit.gameObject.tag == "Bullet")
        {
            Destroy(hit.gameObject);
        }
    }
}
