﻿using System;
using System.Collections.Generic;
using UnityEngine;

public static class DeckFactory
{
    private static Dictionary<string, Dictionary<string, object>> _cards;

    // Maps from a Card ID, to the Card's Data
    public static Dictionary<string, Dictionary<string, object>> Cards
    {
        get
        {
            if (_cards == null) _cards = LoadDemoCollection();
            return _cards;
        }
    }

    private static Dictionary<string, Dictionary<string, object>> LoadDemoCollection()
    {
        var file = Resources.Load<TextAsset>("DemoCards");
        var dict = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
        Resources.UnloadAsset(file);

        var array = (List<object>)dict["cards"];
        var result = new Dictionary<string, Dictionary<string, object>>();
        foreach (var entry in array)
        {
            var cardData = (Dictionary<string, object>)entry;
            var id = (string)cardData["id"];
            result.Add(id, cardData);
        }

        return result;
    }

    public static List<Card> CreateDeck(string fileName, int ownerIndex)
    {
        var file = Resources.Load<TextAsset>(fileName);
        var contents = MiniJSON.Json.Deserialize(file.text) as Dictionary<string, object>;
        Resources.UnloadAsset(file);

        var array = (List<object>)contents["deck"];
        var result = new List<Card>();
        foreach (var item in array)
        {
            var id = (string)item;
            var card = CreateCard(id, ownerIndex);
            result.Add(card);
        }

        return result;
    }

    public static Card CreateCard(string id, int ownerIndex)
    {
        var cardData = Cards[id];
        var card = CreateCard(cardData, ownerIndex);
        AddTarget(card, cardData);
        AddAbilities(card, cardData);
        AddMechanics(card, cardData);
        return card;
    }

    private static Card CreateCard(Dictionary<string, object> data, int ownerIndex)
    {
        var cardType = (string)data["type"];
        var type = Type.GetType(cardType);
        var instance = Activator.CreateInstance(type) as Card;
        instance.Load(data);
        instance.ownerIndex = ownerIndex;
        return instance;
    }

    private static void AddTarget(Card card, Dictionary<string, object> data)
    {
        if (data.ContainsKey("target") == false)
            return;
        var targetData = (Dictionary<string, object>)data["target"];
        var target = card.AddAspect<Target>();
        var allowedData = (Dictionary<string, object>)targetData["allowed"];
        target.allowed = new Mark(allowedData);
        var preferredData = (Dictionary<string, object>)targetData["preferred"];
        target.preferred = new Mark(preferredData);
    }

    private static void AddAbilities(Card card, Dictionary<string, object> data)
    {
        if (data.ContainsKey("abilities") == false)
            return;
        var abilities = (List<object>)data["abilities"];
        foreach (var entry in abilities)
        {
            var abilityData = (Dictionary<string, object>)entry;
            var ability = AddAbility(card, abilityData);
            AddSelector(ability, abilityData);
        }
    }

    private static Ability AddAbility(Card card, Dictionary<string, object> data)
    {
        var ability = card.AddAspect<Ability>();
        ability.actionName = (string)data["action"];
        ability.userInfo = data["info"];
        return ability;
    }

    private static void AddSelector(Ability ability, Dictionary<string, object> data)
    {
        if (data.ContainsKey("targetSelector") == false)
            return;
        var selectorData = (Dictionary<string, object>)data["targetSelector"];
        var typeName = (string)selectorData["type"];
        var type = Type.GetType(typeName);
        var instance = Activator.CreateInstance(type) as ITargetSelector;
        instance.Load(selectorData);
        ability.AddAspect(instance);
    }

    private static void AddMechanics(Card card, Dictionary<string, object> data)
    {
        if (data.ContainsKey("taunt")) card.AddAspect<Taunt>();
    }
}