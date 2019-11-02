using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGui : MonoBehaviour
{
    [SerializeField] Text moneyText;

    private ClientController gameController;
    private void Awake()
    {
        gameController = ClientController.Instance;
    }

    private void FixedUpdate()
    {
        SetMoneyText(gameController.Money);
    }

    private void SetMoneyText(float amount)
    {
        moneyText.text = "$" + amount.ToString("#.##");
    }
}
