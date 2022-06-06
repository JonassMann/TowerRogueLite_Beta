using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "StatBlock", menuName = "ScriptableObjects/Statblock", order = 1)]
public class StatBlock : ScriptableObject
{
    public List<ValuePair> statList = new List<ValuePair>();

    public Dictionary<string, float> stats;

    private void OnEnable()
    {
        stats = new Dictionary<string, float>();

        foreach (ValuePair v in statList)
        {
            if (stats.ContainsKey(v.key))
                Debug.LogWarning($"Duplicate key in StatBlock: {name}");

            stats[v.key] = v.value;
        }
    }

    public void Add(StatBlock a)
    {
        if (a == null) return;

        foreach (KeyValuePair<string, float> k in a.stats)
            if (!stats.ContainsKey(k.Key))
                stats[k.Key] = k.Value;
            else
            {
                if (k.Key.EndsWith("_M"))
                    stats[k.Key] *= k.Value;
                else
                    stats[k.Key] += k.Value;
            }
    }

    public void Remove(StatBlock a)
    {
        if (a == null) return;

        foreach (KeyValuePair<string, float> k in a.stats)
            if (stats.ContainsKey(k.Key))
            {
                if (k.Key.EndsWith("_M"))
                    stats[k.Key] /= k.Value;
                else
                    stats[k.Key] -= k.Value;

                if (stats[k.Key] == 0)
                    stats.Remove(k.Key);
            }
    }

    public float GetStat(string statName)
    {
        //if (!stats.ContainsKey(statName))
        //    Debug.Log($"Could not find stat: {statName}");

        float stat = 0;
        float multiplier = 1;

        if (stats.TryGetValue(statName, out float flat))
            stat = flat;

        if (stats.TryGetValue(statName + "_M", out float multi))
            multiplier = multi;

        return stat * multiplier;
    }

    public bool HasStat(string statName)
    {
        return stats.ContainsKey(statName);
    }

    [Serializable]
    public struct ValuePair
    {
        public string key;
        public float value;
    }
}