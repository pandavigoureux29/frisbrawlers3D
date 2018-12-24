using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputPowerButton : MonoBehaviour {

    public Text text;

    public PlayerInputManager input;
    Player player;
    Power power;

    Color goodColor;

	// Use this for initialization
	void Awake () {
        input.OnPlayerSet += Input_OnPlayerSet;     
	}

    private void Input_OnPlayerSet(object sender, System.EventArgs e)
    {
        player = input.Player;
        power = player.GetComponent<Power>();
    }

    // Update is called once per frame
    void Update () {
        if ( power != null && !power.IsReady )
        {
            GetComponent<Button>().interactable = false;
            var s = (int) power.RemainingCooldown;
            if (s > 0)
                text.text = s.ToString();
            else
                text.text = "";
        }
        else
        {
            GetComponent<Button>().interactable = true;
        }
	}    
}
