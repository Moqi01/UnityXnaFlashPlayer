using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XnaVG;

public class DrawShape : MonoBehaviour {
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public VGMatrix PaintTransformation;
    public VGMatrix Projection;
    public VGMatrix Matrice;
    public Mesh mesh;
    private Material material;
    private Texture texture;
    private void Awake()
    {
        material = new Material(meshRenderer.material);
        meshRenderer.material = material;
    }
    // Use this for initialization
    void Start () {
        
        meshRenderer.sortingOrder = transform .GetSiblingIndex();
    }

    // Update is called once per frame
    void Update () {
		//if(!IsShow&& gameObject.activeSelf)
  //          SetShow(false);
  //      IsShow = false;
    }

    public void SetShow(bool isShow=true)
    {
        if(!isShow)
            gameObject.SetActive(false);
        else
            gameObject.SetActive(true);

        IsShow = isShow;
    }

    public void SetDraw(Mesh mesh,VGMatrix matrices,VGMatrix projection,Texture2D texture=null)
    {
        meshFilter.mesh = mesh;
        if (texture != null)
        {
            material.SetTexture("_MainTex", texture);
            material.SetFloat("_IsTex", 1);
        }
        else
        {
            material.SetFloat("_IsTex", 0);
        }

        Vector4 t = new Vector4(matrices.M31, matrices.M32, matrices.M33);
        Vector4 s = new Vector4(matrices.M11, matrices.M22, matrices.M33);
        Vector4 r = new Vector4(matrices.M21, matrices.M12);
        material.SetVector("_Transformation", t);
        material.SetVector("_Scale", s);
        material.SetVector("_Rotation", r);


        //Vector4 pt = new Vector4(projection.M31, projection.M32);
        ////Vector4 ps = new Vector4(projection.M11 * 1000, projection.M22 * 1000);
        //Vector4 ps = new Vector4(0.2f,-0.2f);
        //Vector4 pr = new Vector4(projection.M21, projection.M12);
        //material.SetVector("_ProjectionT", pt);
        //material.SetVector("_ProjectionS", ps);
        //material.SetVector("_ProjectionR", pr);

        SetShow(true);
    }

    public bool IsShow;
}
