using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Draw : Graphic
{
    public int Count;
    public Material NormalMaterial;
    public Material FontMaterial;
    public Material CurrentMaterial;
    public float a;
    public int index;
    public Texture2D PixelsTexture;
    public Texture2D PaletteTexture;
    // Start is called before the first frame update
    public void InitMaterial()
    {
        NormalMaterial = new Material(Shader.Find("Unlit/UnlitShader"));
        FontMaterial = new Material(Shader.Find("Unlit/FontUnlitShader"));
        CurrentMaterial=new Material(Shader.Find("Unlit/NewUnlitShader"));
        isDrawOver = true;
        this.material = NormalMaterial;
        //this.material = CurrentMaterial;
        index = transform.GetSiblingIndex();
    }

    public void SetMat(int Id)
    {
        this.material =Id==0? NormalMaterial: FontMaterial;
        //material.SetPass(Id);
    }

    public void SetTexture(string  name, Texture2D texture)
    {
        if (name == "xPixels")
        {
            if (PixelsTexture != texture)
            {
                PixelsTexture = texture;
                material.SetTexture("xPixels", PixelsTexture);
            }
        }
        else
        {
            if (PaletteTexture != texture)
            {
                PaletteTexture = texture;
                material.SetTexture("xPalette", PaletteTexture);
            }
        }
    }

    public List<UIVertex> targetVertexList = new List<UIVertex>();

    //public xnaMugen.Video.Vertex[] VertexData = new xnaMugen.Video.Vertex[500];

    //public void SetVertexData(xnaMugen.Video.Vertex[] VertexData)
    //{
    //    targetVertexList.Clear();
    //    int triangleCount = Count * 3;

    //    for (int i = 0; i < triangleCount; i++)
    //    {
    //        UIVertex vertex = new UIVertex();
    //        int v = i;

    //        vertex.position = new Vector3(VertexData[v].Position.X, VertexData[v].Position.Y, VertexData[v].Position.Z);
    //        vertex.uv0 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
    //        //vertex.uv1 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
    //        //vertex.uv2 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
    //        //vertex.uv3 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
    //        vertex.color = new Color32(VertexData[v].Tint.R, VertexData[v].Tint.G, VertexData[v].Tint.B, VertexData[v].Tint.A);
    //        //if (v < obj.Count)
    //        //    obj[v].transform.position = vertex.position;
    //        targetVertexList.Add(vertex);

    //        //if (v >= targetVertexList.Count)
    //        //    {
    //        //        targetVertexList.Add(vertex);
    //        //    }
    //        //    else
    //        //    {
    //        //        targetVertexList[i] = (vertex);
    //        //    }

    //    }
    //    //if(triangleCount < targetVertexList.Count)
    //    //{
    //    //    int v = targetVertexList.Count - triangleCount;
    //    //    for (int i = 0; i < v; i++)
    //    //    {
    //    //        targetVertexList.Remove(targetVertexList[targetVertexList.Count-i-1]);
    //    //    }
    //    //}
    //}

    public void SetDeviceReset(Matrix4x4 value,Matrix4x4 value2,float xBlendWeight)
    {
        NormalMaterial.SetMatrix("xMatrix", value);
        NormalMaterial.SetMatrix("xRotation", value2);
        NormalMaterial.SetFloat("xBlendWeight", xBlendWeight);
        FontMaterial.SetMatrix("xMatrix", value);
        FontMaterial.SetMatrix("xRotation", value2);
        FontMaterial.SetFloat("xBlendWeight", xBlendWeight);
    }

    public bool isDrawOver=true;
    public bool Screenshot = false;
    public List<Vector3> Vectors = new List<Vector3>();

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vh)
    {
        vh.Clear();
        foreach (var item in pos)
        {
            UIVertex uIVertex = new UIVertex();
            uIVertex.position = item * Scale;
            uIVertex.color = Color.blue;
            //vh.AddVert(item, colors,Vector2.zero);
            vh.AddVert(uIVertex);
        }
        base.OnPopulateMesh(vh);

    }

    //   protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vh)
    //   {
    //       //if (!XnaMugen.isOpenGlDraw)
    //       //{
    //       //    //base.OnPopulateMesh(vh);
    //       //    vh.Clear();
    //       //    SetVertexData(VertexData);
    //       //    //vh.AddUIVertexQuad(targetVertexList.ToArray ());
    //       //    vh.AddUIVertexTriangleStream(targetVertexList);
    //       //    //vh.AddUIVertexStream(targetVertexList, new List<int>() {0,1,2,2,3,0});
    //       //    isDrawOver = true;
    //       //}
    //       base.OnPopulateMesh(vh);
    //       targetVertexList.Clear();
    //       vh.Clear();
    //       points.Clear();
    //       Vector3 befor = Vector3.zero;
    //       for (int i = 0; i < Vectors.Count; i++)
    //       {
    //           if (!points.Contains(Vectors[i]))
    //           {

    //               points.Add(Vectors[i]);
    //           }
    //           if(befor.Equals(Vectors[i]))
    //               points.Add(Vectors[i]*0.99f);

    //           befor = Vectors[i];
    //       }
    ////       if (points.Count >0)
    ////       {
    ////points.Insert(4, points[0] * 0.9f);
    ////       points.Insert(points.Count, points[points.Count - 1]*0.99f);
    ////       }

    //       for (int i = 0; i < points.Count; i++)
    //       {
    //           UIVertex uIVertex = new UIVertex();
    //           uIVertex.position = points[i] * Scale;
    //           uIVertex.color = Color.blue;

    //               targetVertexList.Add(uIVertex);

    //       }


    //       if (targetVertexList.Count >0)
    //       {

    //       indes = new Triangulator(points.ToArray()).Triangulate();
    //       List<int> ins = new List<int>(indes);


    //       ins.AddRange(AddIndes);
    //           vh.AddUIVertexStream(targetVertexList,ins);
    //       //vh.AddUIVertexTriangleStream(targetVertexList);
    //       }
    //       if (Screenshot)
    //       {
    //           Microsoft.Xna.Framework.Graphics.Texture2D. OutputRt(PixelsTexture.EncodeToPNG(), "Pixels" + transform.GetSiblingIndex ());
    //           Microsoft.Xna.Framework.Graphics.Texture2D. OutputRt(PaletteTexture.EncodeToPNG(), "Palette" + transform.GetSiblingIndex ());
    //           Screenshot = false;
    //       }
    //   }
    public float Scale = 0.1f;
    public List < Vector3> pos =new List<Vector3>();
    public Color colors;
    public List<Vector2> points = new List<Vector2>();
    public int[] indes;
    public List< int> AddIndes=new List<int>();
}
