using System;
using UnityEngine;

[Serializable]
public class PetState
{
    [Header("Pet Identity")]
    public string petName = "Hoshino";
    public string ownerPublicKey = "";
    public string petPDA = "";
    
    [Header("Core Stats")]
    [Range(0, 100)]
    public int happiness = 50;
    
    [Range(0, 100)]
    public int hunger = 50;
    
    [Range(0, 100)]
    public int energy = 100;
    
    [Range(0, 100)]
    public int health = 100;
    
    [Header("Timestamps")]
    public long lastFeedTime = 0;
    public long lastPlayTime = 0;
    public long lastSleepTime = 0;
    public long lastUpdateTime = 0;
    
    [Header("Actions")]
    public bool isSleeping = false;
    public float sleepDuration = 0f;
    public int level = 1;
    public int experience = 0;
    
    [Header("Decay Settings")]
    public float hungerDecayRate = 1f; // per hour
    public float happinessDecayRate = 0.5f; // per hour
    public float energyDecayRate = 2f; // per hour
    
    public PetState()
    {
        lastUpdateTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
    }
    
    public void UpdateFromBlockchain(OnChainPetData chainData)
    {
        happiness = chainData.happiness;
        hunger = chainData.hunger;
        energy = chainData.energy;
        health = chainData.health;
        lastFeedTime = chainData.lastFeedTime;
        lastPlayTime = chainData.lastPlayTime;
        lastSleepTime = chainData.lastSleepTime;
        lastUpdateTime = chainData.lastUpdateTime;
        isSleeping = chainData.isSleeping;
        level = chainData.level;
        experience = chainData.experience;
    }
    
    public void ApplyDecay()
    {
        long currentTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
        float hoursPassed = (currentTime - lastUpdateTime) / 3600f;
        
        if (hoursPassed > 0)
        {
            hunger = Mathf.Max(0, hunger - Mathf.RoundToInt(hungerDecayRate * hoursPassed));
            happiness = Mathf.Max(0, happiness - Mathf.RoundToInt(happinessDecayRate * hoursPassed));
            
            if (!isSleeping)
            {
                energy = Mathf.Max(0, energy - Mathf.RoundToInt(energyDecayRate * hoursPassed));
            }
            
            if (hunger < 20 || happiness < 20 || energy < 10)
            {
                health = Mathf.Max(0, health - Mathf.RoundToInt(hoursPassed * 2));
            }
            
            lastUpdateTime = currentTime;
        }
    }
    
    public bool NeedsAttention()
    {
        return hunger < 30 || happiness < 30 || energy < 20 || health < 50;
    }
    
    public string GetMoodString()
    {
        if (health < 30) return "Sick";
        if (happiness > 80 && hunger > 70 && energy > 60) return "Happy";
        if (happiness < 30) return "Sad";
        if (hunger < 30) return "Hungry";
        if (energy < 30) return "Tired";
        return "Content";
    }
}

[Serializable]
public class OnChainPetData
{
    public int happiness;
    public int hunger;
    public int energy;
    public int health;
    public long lastFeedTime;
    public long lastPlayTime;
    public long lastSleepTime;
    public long lastUpdateTime;
    public bool isSleeping;
    public int level;
    public int experience;
    public string owner;
}
