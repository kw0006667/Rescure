using UnityEngine;
using System.Collections;

public class EnermyAI : MonoBehaviour
{
    // �i����
    public float fieldOfViewAngle = 110f;

    // ���C�t��
    public float PatrolSpeed = 2.0f;

    // ���d�ɶ�
    public float PatrolWaitTime = 1.0f;

    // �ت��a
    public Transform[] WayPoints;

    // ���a
    private GameObject player;

    // �O�_�Q����
    private bool playerInSight;

    // �ʱ��d��
    private SphereCollider col;

    // �ʵe����
    private Animator anim;

    // �O�_�Q�ݨ�
    public bool isSee;

    private float lookWeight;

    private NavMeshAgent nav;

    private int wayPointIndex;

    // �O�_����
    private bool isStop;

    public bool isTrueMemory = false;//�O�֬��u���O��
    public int memoryId = 0;//�O�йϤ��s��


    // A timer for the partrolWaitTime
    private float patrolTimer;

    // Use this for initialization
    void Start()
    {
        this.player = GameObject.FindGameObjectWithTag("Player");
        this.playerInSight = false;
        this.isSee = false;
        this.col = this.GetComponent<SphereCollider>();
        this.anim = this.GetComponent<Animator>();
        this.nav = this.GetComponent<NavMeshAgent>();
        this.lookWeight = 0.0f;
        this.wayPointIndex = 0;
        this.patrolTimer = 0.0f;
        this.isStop = true;
    }

    // Update is called once per frame
    void Update()
    {
        this.anim.SetLookAtWeight(this.lookWeight);

        this.Patrolling();
        if (this.isSee)
        {
            print("see");
            this.anim.SetLookAtPosition(this.player.transform.position + new Vector3(0, 1.5f, 0));
            this.lookWeight = Mathf.Lerp(this.lookWeight, 1.0f, Time.deltaTime * 3);
        }
        else
        {
            this.lookWeight = Mathf.Lerp(this.lookWeight, 0.0f, Time.deltaTime * 3);
        }
    }

    void FixedUpdate()
    {
        if (!this.isStop)
        {
            this.anim.SetFloat("Speed", 0.15f);
            //this.anim.SetFloat("Direction", 1.0f);
        }
        else
        {
            this.anim.SetFloat("Speed", 0.0f);
        }
    }

    void Patrolling()
    {
        this.nav.speed = this.PatrolSpeed;

        if (this.nav.remainingDistance < nav.stoppingDistance)
        {
            this.isStop = true;
            // Increment the timer.
            this.patrolTimer += Time.deltaTime;

            // If the timer exceeds the wait time...
            if (this.patrolTimer >= this.PatrolWaitTime)
            {
                // Increment the waypointIndex.
                if (this.wayPointIndex == this.WayPoints.Length - 1)
                    this.wayPointIndex = 0;
                else
                    this.wayPointIndex++;

                // Reset the timer.
                this.patrolTimer = 0;
            }
        }
        else
        {
            // If not near a destination, reset the timer.
            this.patrolTimer = 0;
            this.isStop = false;
        }

        // Set the destination to the patrolWaypoint
        this.nav.destination = this.WayPoints[this.wayPointIndex].position;
    }

    void OnTriggerStay(Collider other)
    {
        //print(other.name);
        if (other.gameObject == this.player)
        {
            this.playerInSight = false;

            // Create a vector from the enemy to the player and store the angle between
            Vector3 direction = other.transform.position - this.transform.position;
            float angle = Vector3.Angle(direction, this.transform.forward);

            if (angle < this.fieldOfViewAngle * 0.5f)
            {


                this.isSee = true;
                RaycastHit hit;

                if (Physics.Raycast(this.transform.position + this.transform.up, direction.normalized, out hit, this.col.radius))
                {
                    Debug.DrawRay(this.transform.position + this.transform.up, direction.normalized * this.col.radius, Color.red);
                    if (hit.collider.gameObject == this.player)
                    {
                        // The payer is in sight.
                        this.playerInSight = true;

                        // Set the last global sighting is the players current position.

                    }
                }
            }
            else
            {
                this.isSee = false;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == this.player)
        {
            this.playerInSight = false;
            this.isSee = false;
        }
    }


}
