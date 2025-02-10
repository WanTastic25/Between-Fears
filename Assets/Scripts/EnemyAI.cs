using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{
    public GameObject PlayerObj;
    public Transform PlayerTransform;
    public Rigidbody playerRB;
    public Vector3 playerCurrentPos;
    public NavMeshAgent NavMeshAgent;
    public bool aggression;

    private bool isAggressiveCoroutineRunning;
    public Transform[] nodes;
    public bool GoToDoor;
    public bool choiceNode;
    Vector3 originalDest;
    public GameObject doorObjCheck;
    public Vector3 locationCheck;
    public Animator animator;

    public bool noiseCheck;
    public bool attackisRunning;
    private float timePassed = 0f;
    public healthHandler playerHealth;

    public AudioSource monsterBreathe;
    public AudioSource monsterScream;
    public AudioSource monsterAttack;
    public AudioSource monsterChase;

    private void Awake()
    {
        GoToDoor = false;
        choiceNode = false;
        aggression = false;
        noiseCheck = false;
    }

    private void Start()
    {
        isAggressiveCoroutineRunning = false;
        attackisRunning = false;
        animator = GetComponent<Animator>();
        locationCheck = Vector3.zero;
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.destination = nodes[0].position;
        NavMeshAgent.speed = 0.8f;
        GoToDoor = false;
        choiceNode = false;
        aggression = false;
        noiseCheck = false;

        monsterBreathe.Play();
    }

    // Update is called once per frame
    void Update()
    {
        playerCurrentPos = PlayerTransform.position;

        if (GoToDoor == false && aggression == false)
        {
            if (choiceNode == false)
            {
                MoveToNode();
            }

            checkDistanceToNode();
        }

        if (Vector3.Distance(NavMeshAgent.transform.position, locationCheck) <= 1f && aggression == false)
        {
            NavMeshAgent.speed = 0.8f;
            StartCoroutine(stayAWhile());
            NavMeshAgent.destination = originalDest;
        }

        //I want to have a player transform distance check
        //I want to make the monster aggressive when it detects the player
        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) <= 2f && aggression == false && playerRB.velocity != Vector3.zero) //player moves when enemy is super near
        {
            if (isAggressiveCoroutineRunning == false)
            {
                StopAllCoroutines();
                StartCoroutine(aggressiveStart());
            }
        }

        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) <= 0.5f && aggression == false && playerRB.velocity == Vector3.zero) //when enemy is VERY super near
        {
            if (isAggressiveCoroutineRunning == false)
            {
                StopAllCoroutines();
                StartCoroutine(aggressiveStart());
            }
        }

        if (aggression)
        {
            NavMeshAgent.destination = playerCurrentPos;
        }

        //I want the monster to attack the player when close and when aggressive
        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) <= 1.5f && aggression)
        {
            if (attackisRunning == false && isAggressiveCoroutineRunning == false)
            {
                StopAllCoroutines();
                StartCoroutine(attackRest());
            }
        }

        //I want the monster to return to neutral state if player is no longer detected for 10 seconds
        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) >= 1f && aggression == true)
        {
            if (noiseCheck == true)
            {
                //remain angry
                aggression = true;

                //Reset calm timer
                timePassed = 0;
            }

            if (noiseCheck == false)
            {
                //Dont be angry
                timePassed += Time.deltaTime;
                Debug.Log("The time to calm: " + timePassed);

                if (timePassed >= 6f)
                {
                    aggression = false;
                    StartCoroutine(returnToNormal());
                }
            }
        }
        else
        {
            // Reset timer if conditions are no longer met
            timePassed = 0f;
        }
    }

    public void MoveToNode()
    {
        int i = UnityEngine.Random.Range(0, 2);
        originalDest = (NavMeshAgent.destination = nodes[i].position);
        choiceNode = true;
    }

    public void checkDistanceToNode()
    {
        if (Vector3.Distance(NavMeshAgent.transform.position, nodes[0].position) <= 0.5f)
        {
            originalDest = (NavMeshAgent.destination = nodes[1].position);
        }

        if (Vector3.Distance(NavMeshAgent.transform.position, nodes[1].position) <= 0.5f)
        {
            originalDest = (NavMeshAgent.destination = nodes[0].position);
        }
    }

    public void MoveToDoor(GameObject doorObj)
    {
        GoToDoor = true;
        doorObjCheck = doorObj;
        NavMeshAgent.destination = doorObj.transform.position;
    }

    public void arriveCheck(GameObject doorObj)
    {
        if (Vector3.Distance(NavMeshAgent.transform.position, doorObj.transform.position) <= 1.5f && doorObjCheck.name == doorObj.name)
        {
            StartCoroutine(stayAWhile());
            GoToDoor = false;
            NavMeshAgent.destination = originalDest;
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Noise") && aggression == false)
        {
            noiseCheck = true;
            locationCheck = collider.transform.position;
            NavMeshAgent.destination = locationCheck;
            StartCoroutine(suspicious());
        }

        if (collider.gameObject.CompareTag("Noise") && aggression == true)
        {
            noiseCheck = true;
            locationCheck = collider.transform.position;
            NavMeshAgent.destination = playerCurrentPos;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player") && aggression == false)
        {
            noiseCheck = true;
            locationCheck = other.transform.position;
            NavMeshAgent.destination = locationCheck;
            StopAllCoroutines();
            StartCoroutine(aggressiveStart());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Noise") && aggression == true)
        {
            noiseCheck = false;
        }

        if (other.gameObject.CompareTag("Noise") && aggression == false)
        {
            noiseCheck = false;
        }
    }

    IEnumerator returnToNormal()
    {
        aggression = false;
        NavMeshAgent.speed = 0f;
        animator.SetInteger("trigger", 2);

        monsterBreathe.Play();
        monsterChase.Stop();
        monsterScream.Stop();

        yield return new WaitForSeconds(3f);

        animator.SetInteger("trigger", 0);
        NavMeshAgent.speed = 0.8f;
        NavMeshAgent.destination = originalDest;
    }

    IEnumerator stayAWhile()
    {
        NavMeshAgent.speed = 0f;
        animator.SetInteger("trigger", 2);
        yield return new WaitForSeconds(3f);
        animator.SetInteger("trigger", 0);
        NavMeshAgent.speed = 0.8f;
    }

    IEnumerator suspicious()
    {
        NavMeshAgent.speed = 0f;
        animator.SetInteger("trigger", 1);
        yield return new WaitForSeconds(2f);
        animator.SetInteger("trigger", 0);
        NavMeshAgent.speed = 2.5f;
        NavMeshAgent.destination = locationCheck;
    }

    IEnumerator aggressiveStart()
    {
        monsterBreathe.Stop();
        monsterScream.Play();

        isAggressiveCoroutineRunning = true;
        animator.SetInteger("trigger", 3);
        aggression = true;
        NavMeshAgent.speed = 0f;

        yield return new WaitForSeconds(2f);

        monsterScream.Stop();
        monsterChase.Play();

        animator.SetInteger("trigger", 4);
        NavMeshAgent.speed = 3f;
        isAggressiveCoroutineRunning = false;
    }

    IEnumerator attackRest()
    {
        attackisRunning = true;
        NavMeshAgent.speed = 0f;
        
        animator.SetInteger("trigger", 5); //attack

        yield return new WaitForSeconds(1f);
        
        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) <= 2f) //check player near to give chance to dodge
        {
            playerHealth.damagedByEnemy();
        }
        if (Vector3.Distance(NavMeshAgent.transform.position, PlayerTransform.position) > 2f) //check player near to give chance to dodge
        {
            Debug.Log("Missed!");
        }

        monsterAttack.Play();

        yield return new WaitForSeconds(1f);

        monsterAttack.Stop();
        animator.SetInteger("trigger", 3); //scream
        monsterScream.Play();

        yield return new WaitForSeconds(2.5f);

        monsterScream.Stop();

        animator.SetInteger("trigger", 4); //run
        NavMeshAgent.speed = 3f; // chase
        attackisRunning = false;
    }
}
