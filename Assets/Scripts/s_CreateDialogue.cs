using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class s_CreateDialogue : MonoBehaviour
{

    public GameObject dialogueCanvas;
    private GameObject dialoguePanel;
    s_Dialogue dialogueScript;
    public string[] lines;
    public float textSpeed = 0.05f;

    private int index;
    // Start is called before the first frame update
    void OnEnable()
    {
        dialoguePanel = dialogueCanvas.transform.GetChild(0).gameObject;
        dialogueScript = dialoguePanel.GetComponent<s_Dialogue>();
        if (dialogueScript != null)
        {
            dialogueScript.lines = lines;
            dialogueScript.textSpeed = textSpeed;

        }
        dialogueCanvas.SetActive(true);
        dialoguePanel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueScript.finished == true)
        {
            dialogueCanvas.SetActive(false);
            dialogueScript.finished = false;
            enabled = false;
        }
    }
}
