using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class UIPopup : MonoBehaviour {

    [SerializeField] GameObject m_bg;
    [SerializeField] Text m_text; 

    [SerializeField] List<ButtonInfo> m_buttons;

    private bool m_open = false;
    
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Open()
    {
        m_open = true;
        gameObject.SetActive(true);
    }

    public void Close()
    {
        m_open = false;
        gameObject.SetActive(false);
    }

    public void OnButtonClick(GameObject _button)
    {
        ButtonInfo bInfo = FindButton(_button);
        SendButtonMessage(bInfo);
        if (bInfo.autoClose)
            Close();
    }

    public void SetButton(string _name, string _callback, GameObject _target)
    {
        var button = GetButton(_name);
        if( button != null)
        {
            button.callback = _callback;
            button.callbackTarget = _target;
        }
    }

    #region FINDERS

    public ButtonInfo GetButton(string _name)
    {
        foreach (var buttonInfo in m_buttons)
        {
            if (buttonInfo.Name == _name)
                return buttonInfo;
        }
        return null;
    }
    
    ButtonInfo FindButton(GameObject _button)
    {
        foreach (var buttonInfo in m_buttons)
        {
            if (buttonInfo.button == _button)
                return buttonInfo;
        }
        return null;
    }
    #endregion

    void SendButtonMessage(ButtonInfo _button)
    {
        if( _button.callbackTarget != null)
        {
            _button.callbackTarget.SendMessage(_button.callback);
        }
    }

    [System.Serializable]
    public class ButtonInfo{
        public GameObject button;
        public string callback;
        public GameObject callbackTarget;
        public bool autoClose = false;

        public string Name
        {
            get { return button!=null ? button.name : ""; }
            set { if(button != null) button.name = value ; }
        }

        public void Set(string _text, string _callback, GameObject _callbackTarget, bool _autoClose = false)
        {
            SetText(_text);
            callback = _callback;
            callbackTarget = _callbackTarget;
            autoClose = _autoClose;
        }

        public void SetText(string _text)
        {
            if (button != null)
            {
                var textComp = button.GetComponentInChildren<Text>();
                if (textComp != null) textComp.text = _text;
            }
        }
    }
}
