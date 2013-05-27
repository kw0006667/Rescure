using UnityEngine;
using System.Collections;

// Require these components when using this script
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(SphereCollider))]
public class BotControlScript : MonoBehaviour
{
    [System.NonSerialized]
    public float lookWeight;					// the amount to transition when using head look

    //[System.NonSerialized]
    public Transform enemy;						// a transform to Lerp the camera to during head look

    public float animSpeed = 1.5f;				// a public setting for overall animator animation speed
    public float lookSmoother = 3f;				// a smoothing setting for camera motion
    public bool useCurves;						// a setting for teaching purposes to show use of curves


    private Animator anim;							// a reference to the animator on the character
    private AnimatorStateInfo currentBaseState;			// a reference to the current state of the animator, used for base layer
    private AnimatorStateInfo layer2CurrentState;	// a reference to the current state of the animator, used for layer 2
    private CapsuleCollider col;					// a reference to the capsule collider of the character


    static int idleState = Animator.StringToHash("Base Layer.Idle");
    static int locoState = Animator.StringToHash("Base Layer.Locomotion");			// these integers are references to our animator's states
    static int jumpState = Animator.StringToHash("Base Layer.Jump");				// and are used to check state for various actions to occur
    static int jumpDownState = Animator.StringToHash("Base Layer.JumpDown");		// within our FixedUpdate() function below
    static int fallState = Animator.StringToHash("Base Layer.Fall");
    static int rollState = Animator.StringToHash("Base Layer.Roll");
    static int waveState = Animator.StringToHash("Layer2.Wave");

    private Vector3 point;//紀錄滑鼠點下時的3D座標
    private float time;

    private float h, v; // animator's Speed, Direction

    private SphereCollider detectEnemy; //判斷敵人的範圍
    private float maxEnemyDistance = 5.0f; //範圍最大距離

    //[System.NonSerialized]
    public bool isEnemyInView; //敵人是否被眼睛我看見了
    public float playerViewAngle = 120.0f; //最大可視角度

    private EnermyAI eAI; //敵人腳本
    public bool isDie = false; //是否死亡

    public float memoryProgress = 0.0f; //讀取記憶進度
    private float memoryProgressMax = 3.0f; //最大讀取記憶進度
    public int memoryItem = 0; //得到正確記憶數量
    private int memoryItemMax = 4; //最大正確記憶數量
    public bool isCanGetMemory = true;

    void Start()
    {
        // initialising reference variables
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        //enemy = GameObject.Find("Enemy").transform;
        if (anim.layerCount == 2)
            anim.SetLayerWeight(1, 1);

        detectEnemy = gameObject.AddComponent("SphereCollider") as SphereCollider;
        //detectEnemy = GetComponent<SphereCollider>();
        detectEnemy.isTrigger = true;
        detectEnemy.radius = maxEnemyDistance;
        detectEnemy.center = new Vector3(0.0f, 0.9f, 0.0f);
        isEnemyInView = false;

        //eAI = enemy.root.GetComponent<EnermyAI>();
    }

    void Update()
    {
        if (Input.GetMouseButton(0) && !isDie)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) //當點到物體
            {
                point = hit.point;

                if (Vector3.Distance(new Vector3(point.x, 0.0f, point.z), new Vector3(transform.position.x, 0.0f, transform.position.z)) > 0.3f)
                {
                    transform.LookAt(new Vector3(point.x, transform.position.y, point.z));

                    //if (Time.realtimeSinceStartup - time <= 0.2f) //如果連續點擊
                    //{

                    //}
                    //else
                    //{
                    v = 0.4f;
                    //}

                    //time = Time.realtimeSinceStartup; //紀錄滑鼠點下時間
                }
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            v = 0;
        }
    }

    void FixedUpdate()
    {
        //h = Input.GetAxis("Horizontal");				// setup h variable as our horizontal input axis
        // v = Input.GetAxis("Vertical");				// setup v variables as our vertical input axis
        anim.SetFloat("Speed", v);							// set our animator's float parameter 'Speed' equal to the vertical input axis				
        anim.SetFloat("Direction", h); 						// set our animator's float parameter 'Direction' equal to the horizontal input axis		
        anim.speed = animSpeed;								// set the speed of our animator to the public variable 'animSpeed'
        anim.SetLookAtWeight(lookWeight);					// set the Look At Weight - amount to use look at IK vs using the head's animation
        currentBaseState = anim.GetCurrentAnimatorStateInfo(0);	// set our currentState variable to the current state of the Base Layer (0) of animation

        if (anim.layerCount == 2)
            layer2CurrentState = anim.GetCurrentAnimatorStateInfo(1);	// set our layer2CurrentState variable to the current state of the second Layer (1) of animation


        // LOOK AT ENEMY

        // if we hold Alt..
        if (Input.GetButton("Fire2") && isEnemyInView && enemy && !isDie && isCanGetMemory) //按下動作
        {
            if (eAI.isSee) //被敵人發現
                isDie = true;
            else
            {   // ...set a position to look at with the head, and use Lerp to smooth the look weight from animation to IK (see line 54)                
                anim.SetLookAtPosition(enemy.position);
                lookWeight = Mathf.Lerp(lookWeight, 1f, Time.deltaTime * lookSmoother);

                memoryProgress += Time.deltaTime;
                if (memoryProgress >= memoryProgressMax) //順利地得到記憶(不論正確否)
                {
                    isCanGetMemory = false;
                    if (eAI.isTrueMemory)
                    {
                        memoryItem++;
                        eAI.isTrueMemory = false;
                    }
                }
            }
        }        
        else if (isDie)
        {
        }
        else if (Input.GetButtonUp("Fire2"))
        {
            isCanGetMemory = true;
            memoryProgress = 0;
        }
        else // else, return to using animation for the head by lerping back to 0 for look at weight
        {            
            lookWeight = Mathf.Lerp(lookWeight, 0f, Time.deltaTime * lookSmoother);
        }

        if (memoryItem >= memoryItemMax) //若得到所有所需記憶
        { 
        }

        // STANDARD JUMPING

        // if we are currently in a state called Locomotion (see line 25), then allow Jump input (Space) to set the Jump bool parameter in the Animator to true
        if (currentBaseState.nameHash == locoState)
        {
            if (Input.GetButtonDown("Jump"))
            {
                anim.SetBool("Jump", true);
            }
        }

        // if we are in the jumping state... 
        else if (currentBaseState.nameHash == jumpState)
        {
            //  ..and not still in transition..
            if (!anim.IsInTransition(0))
            {
                if (useCurves)
                    // ..set the collider height to a float curve in the clip called ColliderHeight
                    col.height = anim.GetFloat("ColliderHeight");

                // reset the Jump bool so we can jump again, and so that the state does not loop 
                anim.SetBool("Jump", false);
            }

            // Raycast down from the center of the character.. 
            Ray ray = new Ray(transform.position + Vector3.up, -Vector3.up);
            RaycastHit hitInfo = new RaycastHit();

            if (Physics.Raycast(ray, out hitInfo))
            {
                // ..if distance to the ground is more than 1.75, use Match Target
                if (hitInfo.distance > 1.75f)
                {

                    // MatchTarget allows us to take over animation and smoothly transition our character towards a location - the hit point from the ray.
                    // Here we're telling the Root of the character to only be influenced on the Y axis (MatchTargetWeightMask) and only occur between 0.35 and 0.5
                    // of the timeline of our animation clip
                    anim.MatchTarget(hitInfo.point, Quaternion.identity, AvatarTarget.Root, new MatchTargetWeightMask(new Vector3(0, 1, 0), 0), 0.35f, 0.5f);
                }
            }
        }


        // JUMP DOWN AND ROLL 

        // if we are jumping down, set our Collider's Y position to the float curve from the animation clip - 
        // this is a slight lowering so that the collider hits the floor as the character extends his legs
        else if (currentBaseState.nameHash == jumpDownState)
        {
            col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);
        }

        // if we are falling, set our Grounded boolean to true when our character's root 
        // position is less that 0.6, this allows us to transition from fall into roll and run
        // we then set the Collider's Height equal to the float curve from the animation clip
        else if (currentBaseState.nameHash == fallState)
        {
            col.height = anim.GetFloat("ColliderHeight");
        }

        // if we are in the roll state and not in transition, set Collider Height to the float curve from the animation clip 
        // this ensures we are in a short spherical capsule height during the roll, so we can smash through the lower
        // boxes, and then extends the collider as we come out of the roll
        // we also moderate the Y position of the collider using another of these curves on line 128
        else if (currentBaseState.nameHash == rollState)
        {
            if (!anim.IsInTransition(0))
            {
                if (useCurves)
                    col.height = anim.GetFloat("ColliderHeight");

                col.center = new Vector3(0, anim.GetFloat("ColliderY"), 0);

            }
        }
        // IDLE

        // check if we are at idle, if so, let us Wave!
        else if (currentBaseState.nameHash == idleState)
        {
            if (Input.GetButtonUp("Jump"))
            {
                anim.SetBool("Wave", true);
            }
        }
        // if we enter the waving state, reset the bool to let us wave again in future
        if (layer2CurrentState.nameHash == waveState)
        {
            anim.SetBool("Wave", false);
        }
    }

    public Vector3 enemyTmp = new Vector3(100000, 100000, 100000);

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Enemy" && Vector3.Distance(other.gameObject.transform.position, transform.position) < maxEnemyDistance)
        {
            //print(other.gameObject.transform.position);
            
            Vector3 direction = new Vector3(other.gameObject.transform.position.x, 0.0f, other.gameObject.transform.position.z) - new Vector3(transform.position.x, 0.0f, transform.position.z);
            float angle = Vector3.Angle(direction, transform.forward);
            if (angle < playerViewAngle / 2)
            {
                time = 0;

                if (Vector3.Distance(other.gameObject.transform.position, transform.position) < Vector3.Distance(enemyTmp, transform.position))
                {
                    enemyTmp = other.gameObject.transform.position;
                    enemy = other.gameObject.transform;
                    eAI = enemy.root.GetComponent<EnermyAI>();
                    //direction = new Vector3(enemy.position.x, 0.0f, enemy.position.z) - new Vector3(transform.position.x, 0.0f, transform.position.z);

                    //Ray ray = new Ray(transform.position, direction);
                    //RaycastHit hitInfo = new RaycastHit();

                    //if (Physics.Raycast(ray, out hitInfo, detectEnemy.radius))
                    //{
                        isEnemyInView = true;
                    //}
                }
            }
            else
            {
                time += Time.deltaTime;
                if (time >= 1)
                    isEnemyInView = false;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            isEnemyInView = false;
            enemyTmp = new Vector3(100000, 100000, 100000);
        }
    }
}
