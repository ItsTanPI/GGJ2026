using UnityEditor.U2D.Aseprite;
using UnityEngine;


public class Key : MonoBehaviour
{
    public bool IsActive;

    public void TurnOff()
    {
        Debug.Log("Key Turn On Mask");
        IsActive = false;
    }

    public void TurnON()
    {
        Debug.Log("Key Turn Off Mask");

        IsActive = true;
    }
}