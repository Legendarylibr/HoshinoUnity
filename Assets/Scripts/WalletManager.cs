using System;
using System.Threading.Tasks;
using UnityEngine;
using Solana.Unity.SDK;
using Solana.Unity.Wallet;

public class WalletManager : MonoBehaviour
{
    [Header("Wallet Settings")]
    public bool autoConnect = true;
    
    [Header("Status")]
    public bool isConnected = false;
    public string publicKey = "";
    public string walletName = "";
    
    public static WalletManager Instance { get; private set; }
    
    public event Action<string> OnWalletConnected;
    public event Action OnWalletDisconnected;
    public event Action<string> OnWalletError;
    
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
    
    public async Task<bool> ConnectWallet()
    {
        try
        {
            var result = await Web3.Instance.LoginWalletAdapter();
            
            if (result)
            {
                isConnected = true;
                publicKey = Web3.Instance.WalletBase.Account.PublicKey.Key;
                walletName = Web3.Instance.WalletBase.WalletName ?? "Unknown";
                
                OnWalletConnected?.Invoke(publicKey);
                return true;
            }
            else
            {
                OnWalletError?.Invoke("Failed to connect to wallet");
                return false;
            }
        }
        catch (Exception e)
        {
            OnWalletError?.Invoke($"Connection failed: {e.Message}");
            return false;
        }
    }
    
    public bool IsWalletConnected()
    {
        return isConnected && Web3.Instance.WalletBase?.Account != null;
    }
    
    public string GetPublicKey()
    {
        return IsWalletConnected() ? publicKey : "";
    }
    
    public string GetShortPublicKey()
    {
        if (string.IsNullOrEmpty(publicKey) || publicKey.Length < 8)
            return "";
            
        return $"{publicKey.Substring(0, 4)}...{publicKey.Substring(publicKey.Length - 4)}";
    }
}
