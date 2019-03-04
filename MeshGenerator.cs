using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace VRUtils.MeshToy
{
public class MeshGenerator : MonoBehaviour
{
    [SerializeField] private Material meshMat;
    [SerializeField] private Material controllerDefaultMat;
    [SerializeField] private Material controllerSelectedMat;
    [SerializeField] private float meshScale = 1.0f;
    
    private readonly List<VertexControllerBase> pointList = new List<VertexControllerBase>();
    public Mesh Mesh { get; private set; }

    private readonly List<Vector2> uvs = new List<Vector2>()
    {
        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),

        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),

        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(0, 1),
        new Vector2(1, 1),

        new Vector2(0, 0),
        new Vector2(0, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),

        new Vector2(0, 0),
        new Vector2(0, 0),
        new Vector2(1, 0),
        new Vector2(0, 0),

        new Vector2(1, 0),
        new Vector2(1, 1),
        new Vector2(1, 1),
        new Vector2(1, 0),
    };

    private void Start()
    {
        transform.localScale = Vector3.one;
        Mesh = new Mesh();
        gameObject.AddComponent<MeshFilter>().sharedMesh = Mesh;
        gameObject.AddComponent<MeshRenderer>().material = meshMat;
        CreatePrimitiveCubeMesh();
    }

    private void CreatePrimitiveCubeMesh()
    {
        var l = meshScale / 2;

        var vertices = new List<Vector3>()
        {
            new Vector3(-l, -l, -l),
            new Vector3(-l, l, -l),
            new Vector3(l, l, -l),
            new Vector3(l, -l, -l),
            new Vector3(-l, -l, l),
            new Vector3(-l, l, l),
            new Vector3(l, l, l),
            new Vector3(l, -l, l),

            new Vector3(-l, -l, -l),
            new Vector3(-l, l, -l),
            new Vector3(l, l, -l),
            new Vector3(l, -l, -l),
            new Vector3(-l, -l, l),
            new Vector3(-l, l, l),
            new Vector3(l, l, l),
            new Vector3(l, -l, l),

            new Vector3(-l, -l, -l),
            new Vector3(-l, l, -l),
            new Vector3(l, l, -l),
            new Vector3(l, -l, -l),
            new Vector3(-l, -l, l),
            new Vector3(-l, l, l),
            new Vector3(l, l, l),
            new Vector3(l, -l, l),
        };

        var triangles = new List<int>()
        {
            0, 1, 2,
            2, 3, 0,

            5, 4, 7,
            7, 6, 5,

            12, 8, 11,
            11, 15, 12,

            21, 9, 16,
            16, 20, 21,

            23, 19, 10,
            10, 22, 23,

            17, 13, 14,
            14, 18, 17,
        };

        for (int i = 0; i < triangles.Count; i++)
        {
            triangles[i] += Mesh.vertexCount;
        }

        for (int i = 0; i < 8; i++)
        {
            pointList.Add(CreatePointController(i));
        }

        CreateEdgeController(0, 1);
        CreateEdgeController(1, 2);
        CreateEdgeController(2, 3);
        CreateEdgeController(3, 0);

        CreateEdgeController(0, 4);
        CreateEdgeController(1, 5);
        CreateEdgeController(2, 6);
        CreateEdgeController(3, 7);

        CreateEdgeController(5, 4);
        CreateEdgeController(4, 7);
        CreateEdgeController(7, 6);
        CreateEdgeController(6, 5);

        CreateSurfaceController(3, 2, 1, 0);
        CreateSurfaceController(7, 3, 0, 4);
        CreateSurfaceController(1, 5, 4, 0);
        CreateSurfaceController(2, 6, 5, 1);
        CreateSurfaceController(3, 7, 6, 2);
        CreateSurfaceController(4, 5, 6, 7);

        SetProperties(vertices, triangles);

        Mesh.RecalculateNormals();
        ShowPointController();
    }

    public void PushSurface(int index1, int index2, int index3, int index4)
    {
        var vertices = new List<Vector3>
        {
            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],
            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],

            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],
            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],

            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],
            Mesh.vertices[index1],
            Mesh.vertices[index2],
            Mesh.vertices[index3],
            Mesh.vertices[index4],
        };

        var triangles = new List<int>
        {
            5, 4, 7,
            7, 6, 5,

            12, 8, 11,
            11, 15, 12,

            21, 9, 16,
            16, 20, 21,

            23, 19, 10,
            10, 22, 23,

            17, 13, 14,
            14, 18, 17,
        };

        for (int i = 0; i < triangles.Count; i++)
        {
            triangles[i] += Mesh.vertexCount;
        }

        int vertexIndex = Mesh.vertexCount;
        foreach (var index in new List<int> {index1, index2, index3, index4})
        {
            int listIndex = (index % 8) + (index / 24) * 8;
            pointList.Add(pointList[listIndex]);
            pointList[listIndex].AddIndexList(new List<int> {vertexIndex, vertexIndex + 8, vertexIndex + 16});
            vertexIndex++;
        }

        for (int i = 4; i < 8; i++)
        {
            pointList.Add(CreatePointController(i));
        }

        CreateEdgeController(0, 4);
        CreateEdgeController(1, 5);
        CreateEdgeController(2, 6);
        CreateEdgeController(3, 7);

        CreateEdgeController(5, 4);
        CreateEdgeController(4, 7);
        CreateEdgeController(7, 6);
        CreateEdgeController(6, 5);

        CreateSurfaceController(7, 3, 0, 4);
        CreateSurfaceController(1, 5, 4, 0);
        CreateSurfaceController(2, 6, 5, 1);
        CreateSurfaceController(3, 7, 6, 2);
        CreateSurfaceController(4, 5, 6, 7);
        
        SetProperties(vertices, triangles);

        Mesh.RecalculateNormals();
        ShowSurfaceController();
    }

    private void SetProperties(List<Vector3> vertices, List<int> triangles)
    {
        var vertexList = new List<Vector3>();
        Mesh.GetVertices(vertexList);
        var triangleList = new List<int>();
        Mesh.GetTriangles(triangleList, 0);
        var uvList = new List<Vector2>();
        Mesh.GetUVs(0, uvList);

        vertexList.AddRange(vertices.ToList());
        Mesh.SetVertices(vertexList);

        triangleList.AddRange(triangles.ToList());
        Mesh.SetTriangles(triangleList, 0);

        uvList.AddRange(uvs);
        Mesh.SetUVs(0, uvList);
    }

    private VertexControllerBase CreatePointController(int index)
    {
        index += Mesh.vertexCount;
        var vertexController = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<PointController>();
        vertexController.GetComponent<Rigidbody>().useGravity = false;
        vertexController.GetComponent<Collider>().isTrigger = true;
        vertexController.SetMaterials(controllerDefaultMat, controllerSelectedMat);
        vertexController.SetIndexList(new List<int> {index, index + 8, index + 16});
        vertexController.transform.SetParent(transform);
        vertexController.transform.localScale *= meshScale * 0.1f;
        return vertexController;
    }

    private void CreateEdgeController(int index1, int index2)
    {
        int listIndex1 = index1 + (Mesh.vertexCount / 24) * 8;
        int listIndex2 = index2 + (Mesh.vertexCount / 24) * 8;

        var vertexController = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<EdgeController>();
        vertexController.GetComponent<Rigidbody>().useGravity = false;
        vertexController.GetComponent<Collider>().isTrigger = true;
        vertexController.SetMaterials(controllerDefaultMat, controllerSelectedMat);
        vertexController.SetPointList(new List<VertexControllerBase> {pointList[listIndex1], pointList[listIndex2]});
        vertexController.transform.SetParent(transform);
        vertexController.transform.localScale *= meshScale * 0.1f;
    }

    private void CreateSurfaceController(int index1, int index2, int index3, int index4)
    {
        int listIndex1 = index1 + (Mesh.vertexCount / 24) * 8;
        int listIndex2 = index2 + (Mesh.vertexCount / 24) * 8;
        int listIndex3 = index3 + (Mesh.vertexCount / 24) * 8;
        int listIndex4 = index4 + (Mesh.vertexCount / 24) * 8;

        var vertexController = GameObject.CreatePrimitive(PrimitiveType.Sphere).AddComponent<SurfaceController>();
        vertexController.GetComponent<Rigidbody>().useGravity = false;
        vertexController.GetComponent<Collider>().isTrigger = true;
        vertexController.SetMaterials(controllerDefaultMat, controllerSelectedMat);
        vertexController.SetPointList(new List<VertexControllerBase>{pointList[listIndex1], pointList[listIndex2], pointList[listIndex3], pointList[listIndex4]});
        vertexController.transform.SetParent(transform);
        vertexController.transform.localScale *= meshScale * 0.1f;
    }

    private void ShowController(VertexControllerBase[] showingControllers, VertexControllerBase[] controllers1, VertexControllerBase[] controllers2)
    {
        foreach (var controller in showingControllers)
        {
            controller.ShowController();
        }
        
        foreach (var controller in controllers1)
        {
            controller.HideController();
        }

        foreach (var controller in controllers2)
        {
            controller.HideController();
        }
    }

    public void ShowPointController()
    {
        var points = GetComponentsInChildren<PointController>();
        var edges = GetComponentsInChildren<EdgeController>();
        var surfaces = GetComponentsInChildren<SurfaceController>();

        ShowController(points, edges, surfaces);
    }

    public void ShowEdgeController()
    {
        var points = GetComponentsInChildren<PointController>();
        var edges = GetComponentsInChildren<EdgeController>();
        var surfaces = GetComponentsInChildren<SurfaceController>();

        ShowController(edges, points, surfaces);
    }

    public void ShowSurfaceController()
    {
        var points = GetComponentsInChildren<PointController>();
        var edges = GetComponentsInChildren<EdgeController>();
        var surfaces = GetComponentsInChildren<SurfaceController>();

        ShowController(surfaces, points, edges);
    }

    public void Materialize()
    {
        var meshCollider = gameObject.AddComponent<MeshCollider>();
        meshCollider.sharedMesh = Mesh;
        meshCollider.convex = true;
        meshCollider.isTrigger = false;

        var rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = false;
        rb.useGravity = true;

        var controllers = transform.GetComponentsInChildren<VertexControllerBase>();
        foreach (var controller in controllers)
        {
            Destroy(controller.gameObject);
        }
    }

    public static void SaveUnityObject(Object target)
    {
#if UNITY_EDITOR
        string path = string.Format("Assets/{0}.asset", DateTime.Now.ToString().Replace("/", "").Replace(":", "").Replace(" ", ""));
        AssetDatabase.CreateAsset(target, path);
        AssetDatabase.SaveAssets();
#endif
    }
}
}