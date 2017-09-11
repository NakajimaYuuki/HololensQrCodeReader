﻿using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class GestureResponder : MonoBehaviour, InputClickHandler
{
    private void Start()
    {
        InputManager.Instance.PushFallbackInputHandler(gameObject);
    }

    public void OnInputClicked(InputEventData eventData)
    {
        PlaneTargetGroupPicker.Instance.PickNewTarget();
    }
}
