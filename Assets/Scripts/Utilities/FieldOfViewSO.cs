using UnityEngine;

[CreateAssetMenu(fileName = "New FOV SO", menuName = "FOV")]
public class FieldOfViewSO : ScriptableObject
{
    public float FOV = 90;
    public int RayAmount = 50;
    public float ViewDistance = 5f;
    public LayerMask ColisionMask;
    public LayerMask TargetMask;
}
