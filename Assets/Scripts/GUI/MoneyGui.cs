using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyGui : MonoBehaviour
{
    [SerializeField] Text moneyText;

    private GameController gameController;
    private void Awake()
    {
        gameController = GameController.Instance;
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
