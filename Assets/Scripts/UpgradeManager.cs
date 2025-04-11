using UnityEngine;

namespace PropertyTycoon
{
    public class UpgradeManager : MonoBehaviour
    {
        public bool TryAddHouse(Property property, Player player)
        {
            if (property.CanAddHouse(player) && player.CanAddHotelToSet(property))
            {
                if (player.Balance >= property.houseCost)
                {
                    player.Debit(property.houseCost);
                    Turn_Script.Instance.CheckBankruptcy(player);
                    property.addHouse();
                    Debug.Log($"House added to {property.name}. Total houses: {property.houses}");
                    return true;
                }
                else
                {
                    Debug.Log("Not enough funds to add a house.");
                    return false;
                }
            }
            else
            {
                Debug.Log("Cannot add house. Must own all properties in the colour to add.");
                return false;
            }
        }
        public bool TryAddHotel(Property property, Player player)
        {
            if (property.CanAddHotel(player) && player.CanAddHotelToSet(property))
            {
                if (player.Balance >= property.houseCost * 5) // Hotel = 5 house costs
                {
                    player.Debit(property.houseCost * 5);
                    Turn_Script.Instance.CheckBankruptcy(player);
                    property.addHotel();
                    Debug.Log($"Added a hotel to {property.name}");
                    return true;
                }
                else
                {
                    Debug.Log("Not enough money to buy a hotel.");
                    return false;
                }
            }
            else
            {
                Debug.Log("Cannot add a hotel to this property. Must have 4 houses");
                return false;
            }
        }
    }
}
