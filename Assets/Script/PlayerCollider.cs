using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCollider : MonoBehaviour
{
    public GameObject npc_Panel; 
    public Text[] texts; 
    public GameObject Video_Panel;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "NPC1")
        {
            npc_Panel.SetActive(true);
            texts[0].gameObject.SetActive(true);
        }
        else if (other.tag == "NPC2")
        {
            npc_Panel.SetActive(true);
            texts[1].gameObject.SetActive(true);

        }
        else if (other.tag == "NPC3")
        {
            npc_Panel.SetActive(true);
            texts[2].gameObject.SetActive(true);

        }
        else if (other.tag == "NPC4")
        {
            npc_Panel.SetActive(true);
            texts[3].gameObject.SetActive(true);

        }
        else if (other.tag == "computer")
        {
            Video_Panel.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "NPC1")
        {
            npc_Panel.SetActive(false);
            texts[0].gameObject.SetActive(false);
        }
        else if (other.tag == "NPC2")
        {
            npc_Panel.SetActive(false);
            texts[1].gameObject.SetActive(false);
        }
        else if (other.tag == "NPC3")
        {
            npc_Panel.SetActive(false);
            texts[2].gameObject.SetActive(false);
        }
        else if (other.tag == "NPC4")
        {
            npc_Panel.SetActive(false);
            texts[3].gameObject.SetActive(false);
        }
        else if (other.tag == "computer")
        {
            Video_Panel.SetActive(false);
        }
    }
}
