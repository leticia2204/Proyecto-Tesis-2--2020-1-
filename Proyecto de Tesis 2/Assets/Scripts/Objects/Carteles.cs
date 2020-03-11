using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Carteles : Interactable
{
    public GameObject dialogBox;
    public Text dialogText;
    public string[] dialog;


    int index = 1;
    bool botonSI = false;
    float typingSpeed = 0.03f;
    //public GameObject continueButton;


    void Start()
    {

    }

    public virtual void Update()
    {
        if (Input.GetButtonDown("interact") && playerInRange)
        {
            if (dialogBox.activeInHierarchy)
            {
                dialogBox.SetActive(false);
                index = 1;
            }
            else
            {
                dialogBox.SetActive(true);
                dialogText.text = dialog[0];
            }
            StopAllCoroutines();
        }
        
        //MULTITEXTO:
        /*
        if(dialogText.text == dialog[index])
        {
            continueButton.SetActive(true);
        }*/

    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Debug.Log("Player left range");
            context.Raise();
            playerInRange = false;
            dialogBox.SetActive(false);
        }
    }

    public void btnSI()
    {
        botonSI = true;
        dialogText.text = "";
        StartCoroutine(Type());
        //index = 1;
    }

    public void btnNO()
    {
        dialogBox.SetActive(true);
        dialogText.text = dialog[dialog.Length-1];
        index = 1;
        //continueButton.SetActive(true);
    }


    IEnumerator Type()
    {
            foreach (char c in dialog[index].ToCharArray())
            {
                dialogText.text += c;
                yield return new WaitForSeconds(typingSpeed);
            }
        NextSentece();
    }

    public void NextSentece()
    {
        //source.Play();
        //textDisplayAnim.SetTrigger("change");
        //continueButton.SetActive(false);
        if(index < dialog.Length - 2)
        {
            index++;
            dialogText.text = "";
            StartCoroutine(Type());
        }
        else
        {
            //dialogText.text = "";
            //continueButton.SetActive(false);
        }
    }
}

