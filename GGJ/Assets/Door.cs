using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Door : MonoBehaviour
{
    public int Keys;
    public int CollectedKeys;
    public GameObject ActualDoor;
    public Image Image;
    public Color FinalColor;
    public Color FinalColorDark;
    public int cout = 0;

    private void Awake()
    {
        StartCoroutine(GetIn());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Movement>() != null)
        {
            cout ++;
        }

        if(cout == 2)
        {
            StartCoroutine(Delay());

        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if (other.GetComponent<Movement>() != null)
        {
            cout--;
        }
    }

    IEnumerator Delay()
    {
        while (Image.color.a <= 0.99f)
        {
            Image.color = Color.Lerp(Image.color, FinalColor, Time.deltaTime * 3);
            yield return null;
        }
        Image.color = FinalColor;
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(0);
    }

    IEnumerator GetIn()
    {
        while (Image.color.a >= 0.001f)
        {
            Image.color = Color.Lerp(Image.color, FinalColorDark, Time.deltaTime * 3);
            yield return null;
        }
        Image.color = FinalColorDark;
    }
    public void Open()
    {

    }

    public void Close()
    {

    }

    public void KeyCollected()
    {
        CollectedKeys++;
    }
}
