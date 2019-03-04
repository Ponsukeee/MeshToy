using UnityEngine;

namespace VRUtils.MeshToy
{
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Collider))]
public class SurfaceController : VertexControllerBase
{
    private void FixedUpdate()
    {
        tf.localPosition = meshGenerator.Mesh.vertices[pointList[1].IndexList[0]] +
                           (meshGenerator.Mesh.vertices[pointList[3].IndexList[0]] - meshGenerator.Mesh.vertices[pointList[1].IndexList[0]]) / 2;
    }
}
}