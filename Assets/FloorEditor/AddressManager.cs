using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

// TODO: Address [0] is always outside
// Address[1] is always lobby

public class AddressManager : MonoBehaviour
{
    [Range(0, 0)]
    public int selectedAddressForPainting = 0;
    
    [SerializeReference]
    public List<Address> addresses;
    
    public void Reset()
    {
        selectedAddressForPainting = 0;
        addresses.Clear();
        
        var defaultAddresses = new List<Address>()
        {
            new Address() { addressPreset = "Outside", color = new Color(1, 0, 0.41f) },
            new Address() { addressPreset = "Lobby", color = new Color(1, 0.66f, 0) },
        };
        
        foreach (var address in defaultAddresses)
            addresses.Add(address);
    }
}

[Serializable]
public class Address
{
    public Color color;
    public string addressPreset;
}

public static class AddressPresets
{
    public static readonly string[] AddressPreset = new[]
    {
        "AmericanDiner",
        "Apartment",
        "Atrium",
        "Ballroom",
        "Bar",
        "BasementLobby",
        "BlackmarketSyncClinic",
        "BlackmarketTrader",
        "BuildingBathrooms",
        "BuildingBathroomsAbandoned",
        "Chemist",
        "ChineseEatery",
        "CityHallLobby",
        "ControlRoom",
        "CorporateLobby",
        "EmptyDen",
        "EnforcerOffice",
        "FastFood",
        "FathomsLobby",
        "FathomsYard",
        "GamblingDen",
        "HardwareStore",
        "HighriseOffice",
        "HospitalWard",
        "HotelLobby",
        "HotelRoom",
        "HouseFrontage",
        "IndustrialLobby",
        "IndustrialOffice",
        "IndustrialPlant",
        "Laboratory",
        "Launderette",
        "LoanShark",
        "Lobby",
        "MediumOffice",
        "Park",
        "Path",
        "PawnShop",
        "PowerRoom",
        "Rooftop",
        "RooftopBar",
        "Slum",
        "Supermarket",
        "SyncClinic",
        "WeaponsDealer",
        "WorkplaceCanteen",
        "Yard"
    };
}
