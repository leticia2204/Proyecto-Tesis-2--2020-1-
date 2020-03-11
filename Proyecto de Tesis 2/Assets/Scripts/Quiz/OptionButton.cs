using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Image))]
public class OptionButton : MonoBehaviour
{
    private Text text = null;
    private Button button = null;
    private Image image = null;
    private Color color = Color.black;

    public Option Option {get;set; }

    private void Awake()
    {
        button = GetComponent<Button>();
        image = GetComponent<Image>();
        text = transform.GetChild(0).GetComponent<Text>();
        color = image.color;
    }

    public void Construc(Option option, Action<OptionButton> callback)
    {
        text.text = option.text;
        button.onClick.RemoveAllListeners();
        button.enabled = true;
        image.color = color;
        Option = option;
        button.onClick.AddListener(delegate
        {
            callback(this);
        });
    }

    public void SetColor (Color c)
    {
        button.enabled = false;
        image.color = c;
    }

}
