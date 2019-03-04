using System.Collections.Generic;
using UnityEngine;

namespace VRUtils.MeshToy
{
public abstract class VertexControllerBase : MonoBehaviour
{
    private Material defaultMaterial;
    private Material selectedMaterial;
    private Renderer rend;
    private Collider coll;
    protected Transform tf;

    protected MeshGenerator meshGenerator;
    public List<int> IndexList { get; private set; } = new List<int>();
    protected List<VertexControllerBase> pointList = new List<VertexControllerBase>();

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        coll = GetComponent<Collider>();
        tf = GetComponent<Transform>();

        rend.material = defaultMaterial;
    }

    private void Start()
    {
        meshGenerator = tf.parent.GetComponent<MeshGenerator>();
    }

    public void SetMaterials(Material defaultMat, Material selectedMat)
    {
        defaultMaterial = defaultMat;
        selectedMaterial = selectedMat;
        rend.material = defaultMaterial;
    }

    public void SetIndexList(List<int> list)
    {
        IndexList = list;
    }

    public void SetPointList(List<VertexControllerBase> list)
    {
        pointList = list;
    }

    public void AddIndexList(List<int> list)
    {
        IndexList.AddRange(list);
    }

    public void ShowController()
    {
        enabled = true;
        rend.enabled = true;
        coll.enabled = true;
    }

    public void HideController()
    {
        rend.enabled = false;
        coll.enabled = false;
        enabled = false;
    }

    private void Drag(Vector3 diffPosition)
    {
        if (meshGenerator == null) return;

        var vertices = meshGenerator.Mesh.vertices;
        foreach (var index in IndexList)
        {
            vertices[index] += transform.localRotation * diffPosition;
        }

        foreach (var point in pointList)
        {
            foreach (var index in point.IndexList)
            {
                vertices[index] += transform.localRotation * diffPosition;
            }
        }

        meshGenerator.Mesh.vertices = vertices;
        meshGenerator.Mesh.RecalculateNormals();
    }

    public void OnSet()
    {
        if (rend != null)
            rend.material = selectedMaterial;
    }

    public void OnUnset()
    {
        if (rend != null)
            rend.material = defaultMaterial;
    }
}
}