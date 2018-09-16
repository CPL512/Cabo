using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Networker : NetworkManager {
    const int MAX_PLAYERS = 2;
    GameObject[] players;
    int numSpawned;

    private void Awake()
    {
        players = new GameObject[MAX_PLAYERS];
        numSpawned = 0;
    }

    public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
    {
        Transform[] pos = startPositions.ToArray();
        Vector3 spawnPos = Vector3.zero;
        Quaternion spawnRot = Quaternion.identity;
        for (int i = 0; i < pos.Length; i++)
        {
            if (pos[i].name.Equals("Spawn Position " + numSpawned))
            {
                spawnPos = pos[i].position;
                spawnRot = pos[i].rotation;
            }
        }
        GameObject player = (GameObject)Instantiate(playerPrefab, spawnPos, spawnRot);
        NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        players[numSpawned] = player;
        numSpawned++;
        player.GetComponent<PlayerScript>().setPlayerNum(numSpawned);
    }

    public override void OnStartHost()
    {
        numSpawned = 0;
        base.OnStartHost();
    }
    public override void OnStopHost()
    {
        numSpawned = 0;
        for(int i = 0; i < players.Length; i++)
        {
            players[i] = null;
        }
        base.OnStopHost();
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        numSpawned--;
        base.OnServerDisconnect(conn);
    }

    public override void OnClientConnect(NetworkConnection conn)
    {
        //do nothing since auto create player is disabled and onclientscenechanged does the work
    }

    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        ClientScene.Ready(conn);
        ClientScene.AddPlayer(client.connection, 0);
    }

    public GameObject[] getPlayers()
    {
        return players;
    }

    public int getNumSpawned()
    {
        return numSpawned;
    }
}
