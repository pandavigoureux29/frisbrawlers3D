using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputCurveButton : MonoBehaviour {

    public PlayerInputManager input;
    Player player;
    Power power;

    public Sprite activeColor;
    public Sprite disabledColor;

    // Use this for initialization
    void Awake()
    {
        input.OnPlayerSet += Input_OnPlayerSet;
    }

    private void Input_OnPlayerSet(object sender, System.EventArgs e)
    {
        player = input.Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            GetComponent<Image>().sprite = player.CurveActive ? activeColor : disabledColor;
        }
    }
}
