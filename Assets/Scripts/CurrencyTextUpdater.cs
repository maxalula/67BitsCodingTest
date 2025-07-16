using UnityEngine;
using TMPro;

public class CurrencyTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text cashText;
    /*private void OnEnable()
    {
        CurrencyManager.Instance.OnCashChanged += UpdateCashUI;
    }

    private void OnDisable()
    {
        CurrencyManager.Instance.OnCashChanged -= UpdateCashUI;
    }*/
    [SerializeField] private GameObject warningPanel;
    [SerializeField] private TMP_Text warningText;
    private void Start()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCashChanged += UpdateCashUI;
            UpdateCashUI(CurrencyManager.Instance.CurrentCash);
        }
    }

    private void UpdateCashUI(int newCash)
    {
        cashText.text = "Dinheiro: " + newCash;
    }
    private void OnDestroy()
    {
        if (CurrencyManager.Instance != null)
        {
            CurrencyManager.Instance.OnCashChanged -= UpdateCashUI;
        }
    }
    public void ShowWarningMessage(string message)
    {
        warningText.text = message;
        warningPanel.GetComponent<Animator>().Play("ShowWarningMessage", 0,0);
    }
}