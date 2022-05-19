using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TarotManager : MonoBehaviour
{
    public Queue<Tarot> deck;
    private Tarot current;

    void Start()
    {
        deck = new Queue<Tarot>();
    }

    public void AddTarot()
    {
        deck.Enqueue((Tarot)Random.Range(1, 23));
    }

    public void UseTarot()
    {
        if (current == 0) return;
    }
}

public enum Tarot
{
    None,
    TheFool,
    TheMagician,
    TheHighPriestess,
    TheEmpress,
    TheEmperor,
    TheHierophant,
    TheLovers,
    TheChariot,
    Strength,
    TheHermit,
    TheWheelOfFortune,
    Justice,
    TheHangedMan,
    Death,
    Temperance,
    TheDevil,
    TheTower,
    TheStar,
    TheMoon,
    TheSun,
    Judgement,
    TheWorld
}
