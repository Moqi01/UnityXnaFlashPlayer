using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XnaVG;
using UnityEngine.UI;
using Microsoft.Xna.Framework.Graphics;
using PrimitiveType = Microsoft.Xna.Framework.Graphics.PrimitiveType;
using XnaVG.Rendering.Tesselation;
using Unity.VectorGraphics;

public class DrawGL : MonoBehaviour
{
    public List<DrawShape> drawShapes = new List<DrawShape>();
    public bool isNewMeshMake;
    public int Index;

    public DrawShape drawShapePrefab;
    public string[] PropertyNames;
    public Text FileNameText;
    public Material mat;
    public Material Tmat;

    public UnityEngine.Transform P;
    public Matrix4x4[] matrices;
    public static DrawGL ins;

    List<VGMatrix> PaintTransformations = new List<VGMatrix>();
    List<VGMatrix> Projections = new List<VGMatrix>();
    List<Material> MaterialTemp = new List<Material>();

    List<Mesh> Meshs = new List<Mesh>();
    public List<Vector2> uvs;

    private MaterialPropertyBlock block;

    private void Awake()
    {
        ins = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        PropertyNames = Tmat.GetTexturePropertyNames();
        block = new MaterialPropertyBlock();
        matrices = new Matrix4x4[] { Matrix4x4.identity };

        foreach (UnityEngine.Transform t in P)
        {
            DrawShape d = t.GetComponent<DrawShape>();
            drawShapes.Add(d);
        }
    }
   
    public GameObject CreateObj()
    {
        GameObject gameObject = GameObject.CreatePrimitive(UnityEngine.PrimitiveType.Plane);
        Destroy(gameObject.GetComponent<MeshCollider>());
        gameObject.transform.SetParent(P);
        gameObject.transform.localPosition = new Vector3(0, 0, 1);
        MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
        mr.material = mat;
        //gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
        //gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
        //gameObject.name = gameObject.transform.GetSiblingIndex().ToString();
        return gameObject;
    }
    internal void SetPrimitiveTypes(Microsoft.Xna.Framework.Graphics.PrimitiveType triangleList)
    {
        //primitiveTypes.Add(triangleList);
    }

    internal void SetTextures(Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        //if (isNewDraw)
        //    Textures.Add(Meshs.Count, texture.UnityTexture);
        //else
        //{
        //    //if (!Textures.ContainsKey(Meshs.Count ))
        //        Textures.Add(Meshs.Count , texture.UnityTexture);
        //}
     
    }

    public bool ContainsMesh(Mesh mesh)
    {
        return Meshs.Contains(mesh);
    }

    public void SetMesh(Mesh mesh)
    {
        //Meshs.Add(mesh);
    }

    public Mesh SetMesh(List<Shape> shapes, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
        var node = new SceneNode();
        node.Children = null;
        //List<Shape> NewShapes = new List<Shape>();
        

        node.Transform =Matrix2D.identity;
        //node.Transform = Matrices[Matrices.Count-1];
        bool isTextureFill = false;
        if (texture!=null)
        foreach (var shape in shapes)
        {
            if (shape.Fill is TextureFill)
            {
                var fill = shape.Fill as TextureFill;
                fill.Texture = texture.unityTexture2D;
                    isTextureFill = true;
            }
        }
       
        node.Shapes = shapes;
        node.Transform = Matrix2D.identity;
        node.Clipper = null;

        // Create Scene
        var scene = new Scene();
        scene.Root = node;

        // Create Tessellation Options
        var options = new VectorUtils.TessellationOptions();
        options.MaxCordDeviation = float.MaxValue;
        options.MaxTanAngleDeviation = 0.1f;
        options.SamplingStepSize = 0.01f;
        options.StepDistance = float.MaxValue;

        // Tessellate
        try
        {
            var geometry = VectorUtils.TessellateScene(scene, options);
            if (isTextureFill)
                VectorUtils.GenerateAtlasAndFillUVs(geometry, 32);
            // Debug Meshs

            var mesh = new Mesh();
            //mesh.Clear();
            VectorUtils.FillMesh(mesh, geometry, 1f);

            //mesh.UploadMeshData(true);
            SetMesh(mesh);
           
            //mesh.name = PlaceObject.CharacterID.ToString();
            return mesh;
            //UnityEditor.AssetDatabase.CreateAsset(mesh, "Assets/Vectors/" + "ttt.asset");

        }
        catch (System.Exception exc)
        {

            Debug.LogException(exc);
            return null;
        }
    }

    public void SetFileText(string fileName)
    {
        FileNameText.text = fileName;
        Debug.Log(fileName);
    }

    public Dictionary<int, UnityEngine.Texture> Textures = new Dictionary<int, UnityEngine.Texture>();

    public Mesh SetMesh(Shape shape, Microsoft.Xna.Framework.Graphics.Texture2D texture)
    {
       return SetMesh(new List<Shape>() { shape }, texture);
    }

    //public StencilVertex[] vertices;
    public List<Color> Colors = new List<Color>();
    //public List<Matrix4x4> Matrices = new List<Matrix4x4>();
    //public List<Matrix4x4> RotMatrices = new List<Matrix4x4>();
    //public List<Vector2 > ScValues = new List<Vector2>();
    //public List<Microsoft.Xna.Framework.Graphics.PrimitiveType> primitiveTypes = new List<Microsoft.Xna.Framework.Graphics.PrimitiveType>();
    public List<StencilVertex[]> Vertors=new List<StencilVertex[]>();
    //public List<int> Depths = new List<int>();
    public bool OpenStencil;

    public void SetVectors(XnaVG.Rendering.Tesselation.StencilVertex[] v)
    {
        if (v == null)
        {
            Debug.LogWarning("StencilVertex null");
        }
        else
            Vertors.Add(v);
    }
    public void SetVectors(Microsoft.Xna.Framework.Vector2[] v)
    {
        StencilVertex[] vertices = new StencilVertex[v.Length];
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
        //OnDraw();
        
        Camera.main.backgroundColor = color;
        DrawGame.instance.index = 0;

        for (int i = Index; i < drawShapes.Count; i++)
        {
            drawShapes[i].SetShow(false);
        }
        Index = 0;
    }


    public void SetDrawShape(Mesh mesh, VGMatrix matrices, VGMatrix projection, Microsoft.Xna.Framework.Graphics.Texture2D texture = null, VGCxForm cxForm=null)
    {
        
        DrawShape drawShape;
        if (Index >= drawShapes.Count)
        {
            drawShape = Instantiate(drawShapePrefab, P);
            drawShapes.Add(drawShape);
        }
        else
            drawShape = drawShapes[Index];
        Index++;
        if (texture != null)
            drawShape.SetDraw(mesh, matrices, projection, texture.unityTexture2D, cxForm);
        else
            drawShape.SetDraw(mesh, matrices, projection,null, cxForm);
    }
}
