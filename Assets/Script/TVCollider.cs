using UnityEngine;
using System.Diagnostics;

public class TVCollider : MonoBehaviour
{
    public string websiteURL = "https://www.shinhansec.com/siw/customer-center/protection/597601/view.do#!"; 

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player"))
        {
            OpenWebsite();
        }
    }

    private void OpenWebsite()
    {
        Process.Start(websiteURL);
    }
}
