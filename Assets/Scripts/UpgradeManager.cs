using UnityEngine;

namespace PropertyTycoon
{
    /*
    public class UpgradeManager : MonoBehaviour
    {
        public void TryAddHouse(Property property, Player player)
        {
            if (property.CanAddHouse(player) && player.CanAddHotelToSet(property))
            {
                if (player.Balance >= property.houseCost)
                {
                    player.Debit(property.houseCost);
                    property.AddHouse();
                    Debug.Log($"House added to {property.name}. Total houses: {property.houses}");
                }
                else
                {
                    Debug.Log("Not enough funds to add a house.");
                }
            }
            else
            {
                Debug.Log("Cannot add house. Check ownership or property status.");
            }
        }
        public void TryAddHotel(Property property, Player player)
        {
            if (property.CanAddHotel(player) && player.CanAddHotelToSet(property))
            {
                if (player.Balance >= property.houseCost * 5) // Hotel = 5 house costs
                {
                    player.Debit(property.houseCost * 5);
                    property.AddHotel();
                    Debug.Log($"Added a hotel to {property.name}");
                }
                else
                {
                    Debug.Log("Not enough money to buy a hotel.");
                }
            }
            else
            {
                Debug.Log("Cannot add a hotel to this property.");
            }
        }
    }
    */
}
