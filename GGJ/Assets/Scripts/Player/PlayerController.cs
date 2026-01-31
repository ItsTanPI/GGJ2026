using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections;
using Masks;
using Player;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private InputMain inputActions;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction interactAction;
    private Gamepad myGamepad;

    public Movement _movement;
    public Throw _Throw;
    private MaskManager _maskManager;

    [SerializeField] private float interactionCooldown = 0.2f;
    private bool isInteractionInCooldown = false;
    
    public void Init(InputDevice[] devices)
    {
        inputActions = new InputMain();
        inputActions.asset.devices = devices;

        moveAction = inputActions.Player.Move;
        lookAction = inputActions.Player.Look;
        interactAction = inputActions.Player.Interact;
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
        _Throw = GetComponent<Throw>();
        _maskManager = GetComponent<MaskManager>();
    }

    float holdTime = 0f;
    bool isCharging = false;

    private void Update()
    {
        if (Keyboard.current.rKey.wasPressedThisFrame)
        {
            var scene = SceneManager.GetActiveScene();
            SceneManager.LoadScene(scene.buildIndex);
        }

        Vector2 moveInput = moveAction?.ReadValue<Vector2>() ?? Vector2.zero;
        Vector2 lookInput = lookAction?.ReadValue<Vector2>() ?? Vector2.zero;

        if (_maskManager.CurrentMaskType != MaskType.NecroMask || !GetComponent<Necro>().IsPuppeteering)
        {
            _movement.Move(moveInput, lookInput);
        }
        
        _maskManager.CacheInput(moveInput, lookInput);
        
        if (inputActions.Player.ActivateMask.ReadValue<float>() > 0)
        {
            _maskManager.TryActivateCurrentMask();
        }

        if (interactAction?.ReadValue<float>() > 0 && !isInteractionInCooldown)
        {
            GetComponent<Scanner>().InteractInputPressed();
            StopCoroutine(nameof(Cooldown));
            StartCoroutine(nameof(Cooldown));
        }

        var attack = inputActions.Player.Throw;

        if (attack.WasPressedThisFrame())
        {
            _Throw.StartCharging();
            if (_maskManager.CurrentMaskType == MaskType.NecroMask && GetComponent<Necro>().IsPuppeteering)
            {
                GetComponent<Necro>().KillSkeleton();
            }
        }

        if (attack.WasReleasedThisFrame())
        {
            _Throw.ReleaseThrow(lookInput);
        }

        if(interactAction.WasPerformedThisFrame())
        {
            //_Throw.GetFirstObjectOnLayer();
        }

    }
    
    IEnumerator Cooldown()
    {
        isInteractionInCooldown = true;
        yield return new WaitForSeconds(interactionCooldown);
        isInteractionInCooldown = false;
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
