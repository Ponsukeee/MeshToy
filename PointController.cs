using UnityEngine;

namespace VRUtils.MeshToy
{
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class PointController : VertexControllerBase
{
    private void FixedUpdate()
    {
        tf.localPosition = meshGenerator.Mesh.vertices[IndexList[0]];
    }
}
}