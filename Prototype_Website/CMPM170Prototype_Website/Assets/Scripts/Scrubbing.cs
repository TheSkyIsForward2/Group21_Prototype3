using System;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scrubbing : MonoBehaviour
{
    public TMP_Text body;
    public TMP_Text header;
    public Slider bar;
    public TMP_Text scrubbedText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        //detects intersecting character for body text, sets characters intersecting with mouse to ' ', adds to progress bar.
        char[] newText1 = body.text.ToCharArray();
        int a = TMP_TextUtilities.FindIntersectingCharacter(body, Input.mousePosition, Camera.current, true);
        if (a != -1 && newText1[a] != ' ' && Input.GetMouseButton(0))
        {
            newText1[a] = ' ';
            bar.value++;
        }
        body.SetText(newText1);

        //same thing but with the header.
        char[] newText2 = header.text.ToCharArray();
        int b = TMP_TextUtilities.FindIntersectingCharacter(header, Input.mousePosition, Camera.current, true);
        if (b != -1 && newText2[b] != ' ' && Input.GetMouseButton(0))
        {
            newText2[b] = ' ';
            bar.value++;
        }
        header.SetText(newText2);

        if (bar.value == bar.maxValue)
        {
            scrubbedText.enabled = true;
        }
    }

}
