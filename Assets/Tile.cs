using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    public Vector2Int position;
    public Text text;

    public void SetText(int x, int y)
    {
        text.text = $"{x},{y}";
    }
    public void SetText(string s)
    {
        text.text = s;
    }
}

