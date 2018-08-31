using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(NetworkManager))]
public class NetworkerHUD : MonoBehaviour {

    const int MATCH_SIZE = 2;

    NetworkManager manager;

	// Use this for initialization
	void Start () {
        manager = GetComponent<NetworkManager>();
	}

    public void startMM()
    {
        if (manager.matchMaker == null) manager.StartMatchMaker();
    }

    public void createOnlineGame(string roomName)
    {
        manager.matchMaker.CreateMatch(roomName, MATCH_SIZE, true, "", "", "", 0, 0, manager.OnMatchCreate);
    }

    public int joinOnlineGame(string roomName)
    {
        bool matchJoined = false;
        manager.matchMaker.ListMatches(0, 20, roomName, false, 0, 0, manager.OnMatchList);
        foreach (MatchInfoSnapshot match in manager.matches)
        {
            if (roomName.Equals(match.name))
            {
                matchJoined = true;
                manager.matchName = match.name;
                manager.matchSize = (uint)match.currentSize;
                manager.matchMaker.JoinMatch(match.networkId, "", "", "", 0, 0, manager.OnMatchJoined);
            }
        }
        if (!matchJoined)
        {
            return -1;
        }
        return 0;
    }

    public void stopMM()
    {
        if (manager.matchMaker != null) manager.StopMatchMaker();
    }

    public void createLocalGame()
    {
        manager.StartHost();
    }

    public void joinLocalGame(string ip)
    {
        manager.networkAddress = ip;
        manager.StartClient();
    }
}
