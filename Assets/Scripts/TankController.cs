using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TankController : MonoBehaviour
{
    public GameObject target;
    public GameObject missile;
    public Transform barrel;
    public Transform shotTransform;
    public float fireRate;

    public bool seenTarget;
    public StateMachine stateMachine = new StateMachine();

    private float nextFire = 0.0F;
    private AudioSource missileSound;

    NavMeshAgent agent;
    SphereCollider areaCollider;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        areaCollider = GetComponent<SphereCollider>();
        missileSound = GetComponent<AudioSource>();
        seenTarget = false;
        Debug.Log(areaCollider.radius);
        stateMachine.ChangeState(new State_Patrol(this));
    }

    // Update is called once per frame
    void Update()
    {
        stateMachine.Update();
        //If tank is in attackmode (stopped moving), fire missiles at player
        if (agent.isStopped)
        {
            Debug.Log("Agent is stopped");
            //aim barrel at player
            barrel.LookAt(target.transform.position);
            // fire enemy missile
            if (Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Instantiate(
                missile,
                shotTransform.position,
                shotTransform.rotation);
                missileSound.Play();
            }
        }

    }
    private void OnTriggerEnter(Collider other)
    {
        // is it the player?
        Debug.Log("Entered Trigger");
        if (other.gameObject == target)
        {
            Debug.Log("Target in Range");
            // angle between us and the player
            Vector3 direction = target.transform.position - transform.position;
            RaycastHit hit;
            if (Physics.Raycast(transform.position,
            direction.normalized,
            out hit))
            {
                // flag that we've seen the player
                Debug.Log("hit Target!");
                seenTarget = true;
            }
            else
            {
                seenTarget = false;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //when the player moves out of range, switch states
        if(other.gameObject == target)
        {
            seenTarget = false;
            Debug.Log("Lost Target!");
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (areaCollider != null)
        {
            Gizmos.DrawWireSphere(transform.position, 20);
            if (seenTarget)
                Gizmos.DrawLine(transform.position, target.transform.position);
        }
    }
}
