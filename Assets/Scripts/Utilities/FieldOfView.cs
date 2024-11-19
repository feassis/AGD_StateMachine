using CodeMonkey.Utils;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using static UnityEngine.UI.Image;
using UnityEngine.Rendering.PostProcessing;
using StatePattern.Player;

[RequireComponent(typeof(MeshRenderer))]
public class FieldOfView : MonoBehaviour
{ 
    public Mesh Mesh { get; private set; }

    private FieldOFViewController controller;


    private void Start()
    {
        Mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = Mesh;
    }

    public void SetViewController(FieldOFViewController controller)
    {
        this.controller = controller;
    }

    private void Update()
    {
        if (controller == null)
        {
            return;
        }

        controller.CreateViewMesh();
    }
}

public class FieldOFViewController
{
    private FieldOfViewSO model;
    private FieldOfView view;

    private float startingAngle;
    Vector3 origin = Vector3.zero;


    public FieldOFViewController(FieldOfViewSO model, FieldOfView view)
    {
        this.model = model;
        this.view = GameObject.Instantiate<FieldOfView>(view);
        this.view.SetViewController(this);
    }


    public void Destroy()
    {
        GameObject.Destroy(this.view.gameObject);
    }

   public void CreateViewMesh()
    {
        int rayCount = model.RayAmount;
        float angle = startingAngle;
        float angleIncrease = model.FOV / rayCount;
        float viewDistance = model.ViewDistance;

        Vector3[] vertices = new Vector3[rayCount + 1 + 1];
        Vector2[] uv = new Vector2[vertices.Length];
        int[] triangles = new int[rayCount * 3];

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for (int i = 0; i <= rayCount; i++)
        {
            Vector3 vertex;

            Physics.Raycast(origin, UtilsClass.GetVectorFromAngle(angle), out RaycastHit hitInfo, viewDistance, model.ColisionMask);

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

        view.Mesh.vertices = vertices;
        view.Mesh.uv = uv;
        view.Mesh.triangles = triangles;
    }

    public void SetOrigin(Vector3 origin)
    {
        this.origin = origin;
    }

    public void SetAimDirection(Vector3 aimDirection)
    {
        startingAngle = UtilsClass.GetAngleFromVectorFloatXZ(aimDirection) - model.FOV / 2f;
    }
}
