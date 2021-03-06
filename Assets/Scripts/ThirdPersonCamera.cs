using UnityEngine;
using System.Collections;

public class ThirdPersonCamera : MonoBehaviour
{
    public float smooth = 3f;		// a public variable to adjust smoothing of camera motion
    Transform standardPos;			// the usual position for the camera, specified by a transform in the game
    Transform lookAtPos;			// the position to move the camera to when using head look

    public Transform character;

    private BotControlScript botCtrl;	// control script

    void Start()
    {
        // initialising references
        standardPos = GameObject.Find("CamPos").transform;

        if (GameObject.Find("LookAtPos"))
            lookAtPos = GameObject.Find("LookAtPos").transform;

        // finding the BotControlScript on the root parent of the character
        botCtrl = character.root.GetComponent<BotControlScript>();
    }

    void Update()
    {

    }

    void FixedUpdate()
    {
        // if we hold Alt
        if (Input.GetButton("Fire2") && lookAtPos && botCtrl.isEnemyInView && !botCtrl.isDie && botCtrl.isCanGetMemory)
        {
            // lerp the camera position to the look at position, and lerp its forward direction to match 
            transform.position = Vector3.Lerp(transform.position, lookAtPos.position, Time.deltaTime * smooth);
            //transform.forward = Vector3.Lerp(transform.forward, lookAtPos.forward, Time.deltaTime * smooth);
            transform.LookAt(botCtrl.enemy.position + Vector3.up * 1.6f);
        }
        else
        {
            // return the camera to standard position and direction
            //transform.position = Vector3.Lerp(transform.position, standardPos.position, Time.deltaTime * smooth);
            //transform.forward = Vector3.Lerp(transform.forward, standardPos.forward, Time.deltaTime * smooth);
            transform.LookAt(new Vector3(character.position.x, 0.0f, character.position.z));
            transform.position = Vector3.Lerp(transform.position, character.position + new Vector3(-3.5f, 5.0f, 0.0f).normalized * 10, Time.deltaTime * smooth);
        }

    }

    bool isBehindEnermy()
    {
        return false;
    }
}
