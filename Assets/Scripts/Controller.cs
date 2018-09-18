using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;

public class Controller : NetworkBehaviour {

    const int MAX_PLAYERS = 2;
    const int STARTING_HAND_SIZE = 4;
    const int MAX_HAND_VALUE = 75; //two black kings and four queens + 1
    const float MOVE_DELAY = 0.05f;
    
    public GameObject quitGame;

    PlayerScript[] players;

    [SyncVar]
    int numPlayers = 0;
    [SyncVar]
    int currPlayerInd = -1;

    public GameObject deckObj;
    Deck deck;
    public GameObject discardObj;
    Discard discard;

    [SyncVar]
    bool decking;
    [SyncVar]
    bool beginning;
    [SyncVar]
    bool cambrioing; //waiting for last cards to be set before calculating score

    [SyncVar]
    bool cambrioCalled;
    [SyncVar]
    int cambrioInd;

    private void Awake()
    {
        players = new PlayerScript[MAX_PLAYERS];
        deck = deckObj.GetComponent<Deck>();
        discard = discardObj.GetComponent<Discard>();
        decking = true;
        beginning = false;
        quitGame.SetActive(false);
    }
	
	void Update () {
        if(!isServer) //only server controller needs to do things
        {
            return;
        }
        if (decking && deck.isReady() && deck.peekTop().transform.position.x == deck.transform.position.x)
        { //all cards are in deck, deal the cards
            CmdStartGame();
            decking = false;
        }
        if(beginning) //waiting for players to pick their first two cards to see
        {
            for(int i = 0; i < numPlayers; i++)
            {
                if (players[i] == null) continue;
                if (!players[i].isWaiting()) break; //if any player is still picking, wait until next call
                if(i == numPlayers - 1) //all players done picking, begin game
                {
                    beginning = false;
                    nextPlayerTurn();
                }
            }
        }
        if(cambrioing) //reached the end of the game, wait for final inputs to finish
        {
            for (int i = 0; i < numPlayers; i++)
            {
                if (players[i].isSettingCard()) break; //wait until all players are done setting cards
                if (i == numPlayers - 1)
                {
                    cambrioing = false;
                    finishGame();
                }
            }
        }
	}
    
    [Command]
    public void CmdStartGame() //tell client controllers the list of players
    {
        RpcPopulatePlayers(GameObject.FindGameObjectWithTag("Networker").GetComponent<Networker>().getPlayers());
    }

    [ClientRpc]
    public void RpcPopulatePlayers(GameObject[] p) //populate players so that highlighting works
    {
        for (int pInd = 0; pInd < p.Length; pInd++)
        {
            if (p[pInd] != null)
            {
                players[pInd] = p[pInd].GetComponent<PlayerScript>();

                if (isServer) { //host tracks number of players and dealing of cards
                    numPlayers++;
                    if (numPlayers == GameObject.FindGameObjectWithTag("Networker").GetComponent<Networker>().getNumSpawned())
                    {
                        StartCoroutine(dealCards());
                    }
                }
            }
        }
    }

    IEnumerator dealCards() //deal cards to each player with a delay between each card
    {
        beginning = true;
        foreach (PlayerScript player in players)
        {
            if (player == null) continue;
            for (int i = 0; i < STARTING_HAND_SIZE; i++)
            {
                player.RpcDealCard(i);
                yield return new WaitForSeconds(MOVE_DELAY); //adds a delay between each card being dealt
            }
            player.RpcBegin();
        }
    }

    public void nextPlayerTurn() //start next player's turn, if there is valid player to start
    {
        if(!isServer) //only the server should execute this
        {
            return;
        }
        currPlayerInd = (currPlayerInd + 1) % numPlayers; //move to next player index
        if (cambrioCalled && currPlayerInd == cambrioInd) //if next player is the one who called cambrio, finish
        {
            cambrioing = true;
        }
        else
        {
            if (deck.size() == 0) { //if deck is empty, shuffle the discard back into it
                deck.setDeckNotReady();
                discard.RpcShuffleIntoDeck(deck.gameObject);
            }

            if (players[currPlayerInd].isOut()) //skip players who are out
            {
                int i = (currPlayerInd + 1) % numPlayers; //look for a player that isn't out 
                while(i != currPlayerInd)
                {
                    if(!players[i].isOut()) //non out player found, have them start their turn
                    {
                        currPlayerInd = i - 1;
                        nextPlayerTurn();
                        return;
                    }
                    i = (i + 1) & numPlayers;
                }
                cambrioing = true; //no non out players found so all players are out, finish the game
            }
            else //start the players turn
            {
                players[currPlayerInd].RpcStartTurn();
            }
        }
    }

    public void highlightPlayerCardsExcept(PlayerScript playerToAvoid) //highlight the hands of unspecified players
    {
        for(int i = 0; i < numPlayers; i++)
        {
            if (players[i] != null && (playerToAvoid == null || players[i] != playerToAvoid))
            {
                players[i].highlightHand();
            }
        }
    }

    public void unhighlightPlayerCardsExcept(PlayerScript playerToAvoid) //unhighlight the hands of unspecified players
    {
        for (int i = 0; i < numPlayers; i++)
        {
            if (players[i] != null && (playerToAvoid == null || players[i] != playerToAvoid)) players[i].unhighlightHand();
        }
    }

    public void callCambrio() //calls cambrio for the current player
    {
        cambrioCalled = true;
        cambrioInd = currPlayerInd;
    }

    [ClientRpc]
    public void RpcRevealHands() //tells all players to reveal their hands
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i] != null) players[i].revealHand();
        }
    }

    public void finishGame() //game has finished, reveal hands, calculate scores, announce winner(s)
    {
        RpcRevealHands();
        int minScore = MAX_HAND_VALUE;
        Queue<int> winIndices = new Queue<int>();
        Queue<int> loseIndices = new Queue<int>();
        int score;
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i] == null) continue;
            score = players[i].getScore();
            if(score < minScore)
            {
                minScore = score;
                while(winIndices.Count > 0)
                {
                    loseIndices.Enqueue(winIndices.Dequeue());
                }
                winIndices.Enqueue(i);
            }
            else if(score == minScore)
            {
                winIndices.Enqueue(i);
            }
            else
            {
                loseIndices.Enqueue(i);
            }
        }

        if(winIndices.Count > 1)
        {
            while(winIndices.Count > 0)
            {
                players[winIndices.Dequeue()].RpcTie();
            }
        }
        else
        {
            players[winIndices.Dequeue()].RpcWin();
        }

        while (loseIndices.Count > 0)
        {
            players[loseIndices.Dequeue()].RpcLose();
        }

        quitGame.SetActive(true); //allow players to quit back to main menu
    }
}
