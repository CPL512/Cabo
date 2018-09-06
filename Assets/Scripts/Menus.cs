using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menus : MonoBehaviour {

    const int MESSAGE_TIME = 2;

    NetworkerHUD networkhud;
    
    public Canvas onlineMenu;
    public Canvas localMenu;

    public InputField onlineCreateRoomField;
    public InputField onlineJoinRoomField;
    public InputField localJoinIPField;

    public Text onlineEnterRoomText;
    public Text onlineNoRoomText;
    public Text localEnterIPText;

    // Use this for initialization
    void Awake () {
        networkhud = GameObject.FindGameObjectWithTag("Networker").GetComponent<NetworkerHUD>();
        
        onlineMenu.enabled = false;
        localMenu.enabled = false;

        onlineEnterRoomText.enabled = false;
        onlineNoRoomText.enabled = false;
        localEnterIPText.enabled = false;
    }

    public void openOnlineMenu()
    {
        onlineMenu.enabled = true;
        networkhud.startMM();
    }

    public void createOnlineGame()
    {
        if (onlineCreateRoomField.text != "")
        {
            networkhud.createOnlineGame(onlineCreateRoomField.text);
        }
        else
        {
            onlineEnterRoomText.enabled = true;
            StartCoroutine(disableText(onlineEnterRoomText));
        }
    }

    public void joinOnlineGame()
    {
        if (onlineJoinRoomField.text != "")
        {
            if(networkhud.joinOnlineGame(onlineJoinRoomField.text) == -1)
            {
                onlineNoRoomText.enabled = true;
                StartCoroutine(disableText(onlineNoRoomText));
            }
        }
        else
        {
            onlineEnterRoomText.enabled = true;
            StartCoroutine(disableText(onlineEnterRoomText));
        }
    }

    public void closeOnlineMenu()
    {
        onlineMenu.enabled = false;
        networkhud.stopMM();
    }

    public void openLocalMenu()
    {
        localMenu.enabled = true;
    }

    public void createLocalGame()
    {
        networkhud.createLocalGame();
    }

    public void joinLocalGame()
    {
        if (localJoinIPField.text != "")
        {
            networkhud.joinLocalGame(localJoinIPField.text);
        }
        else
        {
            localEnterIPText.enabled = true;
            StartCoroutine(disableText(localEnterIPText));
        }
    }

    public void closeLocalMenu()
    {
        localMenu.enabled = false;
    }

    public void openTutorial()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    IEnumerator disableText(Text t)
    {
        yield return new WaitForSeconds(MESSAGE_TIME);
        if (t != null) t.enabled = false;
    }
}
