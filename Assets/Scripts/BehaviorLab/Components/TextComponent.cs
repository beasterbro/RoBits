using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextComponent : BlockComponent
{
    [SerializeField] private Text text;

    private void Start()
    {
        text = text ?? GetComponent<Text>();
    }
}
