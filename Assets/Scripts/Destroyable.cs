using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destroyable : MonoBehaviour
{
    public int timeBonus;
    GameController gameController;

    // Start is called before the first frame update
    void Start()
    {
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

    }

    private void OnCollisionEnter(Collision collision)
    {
        // destroy this object if collided with bullet
        if (collision.gameObject.tag.Equals("Bullet") == true)
        {
            Destroy(gameObject);
        }
    }
    private void OnDestroy()
    {
        // tell the game controller
        if (gameController != null)
        {
            gameController.TargetDestroyed(timeBonus);
        }
    }
}

