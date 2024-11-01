using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum EInputControlSchemeType
{
    PC,
    Gamepad,
}

public enum EInputActionMapType
{
    InGame,
    OutGame,
}

public class InputMgr : MonoBehaviour
{
    private PlayerInput playerInput;

    EInputControlSchemeType currControlSchemeType = EInputControlSchemeType.PC;
    EInputActionMapType currActionMapType = EInputActionMapType.InGame;

    // Keyboard InputEvent
    public event Action<Vector2> OnArrowKeyEntered;
    public event Action<Vector2> OnASDFKeyEntered;
    public event Action OnSpaceKeyEntered;
    public event Action OnFKeyEntered;
    public event Action OnZKeyEntered;
    public event Action OnXKeyEntered;
    public event Action OnCKeyEntered;
    public event Action OnVKeyEntered;
    public event Action OnNum1KeyEntered;
    public event Action OnNum2KeyEntered;

    public void Init()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.defaultControlScheme = currControlSchemeType.ToString();
        playerInput.defaultActionMap = currActionMapType.ToString();
        playerInput.notificationBehavior = PlayerNotifications.SendMessages;

    }

    public void Clear()
    {
    }

    #region Basic InputEvent
    public void OnDeviceLost()
    {
        Debug.Log("OnDeviceLost");
    }

    public void OnDeviceRegained()
    {
        Debug.Log("OnDeviceRegained");
    }

    public void OnControlsChanged()
    {
        Debug.Log("OnControlsChanged");
    }
    #endregion

    #region Keyboard InputEvent
    public void OnArrowKey(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>();
        OnArrowKeyEntered?.Invoke(inputVec);
    }

    public void OnASDFKey(InputValue value)
    {
        Vector2 inputVec = value.Get<Vector2>();
        OnASDFKeyEntered?.Invoke(inputVec);
    }

    public void OnSpaceKey()
    {
        OnSpaceKeyEntered?.Invoke();
    }

    public void OnFKey()
    {
        OnFKeyEntered?.Invoke();
    }

    public void OnZKey()
    {
        OnZKeyEntered?.Invoke();
    }

    public void OnXKey()
    {
        OnXKeyEntered?.Invoke();
    }

    public void OnCKey()
    {
        OnCKeyEntered?.Invoke();
    }

    public void OnVKey()
    {
        OnVKeyEntered?.Invoke();
    }

    public void OnNum1Key()
    {
        OnNum1KeyEntered?.Invoke();
    }
    public void OnNum2Key()
    {
        OnNum2KeyEntered?.Invoke();
    }
    #endregion
}
