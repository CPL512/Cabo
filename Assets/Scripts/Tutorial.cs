using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

    [SerializeField] GameObject cardPrefab;

    ArrayList myCards, hisCards;
    Vector3 target;
    enum Modes { WELCOME, BEGINNING, GO_PEEK, YOU_CHOSE, DRAW, STACK, REPLACE, TECHNIQUES, VALUES, RED_KING, SPECIAL_CARDS, PEEK, PEEK_OPP, BLIND_SWAP, KNOW_SWAP, OBJECTIVE, NOTE_ONE, NOTE_TWO, CONFIDENT, WINNER, CONGRATS };
    [SerializeField] Modes currentMode;

    [SerializeField] GameObject tut_1_welcome;
    [SerializeField] GameObject tut_2_beginning;
    [SerializeField] GameObject tut_3_goPeek;
    [SerializeField] GameObject tut_4_youChose;
    [SerializeField] GameObject tut_5_draw;
    [SerializeField] GameObject tut_6_stack;
    [SerializeField] GameObject tut_65_stack;
    [SerializeField] GameObject tut_675_stack;
    [SerializeField] GameObject tut_7_replace;
    [SerializeField] GameObject tut_8_techniques;
    [SerializeField] GameObject tut_9_values;
    [SerializeField] GameObject tut_10_redKing;
    [SerializeField] GameObject tut_11_specialCards;
    [SerializeField] GameObject tut_115_specialCards;
    [SerializeField] GameObject tut_13_peek;
    [SerializeField] GameObject tut_14_peekOpp;
    [SerializeField] GameObject tut_145_peekOpp;
    [SerializeField] GameObject tut_1475_peekOpp;
    [SerializeField] GameObject tut_15_blindSwap;
    [SerializeField] GameObject tut_16_knowSwap;
    [SerializeField] GameObject tut_17_objective;
    [SerializeField] GameObject tut_18_noteOne;
    [SerializeField] GameObject tut_19_noteTwo;
    [SerializeField] GameObject tut_20_confident;
    [SerializeField] GameObject tut_21_winner;
    [SerializeField] GameObject tut_22_congrats;
    [SerializeField] GameObject green_arrow;

    GameObject hearts4;
    GameObject spades6;
    GameObject diamonds8;
    GameObject clubs10;
    GameObject hearts1;
    GameObject spades3;
    GameObject diamonds5;
    GameObject diamonds9;
    GameObject diamonds1;
    GameObject spadesK;
    GameObject diamondsQ;
    GameObject clubs9;
    GameObject hearts7;
    GameObject diamondsK;
    GameObject diamonds4;

    GameObject bubbleInst;
    GameObject bubbleInst2;
    GameObject greenArrowInst;
    GameObject greenArrowInst2;

    const int ACE = 1;
    const int JACK = 11;
    const int QUEEN = 12;
    const int KING = 13;

    bool firstCardFlipped;
    bool secondCardFlipped;
    bool selectedFirst;
    bool drewCard;
    bool discarded;
    bool discardMode;
    bool cardDrawn;
    bool cp1;
    bool cp2;
    bool cp3;

    Vector3 activeCardPos = new Vector3(5.75f, 0, 0);

    void Start ()
    {
        myCards = new ArrayList();
        hisCards = new ArrayList();

        target = new Vector3(-2.55f, -3.5f, 0f); // HANDCARD 1 (MY CARDS)
        cardPrefab.tag = "4HEARTS";
        myCards.Add("4HEARTS");
        cardPrefab.GetComponent<TutCard>().setNum(4);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.HEARTS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        hearts4 =  GameObject.Instantiate(cardPrefab);

        target = new Vector3(-0.85f, -3.5f, 0f); // HANDCARD 2
        cardPrefab.tag = "6SPADES";
        myCards.Add("6SPADES");
        cardPrefab.GetComponent<TutCard>().setNum(6);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        spades6 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(0.85f, -3.5f, 0f); // HANDCARD 3
        cardPrefab.tag = "8DIAMONDS";
        myCards.Add("8DIAMONDS");
        cardPrefab.GetComponent<TutCard>().setNum(8);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamonds8 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(2.55f, -3.5f, 0f); // HANDCARD 4
        cardPrefab.tag = "10CLUBS";
        myCards.Add("10CLUBS");
        cardPrefab.GetComponent<TutCard>().setNum(10);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.CLUBS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        clubs10 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-2.55f, 3.5f, 0f); // his cards
        cardPrefab.tag = "1HEARTS";
        hisCards.Add("1HEARTS");
        cardPrefab.GetComponent<TutCard>().setNum(1);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.HEARTS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        hearts1 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-0.85f, 3.5f, 0f);
        cardPrefab.tag = "3SPADES";
        hisCards.Add("3SPADES");
        cardPrefab.GetComponent<TutCard>().setNum(3);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        spades3 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(0.85f, 3.5f, 0f);
        cardPrefab.tag = "5DIAMONDS";
        hisCards.Add("5DIAMONDS");
        cardPrefab.GetComponent<TutCard>().setNum(5);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamonds5 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(2.55f, 3.5f, 0f);
        cardPrefab.tag = "9DIAMONDS";
        hisCards.Add("9DIAMONDS");
        cardPrefab.GetComponent<TutCard>().setNum(9);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamonds9 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, 0f);  // DECK POSITION (LAST)
        cardPrefab.tag = "1DIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(ACE);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamonds1 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.01f);
        cardPrefab.tag = "KSPADES";
        cardPrefab.GetComponent<TutCard>().setNum(KING);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        spadesK = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.02f);
        cardPrefab.tag = "QDIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(QUEEN);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamondsQ = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.03f);
        cardPrefab.tag = "9CLUBS";
        cardPrefab.GetComponent<TutCard>().setNum(9);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.CLUBS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        clubs9 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.04f);
        cardPrefab.tag = "7HEARTS";
        cardPrefab.GetComponent<TutCard>().setNum(7);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.HEARTS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        hearts7 = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.05f); 
        cardPrefab.tag = "KDIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(KING);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamondsK = GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.06f); // DECK POSITION (FIRST)
        cardPrefab.tag = "4DIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(4);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        diamonds4 = GameObject.Instantiate(cardPrefab);

        currentMode = Modes.WELCOME;
        bubbleInst = GameObject.Instantiate(tut_1_welcome);
    }
	
	void FixedUpdate ()
    {
        //Touch touch = Input.touches[0];
        //if (touch.phase == TouchPhase.Began) {
        //    Ray ray = cam.ScreenPointToRay(touch.position);
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                switch (currentMode)
                {
                    case Modes.WELCOME:
                        exeWelcome(hit);
                        break;
                    case Modes.BEGINNING:
                        exeBeginning(hit);
                        break;
                    case Modes.GO_PEEK:
                        exeGoPeek(hit);
                        break;
                    case Modes.YOU_CHOSE:
                        exeYouChose(hit);
                        break;
                    case Modes.DRAW:
                        exeDraw(hit);
                        break;
                    case Modes.STACK:
                        exeStack(hit);
                        break;
                    case Modes.REPLACE:
                        exeReplace(hit);
                        break;
                    case Modes.TECHNIQUES:
                        exeTechniques(hit);
                        break;
                    case Modes.VALUES:
                        exeValues(hit);
                        break;
                    case Modes.RED_KING:
                        exeRedKing(hit);
                        break;
                    case Modes.SPECIAL_CARDS:
                        exeSpecialCards(hit);
                        break;
                    case Modes.PEEK:
                        exePeek(hit);
                        break;
                    case Modes.PEEK_OPP:
                        exePeekOpp(hit);
                        break;
                    case Modes.BLIND_SWAP:
                        exeBlindSwap(hit);
                        break;
                    case Modes.KNOW_SWAP:
                        exeKnowSwap(hit);
                        break;
                    case Modes.OBJECTIVE:
                        exeObjective(hit);
                        break;
                    case Modes.NOTE_ONE:
                        exeNoteOne(hit);
                        break;
                    case Modes.NOTE_TWO:
                        exeNoteTwo(hit);
                        break;
                    case Modes.CONFIDENT:
                        exeConfident(hit);
                        break;
                    case Modes.WINNER:
                        exeWinner(hit);
                        break;
                    case Modes.CONGRATS:
                        exeCongrats(hit);
                        break;
                }
            }
        }
	}

    void exeWelcome(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_2_beginning);
        updateMode(Modes.BEGINNING);
    }

    void exeBeginning(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_3_goPeek);
        hearts4.GetComponent<TutCard>().highlightCard();
        spades6.GetComponent<TutCard>().highlightCard();
        greenArrowInst = Instantiate(green_arrow, new Vector3(-2.55f, -1.6f, -2f), Quaternion.identity); // creates green arrow for first card
        greenArrowInst2 = Instantiate(green_arrow, new Vector3(-0.87f, -1.6f, -2f), Quaternion.identity); // creates green arrow for second card
        updateMode(Modes.GO_PEEK);
    }

    void exeGoPeek(RaycastHit hit)
    {
        if (hit.transform.tag == "4HEARTS")
        {
            firstCardFlipped = true;
            hearts4.GetComponent<TutCard>().flipUp();
        }
        if (hit.transform.tag == "6SPADES")
        {
            secondCardFlipped = true;
            spades6.GetComponent<TutCard>().flipUp();
        }

        if (firstCardFlipped && secondCardFlipped) // checks if both cards are flipped
        {
            StartCoroutine(flipCardsDown());
            hearts4.GetComponent<TutCard>().removeHighlightCard();
            spades6.GetComponent<TutCard>().removeHighlightCard();
            Destroy(greenArrowInst);
            Destroy(greenArrowInst2);
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_4_youChose);
            updateMode(Modes.YOU_CHOSE);
        }
    }

    IEnumerator flipCardsDown()
    {
        yield return new WaitForSeconds(2);
        spades6.GetComponent<TutCard>().flipDown();
        hearts4.GetComponent<TutCard>().flipDown();
    }

    void exeYouChose(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_5_draw);
        greenArrowInst = Instantiate(green_arrow, new Vector3(-1.2f, 1.79f, -2f), Quaternion.identity);
        diamonds4.GetComponent<TutCard>().highlightCard();
        updateMode(Modes.DRAW);
    }

    void exeDraw(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            diamonds4.GetComponent<TutCard>().setMoveTarget(activeCardPos);
            diamonds4.GetComponent<TutAssetRenderer>().drawCard();
            diamonds4.GetComponent<TutCard>().flipUp();
            diamonds4.GetComponent<TutCard>().removeHighlightCard();
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity);
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_6_stack);
            updateMode(Modes.STACK);
        }
    }

    void exeStack(RaycastHit hit)
    {
        if (hit.transform.tag == "Discard" && !discarded)
        {
            diamonds4.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, 0f)); // DISCARD pos
            diamonds4.GetComponent<TutAssetRenderer>().discardCard();
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_65_stack);
            discarded = true;
        }
        else if (hit.transform.tag == "Discard" && discarded)
        {
            foreach (string tag in myCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().highlightCard();
            }

            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(-2.61f, -1.59f, -2f), Quaternion.identity);
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_675_stack);
            discardMode = true;
        }

        if (hit.transform.tag == "4HEARTS" && discarded && discardMode)
        {
            foreach (string tag in myCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().removeHighlightCard();
            }

            discarded = false;
            hearts4.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.02f));
            hearts4.GetComponent<TutCard>().flipUp();
            Destroy(bubbleInst);
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(-1.20f, 1.77f, -2f), Quaternion.identity);
            diamondsK.GetComponent<TutCard>().highlightCard(); // simulate highlight deck
            bubbleInst = Instantiate(tut_7_replace);
            updateMode(Modes.REPLACE);
        }
    }

    void exeReplace(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            diamondsK.GetComponent<TutCard>().setMoveTarget(activeCardPos);
            diamondsK.GetComponent<TutAssetRenderer>().drawCard();
            diamondsK.GetComponent<TutCard>().flipUp();
            diamondsK.GetComponent<TutCard>().removeHighlightCard();
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(2.53f, -1.66f, -2f), Quaternion.identity);

            foreach (string tag in myCards)
            {
                if (tag != "4HEARTS")
                {
                    GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().highlightCard();
                }
            }

            cardDrawn = true;
        }

        if (hit.transform.tag == "10CLUBS" && cardDrawn)
        {
            foreach (string tag in myCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().removeHighlightCard();
            }

            Destroy(bubbleInst);
            cardDrawn = false; // to use later
            bubbleInst = Instantiate(tut_8_techniques);
            updateMode(Modes.TECHNIQUES);
            clubs10.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.03f));
            clubs10.GetComponent<TutCard>().flipUp();
            diamondsK.GetComponent<TutCard>().setMoveTarget(new Vector3(2.55f, -3.5f, 0f));
            diamondsK.GetComponent<TutAssetRenderer>().replaceCard();
            diamondsK.GetComponent<TutCard>().flipDown();
            Destroy(greenArrowInst);
        }
    }

    void exeTechniques(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_9_values);
        updateMode(Modes.VALUES);
    }

    void exeValues(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_10_redKing);
        updateMode(Modes.RED_KING);
    }

    void exeRedKing(RaycastHit hit)
    {
         Destroy(bubbleInst);
         bubbleInst = Instantiate(tut_11_specialCards);
         bubbleInst2 = Instantiate(tut_115_specialCards);
         updateMode(Modes.SPECIAL_CARDS);
    }

    void exeSpecialCards(RaycastHit hit)
    {
        Destroy(bubbleInst);
        Destroy(bubbleInst2);
        bubbleInst = Instantiate(tut_13_peek);
        hearts7.GetComponent<TutCard>().highlightCard();
        updateMode(Modes.PEEK);
        greenArrowInst = Instantiate(green_arrow, new Vector3(-1.2f, 1.79f, -2f), Quaternion.identity);
    }

    void exePeek(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            hearts7.GetComponent<TutAssetRenderer>().drawCard();
            hearts7.GetComponent<TutCard>().flipUp();
            hearts7.GetComponent<TutCard>().removeHighlightCard();
            hearts7.GetComponent<TutCard>().setMoveTarget(activeCardPos);
            clubs10.GetComponent<TutCard>().highlightCard();
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity); // points to discard
            cardDrawn = true;
        }

        if (hit.transform.tag == "Discard" && cardDrawn)
        {
            cardDrawn = false;
            discarded = true;
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(0.82f, -1.66f, -2f), Quaternion.identity); // points to third hand card
            clubs10.GetComponent<TutCard>().removeHighlightCard();
            hearts7.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.04f));
            hearts7.GetComponent<TutAssetRenderer>().discardCard();
            
        }

        if (hit.transform.tag == "8DIAMONDS" && discarded)
        {
            discarded = false;
            diamonds8.GetComponent<TutCard>().flipUp();
            StartCoroutine(flipCardDown(diamonds8.GetComponent<TutCard>()));
            Destroy(greenArrowInst);
            Destroy(bubbleInst);
            clubs9.GetComponent<TutCard>().highlightCard();
            bubbleInst = Instantiate(tut_14_peekOpp);
            updateMode(Modes.PEEK_OPP);
        }
    }

    IEnumerator flipCardDown(TutCard card)
    {
        yield return new WaitForSeconds(2);
        card.flipDown();
    }

    void exePeekOpp(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            clubs9.GetComponent<TutAssetRenderer>().drawCard();
            clubs9.GetComponent<TutCard>().flipUp();
            clubs9.GetComponent<TutCard>().removeHighlightCard();
            clubs9.GetComponent<TutCard>().setMoveTarget(activeCardPos);
            hearts7.GetComponent<TutCard>().highlightCard(); // highlight discard
            greenArrowInst = Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity); // points to discard
            cardDrawn = true;
        }

        if (hit.transform.tag == "Discard" && cardDrawn)
        {
            cardDrawn = false;
            discarded = true;
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(2.54f, 4.56f, -2f), Quaternion.identity); // points to fourth card of opp
            hearts7.GetComponent<TutCard>().removeHighlightCard();
            clubs9.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.05f)); // deck position
            clubs9.GetComponent<TutAssetRenderer>().discardCard();
        }

        if (hit.transform.tag == "9DIAMONDS" && discarded)
        {
            discarded = false;
            diamonds9.GetComponent<TutCard>().flipUp();
            StartCoroutine(flipCardDown(diamonds9.GetComponent<TutCard>()));
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity);  // point to discard
            cp1 = true;
        }

        if (hit.transform.tag == "Discard" && cp1)
        {
            foreach (string tag in hisCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().highlightCard();
            }
            cp1 = false;
            cp2 = true;
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(2.54f, 4.46f, -2f), Quaternion.identity);  // point to 9 of clubs
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_145_peekOpp);
        }

        if (hit.transform.tag == "9DIAMONDS" && cp2)
        {
            cp2 = false;
            foreach (string tag in hisCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().removeHighlightCard();
            }
            diamonds9.GetComponent<TutCard>().flipUp();
            diamonds9.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.06f)); // deck position
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_1475_peekOpp);

            diamondsK.GetComponent<TutCard>().highlightCard();
            diamonds8.GetComponent<TutCard>().highlightCard();
            spades6.GetComponent<TutCard>().highlightCard();

            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(-0.86f, -1.71f, -2f), Quaternion.identity);
            cp3 = true;
        }

        if (hit.transform.tag == "6SPADES" && cp3)
        {
            cp3 = false;

            diamondsK.GetComponent<TutCard>().removeHighlightCard();
            diamonds8.GetComponent<TutCard>().removeHighlightCard();
            spades6.GetComponent<TutCard>().removeHighlightCard();

            diamonds9.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.07f));
            diamonds9.GetComponent<TutCard>().flipUp();

            spades6.GetComponent<TutCard>().setMoveTarget(new Vector3(2.55f, 3.5f, -0.06f)); // go to opp empty hand
            Destroy(greenArrowInst);

            Destroy(bubbleInst);

            diamondsQ.GetComponent<TutCard>().highlightCard();
            bubbleInst = Instantiate(tut_15_blindSwap);
            updateMode(Modes.BLIND_SWAP);
        }

    }

    void exeBlindSwap(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            diamondsQ.GetComponent<TutAssetRenderer>().drawCard();
            diamondsQ.GetComponent<TutCard>().flipUp();
            diamondsQ.GetComponent<TutCard>().removeHighlightCard();
            diamondsQ.GetComponent<TutCard>().setMoveTarget(activeCardPos);
            diamonds9.GetComponent<TutCard>().highlightCard(); // highlight discard
            greenArrowInst = Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity); // points to discard
            cardDrawn = true;
        }

        if (hit.transform.tag == "Discard" && cardDrawn)
        {
            cardDrawn = false;
            discarded = true;
            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(-2.56f, 4.4f, -2f), Quaternion.identity); // points to fourth card of opp
            diamonds9.GetComponent<TutCard>().removeHighlightCard();
            diamondsQ.GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.08f)); // discard position
            diamondsQ.GetComponent<TutAssetRenderer>().discardCard();
            hearts1.GetComponent<TutCard>().highlightCard();
            spades3.GetComponent<TutCard>().highlightCard();
            diamonds5.GetComponent<TutCard>().highlightCard();
            spades6.GetComponent<TutCard>().highlightCard();
        }

        if (hit.transform.tag == "1HEARTS")
        {
            hearts1.GetComponent<TutCard>().removeHighlightCard();
            spades3.GetComponent<TutCard>().removeHighlightCard();
            diamonds5.GetComponent<TutCard>().removeHighlightCard();
            spades6.GetComponent<TutCard>().removeHighlightCard();

            Destroy(greenArrowInst);
            greenArrowInst = Instantiate(green_arrow, new Vector3(0.83f, -1.77f, -2f), Quaternion.identity); // points to third card of you
            selectedFirst = true;
            diamonds8.GetComponent<TutCard>().highlightCard();
            diamondsK.GetComponent<TutCard>().highlightCard();
        }

        if (hit.transform.tag == "8DIAMONDS" && selectedFirst)
        {
            selectedFirst = false;
            Destroy(greenArrowInst);
            diamonds8.GetComponent<TutCard>().removeHighlightCard();
            diamondsK.GetComponent<TutCard>().removeHighlightCard();
            diamonds8.GetComponent<TutCard>().setMoveTarget(new Vector3(-2.55f, 3.5f, -0.09f)); // move 8D to 1H
            hearts1.GetComponent<TutCard>().setMoveTarget(new Vector3(0.85f, -3.5f, -0.08f)); // move 1H to 8D
            Destroy(bubbleInst);
            bubbleInst = Instantiate(tut_16_knowSwap);
            updateMode(Modes.KNOW_SWAP);
        }
    }

    void exeKnowSwap(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_17_objective);
        updateMode(Modes.OBJECTIVE);
    }

    void exeObjective(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_18_noteOne);
        updateMode(Modes.NOTE_ONE);
    }

    void exeNoteOne(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_19_noteTwo);
        updateMode(Modes.NOTE_TWO);
    }

    void exeNoteTwo(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_20_confident);
        updateMode(Modes.CONFIDENT);
    }

    void exeConfident(RaycastHit hit)
    {
        Destroy(bubbleInst);
        bubbleInst = Instantiate(tut_21_winner);
        greenArrowInst = Instantiate(green_arrow, new Vector3(6.73f, -1.95f, -1f), Quaternion.identity);
        updateMode(Modes.WINNER);
    }

    void exeWinner(RaycastHit hit)
    {
        if (hit.transform.tag == "CAMBRIOBUTTON")
        {
            Destroy(bubbleInst);
            Destroy(greenArrowInst);
            diamonds8.GetComponent<TutCard>().flipUp();
            spades3.GetComponent<TutCard>().flipUp();
            diamonds5.GetComponent<TutCard>().flipUp();
            spades6.GetComponent<TutCard>().flipUp();
            hearts1.GetComponent<TutCard>().flipUp();
            diamondsK.GetComponent<TutCard>().flipUp();
            bubbleInst = Instantiate(tut_22_congrats);
            updateMode(Modes.CONGRATS);
        }
    }

    void exeCongrats(RaycastHit hit)
    {
        quitTutorial();
    }

    void updateMode(Modes newMode)
    {
        currentMode = newMode;
    }

    public void quitTutorial()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
