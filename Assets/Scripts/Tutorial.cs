using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour {

    [SerializeField] GameObject cardPrefab;

    ArrayList myCards, hisCards;
    Vector3 target;
    enum Modes { WELCOME, BEGINNING, GO_PEEK, YOU_CHOSE, DRAW, STACK, REPLACE, TECHNIQUES, VALUES, RED_KING, SPECIAL_CARDS, PEEK, PEEK_OPP, BLIND_SWAP, KNOW_SWAP, OBJECTIVE, NOTE_ONE, NOTE_TWO, CONFIDENT, WINNER };
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
    [SerializeField] GameObject tut_15_blindSwap;
    [SerializeField] GameObject tut_16_knowSwap;
    [SerializeField] GameObject tut_17_objective;
    [SerializeField] GameObject tut_18_noteOne;
    [SerializeField] GameObject tut_19_noteTwo;
    [SerializeField] GameObject tut_20_confident;
    [SerializeField] GameObject tut_21_winner;
    [SerializeField] GameObject green_arrow;

    const int ACE = 1;
    const int JACK = 11;
    const int QUEEN = 12;
    const int KING = 13;

    bool firstCardFlipped;
    bool secondCardFlipped;
    bool drewCard;
    bool discarded;
    bool discardMode;
    bool cardDrawn;

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
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-0.85f, -3.5f, 0f); // HANDCARD 2
        cardPrefab.tag = "6SPADES";
        myCards.Add("6SPADES");
        cardPrefab.GetComponent<TutCard>().setNum(6);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(0.85f, -3.5f, 0f); // HANDCARD 3
        cardPrefab.tag = "8DIAMONDS";
        myCards.Add("8DIAMONDS");
        cardPrefab.GetComponent<TutCard>().setNum(8);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(2.55f, -3.5f, 0f); // HANDCARD 4
        cardPrefab.tag = "10CLUBS";
        myCards.Add("10CLUBS");
        cardPrefab.GetComponent<TutCard>().setNum(10);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.CLUBS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-2.55f, 3.5f, 0f); // his cards
        cardPrefab.tag = "1HEARTS";
        hisCards.Add("1HEARTS");
        cardPrefab.GetComponent<TutCard>().setNum(1);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.HEARTS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-0.85f, 3.5f, 0f);
        cardPrefab.tag = "3SPADES";
        hisCards.Add("3SPADES");
        cardPrefab.GetComponent<TutCard>().setNum(3);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(0.85f, 3.5f, 0f);
        cardPrefab.tag = "5DIAMONDS";
        hisCards.Add("5DIAMONDS");
        cardPrefab.GetComponent<TutCard>().setNum(5);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(2.55f, 3.5f, 0f);
        cardPrefab.tag = "7CLUBS";
        hisCards.Add("7CLUBS");
        cardPrefab.GetComponent<TutCard>().setNum(7);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.CLUBS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, 0f);  // DECK POSITION (LAST)
        cardPrefab.tag = "1DIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(ACE);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.01f);
        cardPrefab.tag = "KSPADES";
        cardPrefab.GetComponent<TutCard>().setNum(KING);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.SPADES);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.02f);
        cardPrefab.tag = "QDIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(QUEEN);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.03f);
        cardPrefab.tag = "9DIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(9);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.04f);
        cardPrefab.tag = "7HEARTS";
        cardPrefab.GetComponent<TutCard>().setNum(7);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.HEARTS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.05f); 
        cardPrefab.tag = "KDIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(KING);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        target = new Vector3(-1.17f, -0.05f, -0.06f); // DECK POSITION (FIRST)
        cardPrefab.tag = "4DIAMONDS";
        cardPrefab.GetComponent<TutCard>().setNum(4);
        cardPrefab.GetComponent<TutCard>().setSuit(TutCard.Suit.DIAMONDS);
        cardPrefab.GetComponent<TutCard>().setMoveTarget(target);
        GameObject.Instantiate(cardPrefab);

        currentMode = Modes.WELCOME;
        GameObject.Instantiate(tut_1_welcome);
    }
	
	void FixedUpdate ()
    {
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
                }
            }
        }
	}

    void exeWelcome(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_1_welcome(Clone)"));
        Instantiate(tut_2_beginning);
        updateMode(Modes.BEGINNING);
    }

    void exeBeginning(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_2_beginning(Clone)"));
        Instantiate(tut_3_goPeek);
        GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().highlightCard();
        GameObject.FindGameObjectWithTag("6SPADES").GetComponent<TutCard>().highlightCard();
        Instantiate(green_arrow, new Vector3(-2.55f, -1.6f, -2f), Quaternion.identity); // creates green arrow for first card
        Instantiate(green_arrow, new Vector3(-0.87f, -1.6f, -2f), Quaternion.identity); // creates green arrow for second card
        updateMode(Modes.GO_PEEK);
    }

    void exeGoPeek(RaycastHit hit)
    {
        if (hit.transform.tag == "4HEARTS")
        {
            firstCardFlipped = true;
            GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().flipUp();
        }
        if (hit.transform.tag == "6SPADES")
        {
            secondCardFlipped = true;
            GameObject.FindGameObjectWithTag("6SPADES").GetComponent<TutCard>().flipUp();
        }

        if (firstCardFlipped && secondCardFlipped) // checks if both cards are flipped
        {
            StartCoroutine(flipCardsDown());
            GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().removeHighlightCard();
            GameObject.FindGameObjectWithTag("6SPADES").GetComponent<TutCard>().removeHighlightCard();
            GameObject[] greenArrows = GameObject.FindGameObjectsWithTag("greenArrow");
            Destroy(greenArrows[0]);
            Destroy(greenArrows[1]);
            Destroy(GameObject.Find("tut_3_goPeek(Clone)"));
            Instantiate(tut_4_youChose);
            updateMode(Modes.YOU_CHOSE);
        }
    }

    IEnumerator flipCardsDown()
    {
        yield return new WaitForSeconds(2);
        GameObject.FindGameObjectWithTag("6SPADES").GetComponent<TutCard>().flipDown();
        GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().flipDown();
    }

    void exeYouChose(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_4_youChose(Clone)"));
        Instantiate(tut_5_draw);
        Instantiate(green_arrow, new Vector3(-1.2f, 1.79f, -2f), Quaternion.identity);
        GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutCard>().highlightCard();
        updateMode(Modes.DRAW);
    }

    void exeDraw(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutCard>().setMoveTarget(activeCardPos);
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutAssetRenderer>().drawCard();
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutCard>().flipUp();
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutCard>().removeHighlightCard();
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity);
            Destroy(GameObject.Find("tut_5_draw(Clone)"));
            Instantiate(tut_6_stack);
            updateMode(Modes.STACK);
        }
    }

    void exeStack(RaycastHit hit)
    {
        if (hit.transform.tag == "Discard" && !discarded)
        {
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, 0f)); // DISCARD pos
            GameObject.FindGameObjectWithTag("4DIAMONDS").GetComponent<TutAssetRenderer>().discardCard();
            Destroy(GameObject.Find("tut_6_stack(Clone)"));
            Instantiate(tut_65_stack);
            discarded = true;
        }
        else if (hit.transform.tag == "Discard" && discarded)
        {
            foreach (string tag in myCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().highlightCard();
            }

            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(-2.61f, -1.59f, -2f), Quaternion.identity);
            Destroy(GameObject.Find("tut_6.5_stack(Clone)"));
            Instantiate(tut_675_stack);
            discardMode = true;
        }

        if (hit.transform.tag == "4HEARTS" && discarded && discardMode)
        {
            foreach (string tag in myCards)
            {
                GameObject.FindGameObjectWithTag(tag).GetComponent<TutCard>().removeHighlightCard();
            }

            discarded = false;
            GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.02f));
            GameObject.FindGameObjectWithTag("4HEARTS").GetComponent<TutCard>().flipUp();
            Destroy(GameObject.Find("tut_6.75_stack(Clone)"));
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(-1.20f, 1.77f, -2f), Quaternion.identity);
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().highlightCard(); // simulate highlight deck
            Instantiate(tut_7_replace);
            updateMode(Modes.REPLACE);
        }
    }

    void exeReplace(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().setMoveTarget(activeCardPos);
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutAssetRenderer>().drawCard();
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().flipUp();
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().removeHighlightCard();
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(2.53f, -1.66f, -2f), Quaternion.identity);

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

            Destroy(GameObject.Find("tut_7_replace(Clone)"));
            cardDrawn = false; // to use later
            Instantiate(tut_8_techniques);
            updateMode(Modes.TECHNIQUES);
            GameObject.FindGameObjectWithTag("10CLUBS").GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.03f));
            GameObject.FindGameObjectWithTag("10CLUBS").GetComponent<TutCard>().flipUp();
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().setMoveTarget(new Vector3(2.55f, -3.5f, 0f));
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutAssetRenderer>().replaceCard();
            GameObject.FindGameObjectWithTag("KDIAMONDS").GetComponent<TutCard>().flipDown();
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
        }
    }

    void exeTechniques(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_8_techniques(Clone)"));
        Instantiate(tut_9_values);
        updateMode(Modes.VALUES);
    }

    void exeValues(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_9_values(Clone)"));
        Instantiate(tut_10_redKing);
        updateMode(Modes.RED_KING);
    }

    void exeRedKing(RaycastHit hit)
    {
         Destroy(GameObject.Find("tut_10_redKing(Clone)"));
         Instantiate(tut_11_specialCards);
         Instantiate(tut_115_specialCards);
         updateMode(Modes.SPECIAL_CARDS);
    }

    void exeSpecialCards(RaycastHit hit)
    {
        Destroy(GameObject.Find("tut_11_specialCards(Clone)"));
        Destroy(GameObject.Find("tut_12_specialCards2(Clone)"));
        Instantiate(tut_13_peek);
        GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutCard>().highlightCard();
        updateMode(Modes.PEEK);
        Instantiate(green_arrow, new Vector3(-1.2f, 1.79f, -2f), Quaternion.identity);
    }

    void exePeek(RaycastHit hit)
    {
        if (hit.transform.tag == "Deck")
        {
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutAssetRenderer>().drawCard();
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutCard>().flipUp();
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutCard>().removeHighlightCard();
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutCard>().setMoveTarget(activeCardPos);
            GameObject.FindGameObjectWithTag("10CLUBS").GetComponent<TutCard>().highlightCard();
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(1.13f, 1.79f, -2f), Quaternion.identity); // points to discard
            cardDrawn = true;
        }

        if (hit.transform.tag == "Discard" && cardDrawn)
        {
            cardDrawn = false;
            discarded = true;
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Instantiate(green_arrow, new Vector3(0.82f, -1.66f, -2f), Quaternion.identity); // points to third hand card
            GameObject.FindGameObjectWithTag("10CLUBS").GetComponent<TutCard>().removeHighlightCard();
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutCard>().setMoveTarget(new Vector3(1.155f, -0.05f, -0.03f));
            GameObject.FindGameObjectWithTag("7HEARTS").GetComponent<TutAssetRenderer>().discardCard();
            
        }

        if (hit.transform.tag == "8DIAMONDS" && discarded)
        {
            discarded = false;
            GameObject.FindGameObjectWithTag("8DIAMONDS").GetComponent<TutCard>().flipUp();
            StartCoroutine(flipCardDown());
            Destroy(GameObject.FindGameObjectWithTag("greenArrow"));
            Destroy(GameObject.Find("tut_13_peek(Clone)"));
            GameObject.FindGameObjectWithTag("9DIAMONDS").GetComponent<TutCard>().highlightCard();
            Instantiate(tut_14_peekOpp);
            updateMode(Modes.PEEK_OPP);
        }
    }

    IEnumerator flipCardDown()
    {
        yield return new WaitForSeconds(2);
        GameObject.FindGameObjectWithTag("8DIAMONDS").GetComponent<TutCard>().flipDown();
    }

    void exePeekOpp(RaycastHit hit)
    {

    }

    void exeBlindSwap(RaycastHit hit)
    {

    }

    void exeKnowSwap(RaycastHit hit)
    {

    }

    void exeObjective(RaycastHit hit)
    {

    }

    void exeNoteOne(RaycastHit hit)
    {

    }

    void exeNoteTwo(RaycastHit hit)
    {

    }

    void exeConfident(RaycastHit hit)
    {

    }

    void exeWinner(RaycastHit hit)
    {

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
