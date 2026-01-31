using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InputManager : MonoBehaviour
{

    public static float SensitivityX;
    public static float SensitivityY;
    public static InputManager Instance { get; private set; }


    public InputMain inputMain;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        inputMain = new InputMain();
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void OnDisable()
    {
        inputMain?.Disable();
    }

    private void OnEnable()
    {
        inputMain?.Enable();
    }

}
