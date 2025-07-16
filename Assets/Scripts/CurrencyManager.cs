using System;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }    
    [SerializeField] private int currentCash;
    public int CurrentCash => currentCash;
    public event Action<int> OnCashChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddCash(int amount)
    {
        UpdateCash(currentCash += amount);
        
    }
    public void SpendCash(int amount)
    {
        if (amount < 0 || currentCash < amount)
        {
            return;
        }
        UpdateCash(currentCash -= amount);
    }
    private void UpdateCash(int amount)
    {
        currentCash = amount;
        OnCashChanged?.Invoke(CurrentCash);
    }
}