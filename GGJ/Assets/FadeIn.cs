using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public GameObject Player;
    public MeshRenderer Renderer;

    Material material;

    private void Start()
    {
        material = Renderer.sharedMaterial;
    }

    private void Update()
    {
        material.SetVector("_WorldPosition", Player.transform.position);
    }
}
