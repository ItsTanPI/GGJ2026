using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{

    private InputMain inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private Gamepad myGamepad;

    public Movement _movement;

    public void Init(InputDevice[] devices)
    {
        inputActions = new InputMain();
        inputActions.asset.devices = devices;

        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        inputActions.Player.Enable();

        foreach (var device in devices)
        {
            if (device is Gamepad pad)
            {
                myGamepad = pad;
                break;
            }
        }
    }


    private void Start()
    {
        _movement = GetComponent<Movement>();
    }

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        Vector2 moveInput = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        Vector2 lookInput = lookAction?.ReadValue<Vector2>() ?? Vector2.zero;
        _movement.Move(moveInput, lookInput);
    }

    public void Vibrate(float low = 0.5f, float high = 0.5f, float duration = 0.1f)
    {
        if (myGamepad != null)
        {
            StartCoroutine(VibrateRoutine(low, high, duration));
        }
    }

    private IEnumerator VibrateRoutine(float low, float high, float duration)
    {
        myGamepad.SetMotorSpeeds(low, high);
        yield return new WaitForSeconds(duration);
        myGamepad.SetMotorSpeeds(0, 0);
    }
}
