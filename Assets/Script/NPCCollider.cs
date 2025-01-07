using UnityEngine;

public class NPCCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && gameObject.CompareTag("NPC1"))
        {
            UIManager.instance.npcDialogue.SetActive(true);
            UIManager.instance.npcTexts[0].SetActive(true);
        }
        else if(other.CompareTag("Player") && gameObject.CompareTag("NPC2")){
            UIManager.instance.npcDialogue.SetActive(true);
            UIManager.instance.npcTexts[1].SetActive(true);
        }
        else if (other.CompareTag("Player") && gameObject.CompareTag("NPC3"))
        {
            UIManager.instance.npcDialogue.SetActive(true);
            UIManager.instance.npcTexts[2].SetActive(true);
        }
        else if (other.CompareTag("Player") && gameObject.CompareTag("NPC4"))
        {
            UIManager.instance.npcDialogue.SetActive(true);
            UIManager.instance.npcTexts[3].SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            UIManager.instance.npcDialogue.SetActive(false);

            foreach (GameObject text in UIManager.instance.npcTexts)
            {
                text.SetActive(false);
            }
        }
    }
}
