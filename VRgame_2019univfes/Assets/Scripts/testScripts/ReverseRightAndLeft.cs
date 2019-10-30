using UnityEngine;
using System.ComponentModel;

public class ReverseRightAndLeft : MonoBehaviour{
    private Matrix4x4 mat;

    void Start () {
        mat = Camera.main.projectionMatrix * Matrix4x4.Scale(new Vector3(-1, 1, 1));
    }

    void OnPreCull() {
        Camera.main.ResetWorldToCameraMatrix();
        Camera.main.ResetProjectionMatrix();
        Camera.main.projectionMatrix = mat;
    }

    void OnPreRender() {
        GL.invertCulling = true;
    }

    void OnPostRender() {
        GL.invertCulling = false;
    }
}