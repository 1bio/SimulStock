using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ReadChatting : MonoBehaviour
{
    private string input;
    public Text output_Text;

   public void ReadStringInput(string s)
    {
       input = s;
        // Debug.Log(input);
        output_Text.text = "ฟ๘มพ : " +  input + '\n';
    }
}
