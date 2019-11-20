using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    public Image image;
    public Text text;
    // Start is called before the first frame update
    void Start()
    {
        EventManager.Subscribe("KingDestroyed", Gameover);
    }

    void Gameover(EventParameter eParam)
    {
        image.gameObject.SetActive(true);
        Color colour = eParam.playerParam.playerColour;
        colour.a = 0.75f;
        image.color = colour;
        text.text = eParam.playerParam.playerName + " Wins!";
    }
}
