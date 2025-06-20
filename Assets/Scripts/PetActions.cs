using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class PetActions : MonoBehaviour
{
    [Header("Action Buttons")]
    public Button feedButton;
    public Button playButton;
    public Button sleepButton;
    public Button chatButton;
    
    [Header("Action Settings")]
    public int feedCooldownMinutes = 30;
    public int playCooldownMinutes = 15;
    
    private PetState currentPetState;
    private bool isProcessingAction = false;
    
    public event Action<string> OnActionStarted;
    public event Action<string> OnActionCompleted;
    public event Action<string> OnActionFailed;
    
    private void Start()
    {
        SetupButtons();
    }
    
    private void SetupButtons()
    {
        if (feedButton != null)
            feedButton.onClick.AddListener(() => _ = FeedPet());
            
        if (playButton != null)
            playButton.onClick.AddListener(() => _ = PlayWithPet());
            
        if (chatButton != null)
            chatButton.onClick.AddListener(() => OpenChat());
    }
    
    public async Task FeedPet()
    {
        if (isProcessingAction) return;
        
        isProcessingAction = true;
        OnActionStarted?.Invoke("Feeding pet...");
        
        try
        {
            bool success = await OnChainInterface.Instance.FeedPet(WalletManager.Instance.GetPublicKey());
            
            if (success)
            {
                OnActionCompleted?.Invoke("Pet fed successfully!");
            }
            else
            {
                OnActionFailed?.Invoke("Failed to feed pet on blockchain");
            }
        }
        catch (Exception e)
        {
            OnActionFailed?.Invoke($"Feed failed: {e.Message}");
        }
        finally
        {
            isProcessingAction = false;
        }
    }
    
    public async Task PlayWithPet()
    {
        if (isProcessingAction) return;
        
        isProcessingAction = true;
        OnActionStarted?.Invoke("Playing with pet...");
        
        try
        {
            bool success = await OnChainInterface.Instance.PlayWithPet(WalletManager.Instance.GetPublicKey());
            
            if (success)
            {
                OnActionCompleted?.Invoke("Had fun playing!");
            }
            else
            {
                OnActionFailed?.Invoke("Failed to play on blockchain");
            }
        }
        catch (Exception e)
        {
            OnActionFailed?.Invoke($"Play failed: {e.Message}");
        }
        finally
        {
            isProcessingAction = false;
        }
    }
    
    public void OpenChat()
    {
        OnActionStarted?.Invoke("Opening chat...");
        OnActionCompleted?.Invoke("Chat opened");
    }
}
