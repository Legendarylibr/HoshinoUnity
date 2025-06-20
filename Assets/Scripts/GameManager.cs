using System;
using System.Threading.Tasks;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Game Settings")]
    public float decayUpdateInterval = 300f; // 5 minutes
    public bool autoInitializePet = true;
    
    [Header("Current Pet")]
    public PetState currentPet;
    
    public static GameManager Instance { get; private set; }
    
    public event Action<PetState> OnPetStateUpdated;
    public event Action OnGameStarted;
    public event Action<string> OnGameError;
    
    private float lastDecayUpdate = 0f;
    private bool isInitialized = false;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private async void Start()
    {
        await InitializeGame();
    }
    
    private async Task InitializeGame()
    {
        try
        {
            Debug.Log("Initializing Hoshino game...");
            
            currentPet = new PetState();
            currentPet.petName = "Hoshino";
            
            isInitialized = true;
            OnGameStarted?.Invoke();
            OnPetStateUpdated?.Invoke(currentPet);
            
            Debug.Log("Game initialized successfully");
        }
        catch (Exception e)
        {
            Debug.LogError($"Game initialization failed: {e.Message}");
            OnGameError?.Invoke($"Initialization failed: {e.Message}");
        }
    }
    
    private void Update()
    {
        if (!isInitialized || currentPet == null) return;
        
        if (Time.time - lastDecayUpdate >= decayUpdateInterval)
        {
            UpdateDecay();
            lastDecayUpdate = Time.time;
        }
    }
    
    private void UpdateDecay()
    {
        if (currentPet == null) return;
        
        currentPet.ApplyDecay();
        OnPetStateUpdated?.Invoke(currentPet);
    }
    
    public PetState GetCurrentPet()
    {
        return currentPet;
    }
    
    public bool IsPetInitialized()
    {
        return isInitialized && currentPet != null;
    }
}
