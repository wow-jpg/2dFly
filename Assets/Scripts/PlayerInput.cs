using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace ZJ.Input
{
    [CreateAssetMenu(menuName = "�����������")]
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
        /// ������Ϸ��ɫ����
        /// </summary>
        public void DisableAllInputs()
        {
            inputActions.Disable();
        }

        /// <summary>
        /// ������Ϸ��ɫ����
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


        //�̳�InputAction�����úõ����붯���ӿڣ�ֻҪ���ж���Դ����ͻ�ִ�У�
        public void OnMove(InputAction.CallbackContext context)
        {
            //�жϻص��׶�  �Ƿ����  ���ڰ���
            if (context.phase == InputActionPhase.Performed)
            {
                onMove?.Invoke(context.ReadValue<Vector2>());
            }
            //�жϻص��׶�  �Ƿ����  ֹͣ����
            if (context.phase == InputActionPhase.Canceled)
            {
                onStopMove?.Invoke();
            }


        }

        public void OnFire(InputAction.CallbackContext context)
        {
            //�жϻص��׶�  �Ƿ����  ���ڰ���
            if (context.phase == InputActionPhase.Performed)
            {
                onFire?.Invoke();
            }
            //�жϻص��׶�  �Ƿ����  ֹͣ����
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
        /// �л�������
        /// </summary>
        /// <param name="actionMap"></param>
        /// <param name="isUIInput">�Ƿ�ΪUI����</param>
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