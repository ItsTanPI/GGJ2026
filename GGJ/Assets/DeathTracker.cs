using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class DeathTracker : MonoBehaviour
{
    public Transform[] GameObj;

    public Image Image;
    public Color FinalColor;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var item in GameObj)
        {
            if (item.GetComponent<Health>())
            {
                if (item.GetComponent<Health>().IsDead)
                {
                    StartCoroutine(Delay());
                    break;
                }
            }

            if (item.position.y <= -10)
            {
                
                StartCoroutine(Delay());
                break;
            }

        }
    }

    IEnumerator Delay()
    {
        while (Image.color.a <= 0.999f)
        {
            Image.color = Color.Lerp(Image.color, FinalColor, Time.deltaTime * 3);
            yield return null;
        }
        Image.color = FinalColor;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
