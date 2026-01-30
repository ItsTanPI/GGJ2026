using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : MonoBehaviour
{
    [Header("Assign characters in order")]
    public GameObject[] characters;
    public bool KeyboardGameplay = false;

    private List<PlayerController> players = new List<PlayerController>();
    private List<Gamepad> connectedGamepads = new List<Gamepad>();

    private void OnEnable()
    {
        InputSystem.onDeviceChange += OnDeviceChange;
        SetupPlayers();
        RefreshGamepads();
    }

    private void OnDisable()
    {
        InputSystem.onDeviceChange -= OnDeviceChange;
    }

    void SetupPlayers()
    {
        // Create PlayerController components for all characters
        players.Clear();

        foreach (var characterGO in characters)
        {
            var pc = characterGO.GetComponent<PlayerController>();
            if (pc == null)
                pc = characterGO.AddComponent<PlayerController>();
            players.Add(pc);
        }
    }

    void RefreshGamepads()
    {
        connectedGamepads.Clear();
        connectedGamepads.AddRange(Gamepad.all);

        AssignDevices();
    }

    void AssignDevices()
    {
        int playerIndex = 0;

        // Assign keyboard + mouse to first player if enabled
        if (KeyboardGameplay && players.Count > 0)
        {
            players[0].Init(new InputDevice[] { Keyboard.current, Mouse.current });
            playerIndex = 1;
        }

        // Assign gamepads to remaining players
        for (int i = playerIndex; i < players.Count; i++)
        {
            int padIndex = i - (KeyboardGameplay ? 1 : 0);
            if (padIndex < connectedGamepads.Count)
                players[i].Init(new InputDevice[] { connectedGamepads[padIndex] });
            else
                players[i].Init(new InputDevice[0]); // No device connected
        }
    }

    void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (device is Gamepad gamepad)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                case InputDeviceChange.Reconnected:
                    if (!connectedGamepads.Contains(gamepad))
                        connectedGamepads.Add(gamepad);
                    break;

                case InputDeviceChange.Removed:
                case InputDeviceChange.Disconnected:
                    connectedGamepads.Remove(gamepad);
                    break;
            }

            // Reassign all devices whenever a controller is plugged/unplugged
            AssignDevices();
        }
    }
}
