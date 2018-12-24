using UnityEngine;
using System.Collections;

public class UIManager : MonoBehaviour {

    private static UIManager _instance;

    [SerializeField] GameObject m_popupTemplate;
    UIPopup m_popup;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public UIPopup Popup()
    {
        if( m_popup == null && m_popupTemplate != null )
        {
            GameObject go = GameObject.Instantiate(m_popupTemplate) as GameObject;
            m_popup = go.GetComponent<UIPopup>();
            go.transform.SetParent(transform,false);
        }
        return m_popup;
    }

    public static UIManager instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject prefab = Resources.Load("LR/UI/UIManager") as GameObject;
                GameObject newGO = Instantiate(prefab) as GameObject;
                _instance = newGO.GetComponent<UIManager>();
                //Search for a canvas
                Canvas canvas = FindObjectOfType<Canvas>();
                if( canvas != null)
                {
                    newGO.transform.SetParent(canvas.transform,false);
                }
                else
                {
                    Debug.LogError("No Canvas found. You cannot use UIManager if no canvas is present in your scene.");
                }
            }
            return _instance;
        }
    }
}
