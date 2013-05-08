using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class RenderToCubeSharp : MonoBehaviour
{
    public int CubeMapSize = 128;
    public bool OneFacePerFrame = false;
    public float FarClipPlane = 100;
    public MaterialSelect MaterialChoice;

    private Camera cam;
    private RenderTexture rtex;

    public enum MaterialSelect
    {
        Glass = 0,
        Floor = 1
    }

    // Use this for initialization
    void Start()
    {
        // render all six faces at startup
        this.UpdateCubemap(63);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        if (this.OneFacePerFrame)
        {
            int faceToRender = Time.frameCount % 6;
            int faceMask = 1 << faceToRender;
            this.UpdateCubemap(faceMask);
        }
        else
        {
            this.UpdateCubemap(63);
        }
    }

    void UpdateCubemap(int faceMask)
    {
        if (!this.cam)
        {
            GameObject go = new GameObject("CubemapCamera");
            go.AddComponent<Camera>();
            go.hideFlags = HideFlags.HideAndDontSave;
            go.transform.position = this.transform.position;
            go.transform.rotation = Quaternion.identity;
            this.cam = go.camera;
            this.cam.farClipPlane = this.FarClipPlane; // don't render very far into cubemap
            this.cam.enabled = false;
        }

        if (!this.rtex)
        {
            this.rtex = new RenderTexture(this.CubeMapSize, this.CubeMapSize, 24);
            this.rtex.isCubemap = true;
            rtex.hideFlags = HideFlags.HideAndDontSave;
            switch (this.MaterialChoice)
            {
                case MaterialSelect.Glass:
                    this.renderer.sharedMaterial.SetTexture("_CubeTex", this.rtex);
                    break;
                case MaterialSelect.Floor:
                    this.renderer.sharedMaterial.SetTexture("_Cube", this.rtex);
                    break;
                default:
                    break;
            }
            
        }
        this.cam.transform.position = this.transform.position;
        this.cam.RenderToCubemap(this.rtex, faceMask);
    }

    void OnDisable()
    {
        DestroyImmediate(this.cam);
        DestroyImmediate(this.rtex);
    }
}
