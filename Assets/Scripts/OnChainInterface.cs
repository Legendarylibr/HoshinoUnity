using System;
using System.Threading.Tasks;
using UnityEngine;
using Solana.Unity.SDK;

public class OnChainInterface : MonoBehaviour
{
    [Header("Program Settings")]
    public string programId = "YOUR_PROGRAM_ID_HERE";
    public string rpcUrl = "https://api.devnet.solana.com";
    
    public static OnChainInterface Instance { get; private set; }
    
    public event Action<OnChainPetData> OnPetDataFetched;
    public event Action<string> OnTransactionComplete;
    public event Action<string> OnError;
    
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
    
    public async Task<bool> InitializePet(string ownerPublicKey)
    {
        try
        {
            Debug.Log("Initializing pet on blockchain...");
            
            if (!WalletManager.Instance.IsWalletConnected())
            {
                OnError?.Invoke("Wallet not connected");
                return false;
            }
            
            // Placeholder for actual implementation
            await Task.Delay(1000);
            OnTransactionComplete?.Invoke("mock_transaction_id");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Initialize pet failed: {e.Message}");
            OnError?.Invoke($"Initialize failed: {e.Message}");
            return false;
        }
    }
    
    public async Task<OnChainPetData> FetchPetData(string ownerPublicKey)
    {
        try
        {
            await Task.Delay(500);
            
            var mockData = new OnChainPetData
            {
                happiness = 75,
                hunger = 60,
                energy = 80,
                health = 90,
                owner = ownerPublicKey
            };
            
            OnPetDataFetched?.Invoke(mockData);
            return mockData;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to fetch pet data: {e.Message}");
            OnError?.Invoke($"Fetch failed: {e.Message}");
            return null;
        }
    }
    
    public async Task<bool> FeedPet(string ownerPublicKey)
    {
        return await SendPetAction("feed_pet", ownerPublicKey);
    }
    
    public async Task<bool> PlayWithPet(string ownerPublicKey)
    {
        return await SendPetAction("play_with_pet", ownerPublicKey);
    }
    
    private async Task<bool> SendPetAction(string action, string ownerPublicKey)
    {
        try
        {
            await Task.Delay(1000);
            Debug.Log($"Pet action {action} completed");
            OnTransactionComplete?.Invoke($"mock_tx_{action}");
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Pet action {action} failed: {e.Message}");
            OnError?.Invoke($"Action failed: {e.Message}");
            return false;
        }
    }
}
