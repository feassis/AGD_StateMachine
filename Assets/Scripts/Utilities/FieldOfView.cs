using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{
    [SerializeField] private LayerMask colisionMask;
    private Mesh mesh;

    Vector3 origin = Vector3.zero;
    float startingAngle = 0;
    float fov;

    private void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        fov = 90;
    }

    private void Update()
    {
        CreateViewMesh();
    }

    private void CreateViewMesh()
    {
        
        Vector3 origin = Vector3.zero;
        int rayCount = 50;
        float angle = startingAngle;
        float angleIncrease = fov / rayCount;
        float viewDistance = 5f;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            Physics.Raycast(transform.position, UtilsClass.GetVectorFromAngle(angle), out RaycastHit hitInfo, viewDistance, colisionMask);

            if (hitInfo.collider == null)
            {
                vertex = origin + UtilsClass.GetVectorFromAngle(angle) * viewDistance;
            }
            else
            {
                vertex = vertex = origin + UtilsClass.GetVectorFromAngle(angle) * hitInfo.distance;
            }

            vertices[vertexIndex] = vertex;

            if (i > 0)
            {
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;
                triangleIndex += 3;
            }

            angle -= angleIncrease;
            vertexIndex++;

        }

        triangles[0] = 0;
        triangles[1] = 1;
        triangles[2] = 2;

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloatXZ(aimDirection) - fov / 2f ;
    }
}
