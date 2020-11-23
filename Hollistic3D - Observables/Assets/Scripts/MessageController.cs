using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageController : MonoBehaviour
{
    private Text message;
    private void Start() {
        message = this.GetComponent<Text>();
        message.enabled = false;
    }
    public void SetMessage(GameObject gameObject)
    {
        message.text = "You picked up an item!!";
        message.enabled = true;
        Invoke("DisableMessage", 2);
    }
    private void DisableMessage()
    {
        message.enabled = false;
    }
}
