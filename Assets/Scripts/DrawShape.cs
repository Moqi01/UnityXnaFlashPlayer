using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Graphics;
using UnityEngine;
using XnaVG;
using Vector4 = UnityEngine.Vector4;

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

    public void SetDraw(Mesh mesh,VGMatrix transformation,VGMatrix projection, VGMatrix paintTransformation, Texture2D texture=null, VGCxForm cxForm=null)
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

        Vector4 t = new Vector4(transformation.M31, transformation.M32, transformation.M33);
        Vector4 s = new Vector4(transformation.M11, transformation.M22, transformation.M33);
        Vector4 r = new Vector4(transformation.M21, transformation.M12);
        material.SetVector("_Transformation", t);
        material.SetVector("_Scale", s);
        material.SetVector("_Rotation", r);
        material.SetVector("_AddTerm", new Vector4 ( cxForm.AddTerm.X, cxForm.AddTerm.Y, cxForm.AddTerm.Z, cxForm.AddTerm.W));
        material.SetVector("_MulTerm", new Vector4 ( cxForm.MulTerm.X, cxForm.MulTerm.Y, cxForm.MulTerm.Z, cxForm.MulTerm.W));

        ////Vector4 ps = new Vector4(projection.M11 * 1000, projection.M22 * 1000);
        //Vector4 pt = new Vector4(projection.M31, projection.M32);
        //Vector4 ps = new Vector4(0.2f,-0.2f);
        //Vector4 pr = new Vector4(projection.M21, projection.M12);
        //material.SetVector("_ProjectionT", pt);
        //material.SetVector("_ProjectionS", ps);
        //material.SetVector("_ProjectionR", pr);

        Vector4 ptt = new Vector4(paintTransformation.M31, paintTransformation.M32, paintTransformation.M33);
        Vector4 pts = new Vector4(paintTransformation.M11, paintTransformation.M22, paintTransformation.M33);
        Vector4 ptr = new Vector4(paintTransformation.M21, paintTransformation.M12);
        material.SetVector("_PaintTransformationT", ptt);
        material.SetVector("_PaintTransformationS", pts);
        material.SetVector("_PaintTransformationR", ptr);

        SetShow(true);
    }

    public bool IsShow;

    public void SetDrawMask(Texture2D texture, Microsoft.Xna.Framework.Vector4 maskChannels)
    {
            material.SetTexture("_Mask", texture);
            material.SetVector("_MaskChannels", maskChannels.ToVector4());
    }

    internal void SetBlendState(Microsoft.Xna.Framework.Graphics.BlendState blendState)
    {
        material.SetInt("_BlendOp", (int)blendState.AlphaBlendFunction-1);
        material.SetInt("_SrcBlend", (int)blendState.AlphaSourceBlend-1);
        material.SetInt("_DstBlend", (int)blendState.AlphaDestinationBlend-1);
    }
}
