using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject player; 

    public Button button_yes;
    public Button button_no;
    
    public GameObject exit_Panel; 

    public GameObject chatting_Panel;
    public Toggle chatToggle;

    public GameObject stock;
    public Button stockPanel_On;
    public Button stockPanel_Off;

    
    public GameObject prefabButton;

    public GameObject purchase_Panel;
    public Button purchase_Panel_Off;

    public Button Send_Button;
    public Text chat;
    public InputField inputField;

    private string input;

    public void ExitButton() 
    {
        if (true)
        {
            exit_Panel.SetActive(true);
        }
     }

 
    public void OnClickYesButton() 
    {
        Application.Quit();
    }

    public void OnClickNoButton()
    {
        if (true)
        {
            exit_Panel.SetActive(false);
        }
    }

    
    public void ChattingToggle()
    {
        if (chatToggle.isOn)
        {
            chatting_Panel.SetActive(true);
        }
        else
        {
            chatting_Panel.SetActive(false);
        }
    }

    public void stock_panelButtonOn()
    {
        if (true)
        {
            stock.SetActive(true);
        }
    }
    
    public void stock_panelButtonOff()
    {
        if (true)
        {
            stock.SetActive(false);
        }
    }

  /*  public void send_Button()
    {
        chat.text += inputField.text + '\n';
    }
   */

    void ReadStringInput(string s)
    {
        input = s;
        Debug.Log(input);
    }

    public void Purchase_Button()
    {
        purchase_Panel.SetActive(false);
    }
}

    


