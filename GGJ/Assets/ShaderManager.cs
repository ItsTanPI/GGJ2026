using UnityEngine;

public class ShaderManager : MonoBehaviour
{
    [Header("Targeting")]
    public Material targetMaterial; // Drag the specific material here

    [Header("Players")]
    public Key playerA;
    public Key playerB;

    private void Update()
    {
        if (targetMaterial == null) return;

        if (playerA != null && playerA.IsActive)
        {
            Debug.Log("Player A is Masked");
            targetMaterial.SetVector("_PlayerA", playerA.transform.position);
            targetMaterial.SetFloat("_IsActiveA", 1.0f);
        }
        else
        {
            targetMaterial.SetFloat("_IsActiveA", 0.0f);
        }

        if (playerB != null && playerB.IsActive)
        {
            Debug.Log("Player  is Masked");

            targetMaterial.SetVector("_PlayerB", playerB.transform.position);
            targetMaterial.SetFloat("_IsActiveB", 1.0f);
        }
        else
        {
            targetMaterial.SetFloat("_IsActiveB", 0.0f);
        }
    }
}