using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ZJ;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;
using ZJ.Input;

public class UIInput : Singleton<UIInput>
{
    [SerializeField] PlayerInput playerInput;
    [SerializeField] InputSystemUIInputModule uiInputModule;

    protected override void Awake()
    {
        base.Awake();
        uiInputModule.GetComponent<InputSystemUIInputModule>();
        uiInputModule.enabled = false;
    }

    /// <summary>
    /// …Ë÷√—°÷–◊¥Ã¨
    /// </summary>
    /// <param name="uiObject"></param>
    public void SelectUI(Selectable uiObject)
    {
        uiObject.Select();
        uiObject.OnSelect(null);
        uiInputModule.enabled = true;
    }

    public void DisableAllUIInput()
    {
        playerInput.DisableAllInputs();
        uiInputModule.enabled = false;
    }
}
