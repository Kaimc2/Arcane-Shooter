using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NameList
{
    // Set random name for AI
    public List<string> availableNames = new()
    {
        "Aziel", "Thorne", "Lyric", "Kael", "Veyra", "ShotoHak", "Sorin", "Draxis", "Merla", "Fael",
        "Kaine", "Zoryn", "Evara", "Mistral", "Zareth", "Kakada", "Ardyn", "Cerys", "PPC", "Vyra",
        "Nyssa", "Liza_Ch", "Kyra", "Orin", "Selwyn", "Zephyr", "Kaira", "Eris", "Rayne", "Malric",
        "Alaric", "Breya", "Taryn", "Elric", "Sylas", "Auren", "Fenrir", "Lyssa", "Tariel", "Pichey",
        "Nym", "Faelor", "JohnGenshin", "Zilric", "Tyris", "Mora", "LumineLover", "Drayse", "Sylwyn", "Varyn"
    };

    public string GetRandomName()
    {
        if (availableNames.Count == 0) return "Unnamed"; // Fallback if names run out

        int index = Random.Range(0, availableNames.Count);
        string name = availableNames[index];
        availableNames.RemoveAt(index); // Remove to avoid duplicates
        return name;
    }
}


