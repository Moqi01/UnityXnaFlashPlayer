using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DrawGame : MonoBehaviour
{
    public List<GameObject> obj = new List<GameObject>();
    public static DrawGame instance;
    public Material material;
    public Material FontMaterial;
    public string GameName= "kfm";
    public static string GameFlieName;
    //public Draw draw;
    public Material MeshMaterial;
    public RawImage image;
    public RawImage FontImage;
    public RawImage xPaletteImage;

    public int Id;

    public List<Draw> draws = new List<Draw>();
    public UnityEngine.Transform DrawP;
    public int index;
    public List<string> Name=new List<string> ();
    private void Awake()
    {
        instance = this;
        GameFlieName = GameName;
       
        foreach (UnityEngine.Transform t in DrawP)
        {
            Draw d= t.GetComponent<Draw>();
            draws.Add(d);
            d.InitMaterial();
        }
        //material.GetTexturePropertyNames(Name);
        GlDraws.Clear();
    }

    public void Refresh()
    {

        for (int i=0;i< draws.Count;i++)
        {
            draws[i].gameObject.SetActive((draws[i].index < index));
        }
        index = 0;
    }

    public void DeviceReset()
    {
        for (int i = 0; i < draws.Count; i++)
        {
            draws[i].SetDeviceReset(xMatrix, xRotation, xBlendWeight);
        }
    }

    public void SetIntValue(string name,int value)
    {
        
            draws[index].material.SetInt(name, value);
    }
    public float xBlendWeight;
    public void SetFloatValue(string name, float value)
    {

         draws[index].material.SetFloat(name, value);
        if (name == "xBlendWeight")
        {
            xBlendWeight = value;
        }
    }

    public void SetTexture(string name, Texture2D value)
    {

            draws[index].SetTexture(name , value);
    }

    public void SetVector2(string name, Vector4 value)
    {
       
            draws[index].material.SetVector(name, value);

    }

    public Matrix4x4 xMatrix;
    public Matrix4x4 xRotation;

    public void SetM(string name, Matrix4x4 value)
    {
       
        draws[index].material.SetMatrix(name, value);
        if(name == "xMatrix")
        {
            xMatrix = value;
        }
        if (name == "xRotation")
        {
            xRotation = value;
        }
    }

    public void SetPass(int value)
    {
        Id = value;

        draws[index].SetMat(value);

    }

    public int CurrentIndex;

    //public void DrawUserPrimitives(Microsoft.Xna.Framework.Graphics.PrimitiveType primitiveType, xnaMugen.Video.Vertex[] vertexData, int vertexOffset, int primitiveCount, Microsoft.Xna.Framework.Graphics.VertexDeclaration vertexDeclaration)
    //{
    //    VertexData = vertexData;
    //    Count = primitiveCount;
    //    if (draws[index].isDrawOver)
    //    {
    //        draws[index].Count = primitiveCount;
    //        //draws[index].SetVertexData(VertexData);

    //        for (int i = 0; i < primitiveCount * 3; i++)
    //        {
    //            draws[index].VertexData[i] = (vertexData[i]);
    //        }
    //        draws[index].isDrawOver = false;
    //        draws[index].SetMaterialDirty();
    //        draws[index].SetVerticesDirty();
    //    }
    //    GlDraws.Add(draws[index]);
    //    index++;
    //    //DrawMesh();
    //}

    public List<Draw> GlDraws = new List<Draw>();

    public List<Vector3> points = new List<Vector3>();

    private void DrawMesh()
    {

        VertexHelper vh = new VertexHelper();
        List<UIVertex> targetVertexList = new List<UIVertex>();
        int triangleCount = Count * 3;

        for (int i = 0; i < triangleCount; i++)
        {
            UIVertex vertex = new UIVertex();

            int v = i;
            //vertex.position = new Vector3(VertexData[v].Position.X, VertexData[v].Position.Y, VertexData[v].Position.Z);
            //vertex.uv0 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
            //vertex.color = new Color32(VertexData[v].Tint.R, VertexData[v].Tint.G, VertexData[v].Tint.B, VertexData[v].Tint.A);
           
            targetVertexList.Add(vertex);
           
        }
        vh.AddUIVertexTriangleStream(targetVertexList);

        Mesh mesh = new Mesh();
        vh.FillMesh(mesh);

        //Mesh mesh = GetComponent<MeshFilter>().mesh;
        //mesh.Clear();
       
        mesh.RecalculateNormals();
       // meshFilter.mesh = mesh;
       
    }

    //public xnaMugen.Video.Vertex[] VertexData=new xnaMugen.Video.Vertex[6];
    public int Count;

    public Vector2[] vertexList = new Vector2[5];
    //雷达图的半径
    public float radius = 5f;

    public float[] diameters = new float[5];
}
