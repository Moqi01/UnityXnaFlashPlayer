using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XnaVG;
//using UnityEngine.UI;
using Microsoft.Xna.Framework.Graphics;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;
using XnaVG.Rendering.Tesselation;

public class DrawGL : MonoBehaviour
{
    public string[] PropertyNames;
    // Start is called before the first frame update
    void Start()
    {
        PropertyNames = Tmat.GetTexturePropertyNames();
    }
    private void Awake()
    {
        ins = this;
    }
    public static int MaxNum = 50000;
    Vector3[] ves = new Vector3[MaxNum];
    public Vector2[] uvs = new Vector2[12];
    int[] triangles = new int[MaxNum];
    Color[] colors = new Color[MaxNum];

    public static DrawGL ins;
    // Update is called once per frame
    void Update()
    {
        if (!isDrow)
        {
            for (int j = Meshs.Count; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(false);
            }
            //for (int j = transform.childCount-1; j >=0; j--)
            //{
            //   Destroy( transform.GetChild(j).gameObject);
            //}
            for (int j = 0; j < Meshs.Count; j++)
            {
                GameObject gameObject = null;
                Mesh mesh = Meshs[j];
                if (j < transform.childCount)
                {

                    gameObject = transform.GetChild(j).gameObject; gameObject.SetActive(true);
                    //return;
                    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                    meshFilter.mesh = mesh;
                }
                else
                {
                    gameObject = CreateObj();

                    MeshFilter meshFilter = gameObject.GetComponent<MeshFilter>();
                    meshFilter.mesh = mesh;

                }
                MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                if (Textures.ContainsKey(j + 1))
                {
                    //if(mr.material!=null)
                    //{
                    //    mr.material=
                    //}
                    Material material = new Material(Tmat);
                    material.SetTexture(PropertyNames[0], Textures[j + 1]);
                    mr.material = material;

                    //Tmat.SetTexture(PropertyNames[0], Textures[j + 1]);
                    //mr.material = Tmat;
                      if (mesh.vertexCount==12)
                        mesh.uv= uvs;
                }
                else
                    mr.material = mat;
                mr.sortingOrder = j;
            }
            return;
        }
        return;
        if (Vertors.Count <= 0) return;
        if (isGraphicDrow)
        {
            for (int j = 0; j < Vertors.Count; j++)
            {
                vertices = Vertors[j];
                if (vertices == null) continue;
                Matrix4x4 m = Matrix4x4.identity;
                if (j < Matrices.Count)
                    m = Matrices[j];
                
                for (int i = 0; i < vertices.Length; i++)
                {
                    //Vector2 v = (new Vector2(vertices[i].Position.X / vX - VMax, HMax - vertices[i].Position.Y / vY));
                    Vector3 v = new Vector3(vertices[i].Position.X, vertices[i].Position.Y);
                    Vector2 n = new Vector2(vertices[i].Control.X, vertices[i].Control.Y);

                    v = m.MultiplyPoint3x4(v);
                   

                    v *= Scale;
                    v *= UseScale;
                    ves[i] = v;
                    //uvs[i] = n;
                    if (j < Colors.Count)
                        colors[i] = Colors[j];
                    else
                        colors[i] = Color.white;
                    //colors[i] = Colors[j];
                    triangles[i] = i;
                     //Debug .DrawLine(v, v2, Colors[j]);
                }
                Mesh mesh = null;
                if (!isShow)
                {
                    GameObject gameObject = null;
                    if (j < transform.childCount)
                    {
                        gameObject = transform.GetChild(j).gameObject; gameObject.SetActive(true);
                        mesh = gameObject.GetComponent<MeshFilter>().mesh;
                        mesh.Clear();
                    }
                    else
                    {
                        gameObject = GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane);
                        gameObject.transform.SetParent(transform);
                        gameObject.transform.localPosition = new Vector3(400, 200, 1000);
                        gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        gameObject.transform.localScale = new Vector3(0.14f, 0.14f, 1f);
                        mesh = new Mesh();
                        gameObject.GetComponent<MeshFilter>().mesh = mesh;
                       
                    }
                    if (isDrow)
                    {
                       
                        //mesh.bindposes = new Matrix4x4[] { m };
                        //mesh.SetVertices(ves,0,vertices.Length);
                        ////mesh.SetUVs(0, uvs, 0, vertices.Length);
                        //mesh.SetTriangles(triangles, 0, vertices.Length,0);
                        //mesh.SetColors(colors, 0, vertices.Length);
                        //Graphics.DrawMeshInstanced(mesh, 0, mat, new Matrix4x4[] { Matrix4x4.identity });

                        //Graphics.DrawMeshNow(mesh, Matrix4x4.identity);

                        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
                        mr.material = mat;
                        mr.sortingOrder = j;

                    }

                }
            }
            for (int j = Vertors.Count; j < transform.childCount; j++)
            {
                transform.GetChild(j).gameObject.SetActive(false);
            }
            //isShow = true;
        }
    }
    // Draws 2 triangles in the left side of the screen
    // that look like a square
    public bool isShow;
    public   Material mat;
    public bool isDrow;
    public bool isGraphicDrow;
    List<Mesh> Meshs = new List<Mesh>();
    void OnPostRender()
    {
        
        if (!mat)
        {
            Debug.LogError("Please Assign a material on the inspector");
            return;
        }
        if (Vertors.Count <= 0) return;

        if(isGraphicDrow)
        {
          

        }
        else if(isDrow)
        {
            //GL.Flush();

            GL.PushMatrix();


            mat.SetPass(0);
            //GL.LoadPixelMatrix();
            GL.LoadOrtho();
            for (int j = 0; j < Vertors.Count; j++)
            {
                vertices = Vertors[j];
                if (vertices == null) continue;
                if (primitiveTypes[j] == PrimitiveType.TriangleList)
                {
                    GL.Begin(GL.TRIANGLE_STRIP);
                    if (j < Colors.Count)
                        GL.Color(Colors[j]);
                    //GL.MultMatrix(transform.localToWorldMatrix);
                    for (int i = 0; i < vertices.Length; i++)
                    {

                        DrawVer(vertices[i], j);
                    }
                }
                else if (primitiveTypes[j] == PrimitiveType.LineList)
                {

                }

                //int num = vertices.Length / 3;
                //for (int i = 0; i < num - 2; i += 3)

                //{
                //    int index = i;
                //    DrawVer(vertices[i]);
                //    DrawVer(vertices[i + 1]);

                //    DrawVer(vertices[i + 1]); 
                //    DrawVer(vertices[i + 2]);

                //    DrawVer(vertices[i + 2]); 
                //    DrawVer(vertices[i]);

                //}
                GL.End();

            }
            GL.PopMatrix();
        }
       

    }
    public GameObject CreateObj()
    {
        GameObject gameObject = GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane);
        gameObject.transform.SetParent(transform);
        gameObject.transform.localPosition = new Vector3(0, 0, 1);
        //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
        return gameObject;
    }
    internal void SetPrimitiveTypes(Microsoft.Xna.Framework.Graphics.PrimitiveType triangleList)
    {
        primitiveTypes.Add(triangleList);
    }
    public Material Tmat;
    internal void SetTextures(Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        if(!Textures.ContainsKey(Meshs.Count+1))
        Textures.Add(Meshs.Count+1, texture.UnityTexture);
    }

    public bool ContainsMesh(Mesh mesh)
    {
        return Meshs.Contains(mesh);
    }

    public void SetMesh(Mesh mesh)
    {
        if(Meshs.Contains(mesh))
        {

        }
        //if(mesh.vertices .Length==12)
        //{

        //}
        Meshs.Add(mesh);
    }

    public Dictionary<int, UnityEngine.Texture> Textures = new Dictionary<int, UnityEngine.Texture>();

    internal void SetMatrices(VGMatrix value,VGMatrix rot)
    {
        UnityEngine.Matrix4x4 v = UnityEngine.Matrix4x4.identity;
        v.m00 = value.M11;
        v.m10 = value.M12;
        //v.m13 = value.M13;
       
        v.m01 = value.M21;
        v.m11 = value.M22;
         //v.m23 = value.M23;
 
         v.m03 = value.M31;
         v.m13 = value.M32;
         //v.m33 = value.M33;
        Matrices.Add(v);

        //UnityEngine.Matrix4x4 r = UnityEngine.Matrix4x4.identity;
        //r.m00 = value.M11;
        //r.m01 = value.M12;

        //r.m10 = value.M21;
        //r.m11 = value.M22;
        Scale = rot.ScaleY*2000;
        //RotMatrices.Add(r);
        ScValues.Add(new Vector2(value.ScaleX, value.ScaleY));
    }

    void DrawVer(XnaVG.Rendering.Tesselation.StencilVertex vertices,int i)
    {
        Matrix4x4 m = Matrix4x4.identity;
        if (i<Matrices.Count)
            m = Matrices[i];
        //float x = vertices.Position.X * ScValues[i].x + m.m30;
        //float y = vertices.Position.Y * ScValues[i].y + m.m31;
        float x = vertices.Position.X ;
        float y = vertices.Position.Y ;

        Vector4 result =new Vector4(x,y,0,1);            result = m * result;        //float CosAngle = Mathf.Cos(RotValues[i]);        //float SinAngle = Mathf.Sin(RotValues[i]);        //float X = x * CosAngle + SinAngle * y;        //float Y = -x * SinAngle + CosAngle * y;

        float X = result.x;
        float Y = result.y;

        //Vector2 v = (new Vector2((X + vertices.Control.X) / vX - VMax,HMax- (Y + vertices.Control.Y) / vY));
        Vector2 v = (new Vector2(X / vX - VMax,HMax- Y / vY));
        v *= Scale;
        GL.Vertex(v);

    }
    public float Scale = 1;
    public float UseScale = 1;
    public XnaVG.Rendering.Tesselation.StencilVertex[] vertices;
    public float vX = 12 * Screen.width;
    public float vY = 12 * Screen.height;
    public float HMax = 1;
    public float VMax = 1;
    //public Vector3[] Vectors;
    public int VIndex;
    public List<Color> Colors = new List<Color>();
    public List<Matrix4x4> Matrices = new List<Matrix4x4>();
    public List<Matrix4x4> RotMatrices = new List<Matrix4x4>();
    public List<Vector2 > ScValues = new List<Vector2>();
    public List<Microsoft.Xna.Framework.Graphics.PrimitiveType> primitiveTypes = new List<Microsoft.Xna.Framework.Graphics.PrimitiveType>();
    public List<XnaVG.Rendering.Tesselation.StencilVertex[]> Vertors=new List<XnaVG.Rendering.Tesselation.StencilVertex[]>();

    public bool OpenStencil;

    public void SetVectors(XnaVG.Rendering.Tesselation.StencilVertex[] v)
    {
        if (v == null)
        {

        }
        else
            Vertors.Add(v);
    }
    public void SetVectors(Microsoft.Xna.Framework.Vector2[] v)
    {
        XnaVG.Rendering.Tesselation.StencilVertex[] vertices = new XnaVG.Rendering.Tesselation.StencilVertex[v.Length];
        for (int i = 0; i < v.Length; i++)
        {
            StencilVertex sv = new StencilVertex();
            sv.Set(v[i].X, v[i].Y, 0, 0);
            vertices[i] = sv;
        }
        Vertors.Add(vertices);
    }
    public void SetColor(Color c)
    {
        Colors.Add(c);
    }
    public void Clear(Color color)
    {
        Matrices.Clear();
        Vertors.Clear();
        Colors.Clear();
        RotMatrices.Clear();
        ScValues.Clear();
        primitiveTypes.Clear();
        Textures.Clear();
        Meshs.Clear();
        Camera.main.backgroundColor = color;
    }
}
