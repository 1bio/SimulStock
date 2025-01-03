using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Chatting : MonoBehaviour
{
    public InputField inputText;

    public void Test(Text text)
    {
        text.text = inputText.text;
    }
}
