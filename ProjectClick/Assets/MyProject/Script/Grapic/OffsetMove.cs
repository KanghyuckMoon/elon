using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffsetMove : MonoBehaviour
{
    [SerializeField]
    private float upScale;
    MeshRenderer mesh;
    Vector2 offset = Vector2.zero;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void FixedUpdate()
    {
        offset.y += upScale;
        mesh.material.SetTextureOffset("_MainTex", offset);
    }
}
