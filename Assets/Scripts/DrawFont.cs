using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawFont : Graphic
{
    //public xnaMugen.Video.Vertex[] VertexData = new xnaMugen.Video.Vertex[6];
    public int Count;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    protected override void OnPopulateMesh(UnityEngine.UI.VertexHelper vh)
    {

        base.OnPopulateMesh(vh);
        vh.Clear();

        List<UIVertex> targetVertexList = new List<UIVertex>();
        int triangleCount = Count;

        for (int i = 0; i < triangleCount; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                UIVertex vertex = new UIVertex();
                int v = i * 3 + j;

                //vertex.position = new Vector3(VertexData[v].Position.X, VertexData[v].Position.Y, VertexData[v].Position.Z);
                //vertex.uv0 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
                ////vertex.uv1 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y); 
                ////vertex.uv2 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y); 
                ////vertex.uv3 = new Vector2(VertexData[v].TextureCoordinate.X, VertexData[v].TextureCoordinate.Y);
                //vertex.color = new Color32(VertexData[v].Tint.R, VertexData[v].Tint.G, VertexData[v].Tint.B, VertexData[v].Tint.A);
                //if (v < obj.Count)
                //    obj[v].transform.position = vertex.position;
                targetVertexList.Add(vertex);
            }
        }
        //vh.AddUIVertexQuad(targetVertexList.ToArray ());
        vh.AddUIVertexTriangleStream(targetVertexList);
        //vh.AddUIVertexStream(targetVertexList, new List<int>() {0,1,2,2,3,0});
    }
}
