using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    public FrameInput FrameInput { get; private set; }
    private PlayerInputActions _playerInputActions;
    private InputAction _move;
    private InputAction _jump;

    //Unity消息 | 0个引用
    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _move = _playerInputActions.Player.Move;
        _jump = _playerInputActions.Player.Jump;
    }

    private void Update()
    {
        FrameInput = GatherInput();
    }

    //Unity消息 | 0个引用
    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    //Unity消息 | 0个引用
    private void OnDisable()
    {
        _playerInputActions.Disable();
    }

    // 1个引用
    private FrameInput GatherInput()
    {
        return new FrameInput
        {
            Move = _move.ReadValue<Vector2>(),
            Jump = _jump.WasPressedThisFrame()
        };
    }
}
/// <summary>
/// 帧输入结构体，管理所有的玩家输入动作:移动 跳跃
/// </summary>
public struct FrameInput
{
    public Vector2 Move;
    public bool Jump;
}
