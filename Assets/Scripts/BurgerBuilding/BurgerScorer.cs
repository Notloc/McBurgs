using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BurgerScoring;

public struct BurgerScore
{
    public float vegetarianScore;
    public float chessiness;
}

public class BurgerScorer : MonoBehaviour, IHaveTooltip
{
    [Header("Tooltip Data")]
    [SerializeField] Vector3 displayOffset;
    [SerializeField] TooltipData tooltipData;

    public Vector3 DisplayOffset { get { return displayOffset; } }
    public TooltipData TooltipData { get { return tooltipData; } }

    public BurgerScore Score { get { return burgerInfo.Score(); } }
    private BurgerInfo burgerInfo = new BurgerInfo();

    public void ScoreBurger(List<IBurgerComponent> burgerComponents)
    {
        burgerInfo = new BurgerInfo();
        var myFood = GetComponent<IFood>();

        burgerInfo.AddFood(myFood);
        foreach (var component in burgerComponents)
            burgerInfo.AddFood(component.gameObject.GetComponent<IFood>());
    }
    
    //public BurgerRating Rate(TastePreference preferences) { }
}

/// <summary>
/// Classes specific to scoring the attributes of a burger
/// </summary>
namespace BurgerScoring
{
    public class InvalidFood : IFood
    {
        public FoodType FoodType { get { return FoodType.Inedible; } }
        public GameObject gameObject { get; }
        public Transform transform { get; }
    }

    public class BurgerInfo
    {
        const float CHEESE_MULT = 2.5f;

        int meatCount = 0;
        int vegeCount = 0;
        int cheeseCount = 0;

        int inedibleCount = 0;

        public BurgerScore Score()
        {
            BurgerScore score = new BurgerScore();

            int total = meatCount + vegeCount + cheeseCount;
            int meatVegTotal = meatCount + vegeCount;

            score.vegetarianScore = (vegeCount / (float)meatVegTotal);
            score.chessiness = (cheeseCount / (float)total) * CHEESE_MULT;

            return score;
        }

        public void AddFood(IFood food)
        {
            if (food.IsNull())
                food = new InvalidFood();

            switch (food.FoodType)
            {
                case FoodType.Bread:
                    HandleBread(food);
                    break;
                case FoodType.Meat:
                    HandleMeat(food);
                    break;
                case FoodType.Vegetable:
                    HandleVegetable(food);
                    break;
                case FoodType.Cheese:
                    HandleCheese(food);
                    break;
                case FoodType.Sauce:
                    HandleSauce(food);
                    break;
                default:
                    HandleInedible(food);
                    break;
            }
        }

        private void HandleBread(IFood food)
        { }

        private void HandleMeat(IFood food)
        {
            meatCount++;
        }

        private void HandleVegetable(IFood food)
        {
            vegeCount++;
        }

        private void HandleCheese(IFood food)
        {
            cheeseCount++;
        }

        private void HandleSauce(IFood food)
        {

        }

        private void HandleInedible(IFood food)
        {
            inedibleCount++;
        }
    }
}