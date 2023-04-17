using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZJ.Input
{
    [CreateAssetMenu(menuName = "玩家输入设置")]
    public class PlayerInput : 
        ScriptableObject,
        InputActions.IGamePlayActions,
        InputActions.IPauseMenuActions,
        InputActions.IGameOverScreenActions
    {
        public event UnityAction<Vector2> onMove;
        public event UnityAction onStopMove;

        public event UnityAction onFire;
        public event UnityAction onStopFire;
        public event UnityAction onDodge;
        public event UnityAction onOverdrive;
        public event UnityAction onPause;
        public event UnityAction onUnpause;
        public event UnityAction onLaunchMissile;
        public event UnityAction onConfirmGameOver;

        InputActions inputActions;

        private void OnEnable()
        {
            inputActions = new InputActions();
            inputActions.GamePlay.SetCallbacks(this);
            inputActions.PauseMenu.SetCallbacks(this);
            inputActions.GameOverScreen.SetCallbacks(this);
        }

        private void OnDisable()
        {
            DisableAllInputs();
        }


        /// <summary>
        /// 禁用游戏角色输入
        /// </summary>
        public void DisableAllInputs()
        {
            inputActions.Disable();
        }

        /// <summary>
        /// 启用游戏角色输入
        /// </summary>
        public void EnableGamePlayInput()
        {
            // inputActions.GamePlay.Enable();
            SwitchActionMap(inputActions.GamePlay, false);
        }

        public void EnablePauseMenuInput()
        {
            SwitchActionMap(inputActions.PauseMenu, true);
        }

        public void EnableGameOverSceneInput()
        {
            SwitchActionMap(inputActions.GameOverScreen, true);
        }


        //继承InputAction的设置好的输入动作接口（只要是有动作源输入就会执行）
        public void OnMove(InputAction.CallbackContext context)
        {
            //判断回调阶段  是否等于  正在按下
            if (context.phase == InputActionPhase.Performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }
            //判断回调阶段  是否等于  停止按下
            if (context.phase == InputActionPhase.Canceled)
            {
                onStopMove?.Invoke();
            }


        }

        public void OnFire(InputAction.CallbackContext context)
        {
            //判断回调阶段  是否等于  正在按下
            if (context.phase == InputActionPhase.Performed)
            {
                onFire?.Invoke();
            }
            //判断回调阶段  是否等于  停止按下
            if (context.phase == InputActionPhase.Canceled)
            {
                onStopFire?.Invoke();
            }


        }

        public void OnDodge(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onDodge?.Invoke();
            }
        }

        public void OnOverdrive(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onOverdrive?.Invoke();
            }
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onPause?.Invoke();
            }

        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onUnpause?.Invoke();
            }
        }


        /// <summary>
        /// 切换动作表
        /// </summary>
        /// <param name="actionMap"></param>
        /// <param name="isUIInput">是否为UI输入</param>
        void SwitchActionMap(InputActionMap actionMap, bool isUIInput)
        {
            inputActions.Disable();
            actionMap.Enable();

            if (isUIInput)
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }

        public void OnLaunchMissile(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                onLaunchMissile?.Invoke();
            }
        }

        public void OnConfirmGameOver(InputAction.CallbackContext context)
        {
            if(context.performed)
            {
                onConfirmGameOver?.Invoke();
            }
        }
    }

}