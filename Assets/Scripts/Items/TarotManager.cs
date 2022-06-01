using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System;

public class TarotManager : MonoBehaviour
{
    public Queue<Tarot> deck;
    private Tarot current;
    private Character ch;
    private PlayerInputActions playerInputActions;

    public Dictionary<string, Sprite> tarotSprites;
    public Image tarotimage;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Enable();
        playerInputActions.Player.Tarot.performed += Tarot_performed;
    }

    void Start()
    {
        deck = new Queue<Tarot>();
        ch = GetComponent<Character>();

        tarotSprites = new Dictionary<string, Sprite>();
        UnityEngine.Object[] tarotObjects = Resources.LoadAll("Tarot", typeof(Sprite));
        foreach (UnityEngine.Object o in tarotObjects)
            tarotSprites[o.name] = (Sprite)o;
    }

    private void Tarot_performed(InputAction.CallbackContext obj)
    {
        UseTarot();
    }

    public void AddTarot(int count)
    {
        for (int i = 0; i < count; i++)
        {
            //deck.Enqueue((Tarot)Random.Range(1, 23));
        }
        deck.Enqueue(Tarot.TheChariot);
        deck.Enqueue(Tarot.TheHighPriestess);
        SetCurrent();
    }

    private void SetCurrent()
    {
        if (deck.Count == 0)
        {
            current = Tarot.None;
            tarotimage.gameObject.SetActive(false);
        }
        else if (current == Tarot.None)
        {
            current = deck.Dequeue();
            tarotimage.gameObject.SetActive(true);
            Debug.Log(Enum.GetName(typeof(Tarot), current));
            tarotimage.sprite = tarotSprites[Enum.GetName(typeof(Tarot), current)];
        }
    }

    public void UseTarot()
    {
        Debug.Log(current);

        switch (current)
        {
            case Tarot.None:
                Debug.Log("No cards in deck");
                break;
            case Tarot.TheFool:
                ch.tarots |= Tarot.TheFool;
                break;
            case Tarot.TheMagician:
                ch.tarots |= Tarot.TheMagician;
                break;
            case Tarot.TheHighPriestess:
                ch.tarots |= Tarot.TheHighPriestess;
                break;
            case Tarot.TheEmpress:
                // TODO Doubles coin pickups
                break;
            case Tarot.TheEmperor:
                ch.tarots |= Tarot.TheEmperor;
                break;
            case Tarot.TheHierophant:
                // TODO Enemies stop attacking for 5 seconds
                break;
            case Tarot.TheLovers:
                ch.tarots |= Tarot.TheLovers;
                break;
            case Tarot.TheChariot:
                ch.tarots |= Tarot.TheChariot;
                break;
            case Tarot.Strength:
                ch.tarots |= Tarot.Strength;
                break;
            case Tarot.TheHermit:
                // TODO Show map
                break;
            case Tarot.TheWheelOfFortune:
                ch.tarots |= Tarot.TheWheelOfFortune;
                break;
            case Tarot.Justice:
                ch.tarots |= Tarot.Justice;
                break;
            case Tarot.TheHangedMan:
                ch.tarots |= Tarot.TheHangedMan;
                break;
            case Tarot.Death:
                // nope
                break;
            case Tarot.Temperance:
                ch.tarots |= Tarot.Temperance;
                break;
            case Tarot.TheDevil:
                // nope
                break;
            case Tarot.TheTower:
                // nope
                break;
            case Tarot.TheStar:
                // nope
                break;
            case Tarot.TheMoon:
                // nope
                break;
            case Tarot.TheSun:
                ch.tarots |= Tarot.TheSun;
                break;
            case Tarot.Judgement:
                ch.tarots |= Tarot.Judgement;
                break;
            case Tarot.TheWorld:
                AddTarot(3);
                break;
            default:
                break;
        }

        current = Tarot.None;
        SetCurrent();
    }
}

public enum Tarot
{
    None = 0,
    TheFool = 1,
    TheMagician = 2,
    TheHighPriestess = 4,
    TheEmpress = 8,
    TheEmperor = 16,
    TheHierophant = 32,
    TheLovers = 64,
    TheChariot = 128,
    Strength = 256,
    TheHermit = 512,
    TheWheelOfFortune = 1024,
    Justice = 2048,
    TheHangedMan = 4096,
    Death = 8192,
    Temperance = 16384,
    TheDevil = 32768,
    TheTower = 65536,
    TheStar = 131072,
    TheMoon = 262144,
    TheSun = 524288,
    Judgement = 1048576,
    TheWorld = 2097152
}
