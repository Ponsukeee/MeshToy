using UnityEngine;

namespace VRUtils.MeshToy
{
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class EdgeController : VertexControllerBase
{
    private void FixedUpdate()
    {
        tf.localPosition = meshGenerator.Mesh.vertices[pointList[0].IndexList[0]] +
                           (meshGenerator.Mesh.vertices[pointList[1].IndexList[0]] - meshGenerator.Mesh.vertices[pointList[0].IndexList[0]]) / 2;
    }
}
}