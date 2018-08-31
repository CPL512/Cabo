using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerScript : NetworkBehaviour {

    const int HAND_MAX = 6;
    enum Modes { SPAWN, BEGIN, DRAW, TURN, PEEK, SWAP, KING_DECIDING, WAITING, DOUBLING, REPLACING, CAMBRIO, OUT };
    const int PEEK_SELF_7 = 7;
    const int PEEK_SELF_8 = 8;
    const int PEEK_OTHER_9 = 9;
    const int PEEK_OTHER_10 = 10;
    const int BLIND_SWAP_J = 11;
    const int BLIND_SWAP_Q = 12;
    const int SEE_SWAP_K = 13;
    const int FLIP_TIME = 3;

    public GameObject confirmButton;
    public GameObject cancelButton;

    public GameObject disconnectButton;

    Networker net;
    Controller control;
    Deck deck;
    Discard discard;

    HandCard[] hand;
    Card activeCard;
    Vector3 activeCardPos;

    HandCard thisSwapSpot; //this player's handcard involved in swapping
    HandCard otherSwapSpot; //other player's handcard involved in swapping

    // ETHAN: added these variables to keep track of the GameObject cards we flipped/highlighted in exeBegin(RaycastHit) method
    private HandCard hc1;
    private HandCard hc2;

    HandCard doubleSpot;
    Modes oldMode; //mode player was in before tapping discard pile to double

    bool peekingSelf; //7 and 8 peek self, 9 and 10 peek others
    bool pickingSelfForSwap; //swapping has two phases, picking your own card, then picking somebody else's card
    bool swapIsBlind; //J and Q are blind swap, K is seeing swap
    
    int chosenCards;

    [SyncVar]
    bool settingCard;
    
    [SyncVar]
    Modes mode;

    Camera cam;

    public string getName()
    {
        return "deprecated";
    }

	// Use this for initialization
	void Start () {
        net = GameObject.FindGameObjectWithTag("Networker").GetComponent<Networker>();
        control = GameObject.FindGameObjectWithTag("GameController").GetComponent<Controller>();
        deck = GameObject.FindGameObjectWithTag("Deck").GetComponent<Deck>();
        discard = GameObject.FindGameObjectWithTag("Discard").GetComponent<Discard>();
        activeCardPos = new Vector3(6, 0, 0);
        settingCard = false;

        hand = new HandCard[HAND_MAX];
        HandCard[] hcScripts = GetComponentsInChildren<HandCard>();
        foreach(HandCard hc in hcScripts)
        {
            int ind = -1;
            switch(hc.transform.name)
            {
                case "Hand Card 1": // setting Hand Card 1 object to be hand[0]
                    ind = 0;
                    break;
                case "Hand Card 2":
                    ind = 1;
                    break;
                case "Hand Card 3":
                    ind = 2;
                    break;
                case "Hand Card 4":
                    ind = 3;
                    break;
                case "Extra Card 1":
                    ind = 4;
                    break;
                case "Extra Card 2":
                    ind = 5;
                    break;
            }
            hand[ind] = hc;
        }

        for(int i = 0; i < HAND_MAX; i++)
        {
            hand[i].setOwner(this);
        }

        activeCard = null;
        chosenCards = 0;
        mode = Modes.SPAWN;

        if(this.isLocalPlayer)
        {
            cam = GetComponentInChildren<Camera>();
            cam.enabled = true;
            GetComponentInChildren<AudioListener>().enabled = true;

            disconnectButton.SetActive(true);

            if (!this.isServer)
            {
                GameObject gameStarter = GameObject.FindGameObjectWithTag("GameStarter");
                if (gameStarter != null)
                {
                    gameStarter.SetActive(false);
                }
            }
        }
	}
	
	void FixedUpdate () {
        if (this.isLocalPlayer)
        {
            //Touch touch = Input.touches[0];
            //if (touch.phase == TouchPhase.Began) {
            //    Ray ray = cam.ScreenPointToRay(touch.position);
            if (Input.GetMouseButtonDown(0)) {
              Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    if(this.isServer && hit.transform.tag == "GameStarter" && deck.isDoneShuffling())
                    {
                        hit.transform.GetComponentInChildren<TextMesh>().text = "Shuffling";
                        //shuffling is already done, just need to transfer cards from shuffle deck to normal deck
                        StartCoroutine(deck.deckCards());
                    }
                    else if(hit.transform.tag == "Disconnect")
                    {
                        net.StopHost();
                    }
                    else if(this.isServer && hit.transform.tag == "QuitGame")
                    {
                        net.StopHost();
                    }
                    switch (mode)
                    {
                        case Modes.BEGIN:
                            exeBegin(hit);
                            break;
                        case Modes.DRAW:
                            exeDraw(hit);
                            break;
                        case Modes.TURN:
                            exeTurn(hit);
                            break;
                        case Modes.PEEK:
                            exePeek(hit);
                            break;
                        case Modes.SWAP:
                            exeSwap(hit);
                            break;
                        case Modes.KING_DECIDING:
                            exeKingDeciding(hit);
                            break;
                        case Modes.WAITING:
                            exeWaiting(hit);
                            break;
                        case Modes.DOUBLING:
                            exeDoubling(hit);
                            break;
                        case Modes.REPLACING:
                            exeReplacing(hit);
                            break;
                    }
                }
            }
        }
	}

    // ETHAN: function that helps flip back the selected two cards with a delay
    IEnumerator flipBack(HandCard hand1, HandCard hand2)
    {
        yield return new WaitForSeconds(FLIP_TIME);
        if (hand1 != null && hand1.getCard() != null) hand1.getCard().flipDown(); //guarantees it always flip down and not up
        if (hand2 != null && hand2.getCard() != null) hand2.getCard().flipDown();
    }

    void exeBegin(RaycastHit hit)
    {
        if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getOwner() == this
            && hit.transform.GetComponent<HandCard>().getCard() != null)
        {
            if (chosenCards == 0)
            {
                // ETHAN: added animation that flips the selected FIRST card
                hc1 = hit.transform.GetComponent<HandCard>();
                hc1.getCard().toggleCard();

                chosenCards++;
            }
            else if (chosenCards == 1 && hit.transform.GetComponent<HandCard>() != hc1)
            {
                // ETHAN: added animation that flips the selected SECOND card
                hc2 = hit.transform.GetComponent<HandCard>();
                hc2.getCard().toggleCard();

                // ETHAN: added animation that flips back the selected two cards (after flipBack() delay)
                StartCoroutine(flipBack(hc1, hc2));

                chosenCards = 0;
                this.unhighlightHand();
                CmdUpdateMode(Modes.WAITING);
            }
        }
    }

    void exeDraw(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck" && deck.isReady())
        {
            activeCard = deck.peekTop();
            this.CmdMoveToActiveCard(activeCard.gameObject);
            this.CmdPopDeck();

            // ETHAN: added animation that draws card and flips it to reveal to player
            activeCard.GetComponent<AssetRenderer>().drawCard();
            activeCard.toggleCard();

            Debug.Log("drew " + activeCard.toString());
            CmdUpdateMode(Modes.TURN);
            deck.unhighlightDeck();
            discard.highlightDiscard();
            this.highlightHand();
        }
        else if (hit.transform.tag == "Discard" && discard.size() > 0)
        {
            oldMode = mode;
            CmdUpdateMode(Modes.DOUBLING);
            deck.unhighlightDeck();
            control.highlightPlayerCardsExcept(null);
        }
        else if (hit.transform.tag == "Cambrio")
        {
            this.CmdCallCambrio();
            deck.unhighlightDeck();
        }
    }

    void exeTurn(RaycastHit hit)
    {
        if (hit.transform.tag == "Discard")
        {
            this.CmdDiscardCard(activeCard.gameObject);
            discard.unhighlightDiscard();
            this.unhighlightHand();

            // ETHAN: added animation puts card into discard pile
            activeCard.GetComponent<AssetRenderer>().discardCard();

            if (activeCard.getNum() == PEEK_SELF_7 || activeCard.getNum() == PEEK_SELF_8)
            {
                CmdUpdateMode(Modes.PEEK);
                this.highlightHand();
                cancelButton.SetActive(true);
                peekingSelf = true;

            }
            else if (activeCard.getNum() == PEEK_OTHER_9 || activeCard.getNum() == PEEK_OTHER_10)
            {
                CmdUpdateMode(Modes.PEEK);
                control.highlightPlayerCardsExcept(this);
                cancelButton.SetActive(true);
                peekingSelf = false;
            }
            else if (activeCard.getNum() == BLIND_SWAP_J || activeCard.getNum() == BLIND_SWAP_Q)
            {
                CmdUpdateMode(Modes.SWAP);
                this.highlightHand();
                cancelButton.SetActive(true);
                pickingSelfForSwap = true;
                swapIsBlind = true;
            }
            else if (activeCard.getNum() == SEE_SWAP_K)
            {
                CmdUpdateMode(Modes.SWAP);
                this.highlightHand();
                cancelButton.SetActive(true);
                pickingSelfForSwap = true;
                swapIsBlind = false;
            }
            else
            {
                activeCard = null;
                this.CmdFinishTurn();
            }
        }
        else if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getCard() != null &&
            hit.transform.GetComponent<HandCard>().getOwner() == this)
        {
            Card oldCard = hit.transform.GetComponent<HandCard>().getCard();

            int handInd = this.findHandCard(hit.transform.GetComponent<HandCard>());
            this.CmdSetCard(handInd, activeCard.gameObject);
            activeCard.flipDown();

            this.CmdDiscardCard(oldCard.gameObject);
            this.CmdFinishTurn();
            discard.unhighlightDiscard();
            this.unhighlightHand();
        }
    }

    void exePeek(RaycastHit hit)
    {
        if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getCard() != null)
        {
            if ((peekingSelf && hit.transform.GetComponent<HandCard>().getOwner() == this) ||
                (!peekingSelf && hit.transform.GetComponent<HandCard>().getOwner() != this))
            {
                hc1 = hit.transform.GetComponent<HandCard>();
                hc1.getCard().flipUp();
                StartCoroutine(flipBack(hc1, null));
                this.CmdFinishTurn();
                if (peekingSelf) this.unhighlightHand();
                else control.unhighlightPlayerCardsExcept(this);
                cancelButton.SetActive(false);
            }
        }
        else if (hit.transform.tag == "Discard" && discard.size() > 0)
        {
            cancelButton.SetActive(false);
            oldMode = mode;
            CmdUpdateMode(Modes.DOUBLING);
            control.highlightPlayerCardsExcept(null);
        }
        else if (hit.transform.tag == "Cancel")
        {
            cancelButton.SetActive(false);
            control.unhighlightPlayerCardsExcept(null);
            this.CmdFinishTurn();
        }
    }

    void exeSwap(RaycastHit hit)
    {
        if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getCard() != null)
        {
            if (pickingSelfForSwap && hit.transform.GetComponent<HandCard>().getOwner() == this)
            {
                thisSwapSpot = hit.transform.GetComponent<HandCard>();
                pickingSelfForSwap = false;
                this.unhighlightHand();
                control.highlightPlayerCardsExcept(this);
            }
            else if (!pickingSelfForSwap && hit.transform.GetComponent<HandCard>().getOwner() != this &&
                !hit.transform.GetComponent<HandCard>().getOwner().isCambrio())
            {
                otherSwapSpot = hit.transform.GetComponent<HandCard>();

                Card thisSwapCard = thisSwapSpot.getCard();
                Card otherSwapCard = otherSwapSpot.getCard();

                if (!swapIsBlind)
                {
                    thisSwapCard.flipUp();
                    otherSwapCard.flipUp();
                    confirmButton.SetActive(true);
                    cancelButton.SetActive(true);
                    control.unhighlightPlayerCardsExcept(this);
                    this.CmdUpdateMode(Modes.KING_DECIDING);
                }
                else
                {
                    int thisSwapInd = this.findHandCard(thisSwapSpot);
                    int otherSwapInd = otherSwapSpot.getOwner().findHandCard(otherSwapSpot);
                    this.CmdSetCard(thisSwapInd, otherSwapCard.gameObject);
                    this.CmdSetOtherPlayerCard(otherSwapSpot.getOwner().gameObject, otherSwapInd, thisSwapCard.gameObject);
                    pickingSelfForSwap = true;
                    thisSwapSpot = null;
                    otherSwapSpot = null;
                    cancelButton.SetActive(false);
                    control.unhighlightPlayerCardsExcept(this);
                    this.CmdFinishTurn();
                }
            }
        }
        else if (hit.transform.tag == "Discard" && discard.size() > 0 && pickingSelfForSwap) //can only double before picking the first card
        {
            oldMode = mode;
            CmdUpdateMode(Modes.DOUBLING);
            control.highlightPlayerCardsExcept(null);
            cancelButton.SetActive(false);
        }
        else if (hit.transform.tag == "Cancel")
        {
            cancelButton.SetActive(false);
            control.unhighlightPlayerCardsExcept(null);
            if (thisSwapSpot != null) thisSwapSpot.getCard().flipDown();
            if (otherSwapSpot != null) otherSwapSpot.getCard().flipDown();
            this.CmdFinishTurn();
        }
    }

    void exeKingDeciding(RaycastHit hit)
    {
        if (hit.transform.tag == "Confirm" || hit.transform.tag == "Cancel")
        {
            thisSwapSpot.getCard().flipDown();
            otherSwapSpot.getCard().flipDown();

            if (hit.transform.tag == "Confirm")
            {
                int thisSwapInd = this.findHandCard(thisSwapSpot);
                int otherSwapInd = otherSwapSpot.getOwner().findHandCard(otherSwapSpot);
                this.CmdSetCard(thisSwapInd, otherSwapSpot.getCard().gameObject);
                this.CmdSetOtherPlayerCard(otherSwapSpot.getOwner().gameObject, otherSwapInd, thisSwapSpot.getCard().gameObject);
            }

            pickingSelfForSwap = true;
            thisSwapSpot = null;
            otherSwapSpot = null;
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
            this.CmdFinishTurn();
        }
    }

    void exeWaiting(RaycastHit hit)
    {
        if (hit.transform.tag == "Discard" && discard.size() > 0)
        {
            oldMode = mode;
            CmdUpdateMode(Modes.DOUBLING);
            control.highlightPlayerCardsExcept(null);
            //Debug.Log("doubling");
        }
    }

    void exeDoubling(RaycastHit hit)
    {
        if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getCard() != null &&
            !hit.transform.GetComponent<HandCard>().getOwner().isCambrio())
        {
            control.unhighlightPlayerCardsExcept(null);
            doubleSpot = hit.transform.GetComponent<HandCard>(); //need to remember the spot across calls
            Card doubleCard = doubleSpot.getCard();
            if (doubleCard.getNum() == discard.peekTop().getNum())
            {
                this.CmdDiscardCard(doubleCard.gameObject);
                int handInd = doubleSpot.getOwner().findHandCard(doubleSpot);
                doubleSpot.getOwner().CmdSetCard(handInd, null); //remove card from handcard
                if (doubleSpot.getOwner() != this)
                {
                    CmdUpdateMode(Modes.REPLACING);
                    this.highlightHand();
                    //Debug.Log("replacing");
                }
                else
                {
                    CmdUpdateMode(oldMode);
                    revertBoardState();
                }
            }
            else //double incorrect, add top of discard to player's empty spot
            {
                this.CmdRevealOtherPlayerHandCard(doubleSpot.getOwner().gameObject, doubleSpot.getOwner().findHandCard(doubleSpot));
                int moveDestInd = FindEmptyHandCard();
                if (moveDestInd == -1) //no more empty spots, so you lose
                {
                    this.CmdUpdateMode(Modes.OUT);
                    this.CmdRevealHand();
                    this.CmdNextPlayer();
                }
                else
                {
                    this.CmdSetCard(moveDestInd, discard.peekTop().gameObject);
                    this.CmdPopDiscard();
                    CmdUpdateMode(oldMode);
                    revertBoardState();
                }
            }
        }
        else if (hit.transform.tag == "Discard")
        {
            CmdUpdateMode(oldMode);
            control.unhighlightPlayerCardsExcept(null);
            revertBoardState();
            //Debug.Log(oldMode);
        }
    }

    void exeReplacing(RaycastHit hit)
    {
        if (hit.transform.tag == "HandCard" && hit.transform.GetComponent<HandCard>().getCard() != null)
        {
            HandCard replacingSpot = hit.transform.GetComponent<HandCard>();
            if (replacingSpot.getOwner() == this)
            {
                int doubleHandInd = doubleSpot.getOwner().findHandCard(doubleSpot);
                int replaceHandInd = this.findHandCard(replacingSpot);
                doubleSpot.getOwner().CmdSetCard(doubleHandInd, replacingSpot.getCard().gameObject);
                this.CmdSetCard(replaceHandInd, null);
                CmdUpdateMode(oldMode);
                this.unhighlightHand();
                revertBoardState();
            }
        }
    }

    [Command]
    void CmdUpdateMode(Modes m)
    {
        mode = m;
    }

    [ClientRpc]
    public void RpcBegin()
    {
        mode = Modes.BEGIN;
        if (this.isLocalPlayer) this.highlightHand();
    }

    public bool isWaiting()
    {
        return mode == Modes.WAITING;
    }

    public bool isCambrio()
    {
        return mode == Modes.CAMBRIO;
    }

    public bool isOut()
    {
        return mode == Modes.OUT;
    }

    [ClientRpc]
    public void RpcStartTurn()
    {
        mode = Modes.DRAW;
        if (this.isLocalPlayer) deck.highlightDeck();
    }

    [Command]
    public void CmdPopDeck()
    {
        deck.popCard();
    }

    [Command]
    public void CmdMoveToActiveCard(GameObject card)
    {
        RpcMoveToActiveCard(card);
    }

    [ClientRpc]
    public void RpcMoveToActiveCard(GameObject card)
    {
        card.GetComponent<Card>().setMoveTarget(activeCardPos);
    }

    [Command]
    public void CmdDiscardCard(GameObject card)
    {
        discard.addCard(card);
    }

    [Command]
    public void CmdPopDiscard()
    {
        discard.RpcPopCard();
    }

    [Command]
    public void CmdNextPlayer() //starts next player turn without setting this to waiting
    {
        control.nextPlayerTurn();
    }

    [Command]
    public void CmdFinishTurn()
    {
        mode = Modes.WAITING;
        control.nextPlayerTurn();
    }

    [Command]
    public void CmdCallCambrio()
    {
        control.callCambrio();
        RpcDisableCambrio();
        mode = Modes.CAMBRIO;
        control.nextPlayerTurn();
    }

    [ClientRpc]
    public void RpcDisableCambrio()
    {
        GameObject[] cambrioCallers = GameObject.FindGameObjectsWithTag("Cambrio");
        foreach (GameObject caller in cambrioCallers)
        {
            caller.SetActive(false);
        }
    }

    public int getScore()
    {
        int score = 0;
        for(int i = 0; i < hand.Length; i++)
        {
            if (hand[i].getCard() != null) score += hand[i].getCard().getValue();
        }
        return score;
    }

    public int findHandCard(HandCard hc)
    {
        for(int i = 0; i < hand.Length; i++)
        {
            if (hand[i] == hc) return i;
        }
        return -1;
    }

    public int FindEmptyHandCard()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i].getCard() == null)
            {
                return i;
            }
        }
        return -1;
    }
    
    [Command]
    public void CmdSetCard(int handInd, GameObject card)
    {
        settingCard = true;
        RpcSetCard(handInd, card);
    }

    [ClientRpc]
    public void RpcSetCard(int handInd, GameObject card)
    {
        if (card != null)
        {
            //Debug.Log("hand at " + handInd + " set to " + card.GetComponent<Card>().toString());
            hand[handInd].setCard(card.GetComponent<Card>());
            card.GetComponent<Card>().setMoveTarget(hand[handInd].transform.position);
            card.GetComponent<Card>().flipDown();
            card.GetComponent<AssetRenderer>().replaceCard(); // plays animation to shrink back card
        }
        else
        {
            hand[handInd].setCard(null);
        }

        if(isServer) settingCard = false;
    }

    [Command]
    public void CmdSetOtherPlayerCard(GameObject player, int handInd, GameObject card)
    {
        player.GetComponent<PlayerScript>().CmdSetCard(handInd, card);
    }

    public bool isSettingCard()
    {
        return settingCard;
    }

    [ClientRpc]
    public void RpcDealCard(int handInd)
    {
        Card card = deck.drawCard();
        hand[handInd].setCard(card);
        card.setMoveTarget(hand[handInd].transform.position);
    }

    public void highlightHand()
    {
        for(int i = 0; i < hand.Length; i++)
        {
            if (hand[i].getCard() != null) hand[i].getCard().highlightCard();
        }
    }

    public void unhighlightHand()
    {
        for (int i = 0; i < hand.Length; i++)
        {
            if (hand[i].getCard() != null) hand[i].getCard().removeHighlightCard();
        }
    }

    public void revertBoardState()
    {
        switch (oldMode)
        {
            case Modes.DRAW:
                deck.highlightDeck();
                break;
            case Modes.PEEK:
                if (peekingSelf) this.highlightHand();
                else control.highlightPlayerCardsExcept(this);
                cancelButton.SetActive(true);
                break;
            case Modes.SWAP:
                if (pickingSelfForSwap) this.highlightHand();
                else control.highlightPlayerCardsExcept(this);
                cancelButton.SetActive(true);
                break;
            case Modes.WAITING:
                //nothing is active when waiting
                break;
        }
    }

    [Command]
    public void CmdRevealOtherPlayerHandCard(GameObject player, int handInd)
    {
        player.GetComponent<PlayerScript>().RpcRevealHandCard(handInd);
    }

    [ClientRpc]
    public void RpcRevealHandCard(int handInd)
    {
        hand[handInd].getCard().flipUp();
        StartCoroutine(flipBack(hand[handInd], null));
    }

    [Command]
    public void CmdRevealHand()
    {
        RpcRevealHand();
    }

    [ClientRpc]
    public void RpcRevealHand()
    {
        revealHand();
    }

    public void revealHand()
    {
        this.StopAllCoroutines(); //stop any running flipBack instances from flipping revealed hand back down
        for(int i = 0; i < hand.Length; i++)
        {
            if (hand[i] != null && hand[i].getCard() != null) hand[i].getCard().flipUp();
        }
    }
}
