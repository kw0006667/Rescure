using UnityEngine;
using System.Collections;

public class TransparentInView : MonoBehaviour
{
    public GameObject Player;
    public LayerMask Mask;

    private RaycastHit hit;
    private Ray ray;
    private Camera mainCamera;
    private Shader diffuseShader;
    private Shader transparentShader;

    private GameObject tempObject;
    private Material tempMat;
    // Use this for initialization
    void Start()
    {
        this.mainCamera = this.camera;
        this.diffuseShader = Shader.Find("Diffuse");
        this.transparentShader = Shader.Find("Transparent/Diffuse");
        this.tempObject = null;
        this.tempMat = null;
    }

    // Update is called once per frame
    void Update()
    {
        //ray = new Ray(this.mainCamera.transform.position, (this.Player.transform.position - this.mainCamera.transform.position));
        float distance = Vector3.Distance(this.Player.transform.position, this.mainCamera.transform.position);
        if (Physics.Linecast(this.Player.transform.position, this.mainCamera.transform.position, out this.hit, this.Mask) )
        {
            print(this.hit.collider.name);
            if (!this.hit.collider.gameObject.Equals(this.Player))
            {
                print(this.hit.collider.name);
                //Color col = this.hit.collider.gameObject.renderer.material.GetColor("_Color");
                Component r = this.hit.collider.gameObject.GetComponent<Renderer>();
                this.tempMat = ((Renderer)r).material;
                ((Renderer)r).material.shader = this.transparentShader;
                this.tempObject = this.hit.collider.gameObject;
            }

            if (this.tempObject != this.hit.collider.gameObject)
            {
                this.tempObject.renderer.material = this.tempMat;
            }
        }
        this.hit = new RaycastHit();
        Debug.DrawRay(this.mainCamera.transform.position, (this.Player.transform.position - this.mainCamera.transform.position).normalized * (distance - 0.5f), Color.red);
    }
}
