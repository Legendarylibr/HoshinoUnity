using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Main UI Panels")]
    public GameObject mainPanel;
    public GameObject feedPanel;
    public GameObject chatPanel;
    public GameObject walletPanel;
    
    [Header("Pet Stats UI")]
    public Slider happinessSlider;
    public Slider hungerSlider;
    public Slider energySlider;
    public Slider healthSlider;
    
    [Header("Pet Stats Text")]
    public TextMeshProUGUI happinessText;
    public TextMeshProUGUI hungerText;
    public TextMeshProUGUI energyText;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI moodText;
    
    [Header("Pet Info")]
    public TextMeshProUGUI petNameText;
    public Image petAvatarImage;
    
    [Header("Wallet UI")]
    public Button connectWalletButton;
    public TextMeshProUGUI walletStatusText;
    public TextMeshProUGUI publicKeyText;
    
    public static UIManager Instance { get; private set; }
    
    private PetState currentPetState;
    
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
    
    private void Start()
    {
        SetupButtons();
        ShowMainPanel();
    }
    
    private void SetupButtons()
    {
        if (connectWalletButton != null)
            connectWalletButton.onClick.AddListener(() => _ = WalletManager.Instance.ConnectWallet());
    }
    
    public void ShowMainPanel()
    {
        HideAllPanels();
        if (mainPanel != null) mainPanel.SetActive(true);
    }
    
    public void ShowChatPanel()
    {
        HideAllPanels();
        if (chatPanel != null) chatPanel.SetActive(true);
    }
    
    private void HideAllPanels()
    {
        if (mainPanel != null) mainPanel.SetActive(false);
        if (feedPanel != null) feedPanel.SetActive(false);
        if (chatPanel != null) chatPanel.SetActive(false);
        if (walletPanel != null) walletPanel.SetActive(false);
    }
    
    public void UpdatePetStatsUI(PetState petState)
    {
        currentPetState = petState;
        
        if (petState == null)
        {
            if (petNameText != null) petNameText.text = "No Pet Connected";
            return;
        }
        
        // Update sliders
        if (happinessSlider != null) happinessSlider.value = petState.happiness / 100f;
        if (hungerSlider != null) hungerSlider.value = petState.hunger / 100f;
        if (energySlider != null) energySlider.value = petState.energy / 100f;
        if (healthSlider != null) healthSlider.value = petState.health / 100f;
        
        // Update text
        if (happinessText != null) happinessText.text = $"{petState.happiness}%";
        if (hungerText != null) hungerText.text = $"{petState.hunger}%";
        if (energyText != null) energyText.text = $"{petState.energy}%";
        if (healthText != null) healthText.text = $"{petState.health}%";
        
        if (moodText != null) moodText.text = petState.GetMoodString();
        if (petNameText != null) petNameText.text = petState.petName;
    }
}
