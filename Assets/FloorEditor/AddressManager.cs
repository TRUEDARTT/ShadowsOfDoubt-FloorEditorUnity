using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class AddressManager : MonoBehaviour
{
    [Range(0, 0)]
    public int selectedAddressForPainting = 0;
    
    [SerializeReference]
    public List<Address> addresses = new List<Address>();

    [InitializeOnLoadMethod]
    private static void OnProjectReload()
    {
        var addressManager = GameObject.FindAnyObjectByType<AddressManager>();
        if (addressManager)
        {
            addressManager.selectedAddressForPainting = 0;
            addressManager.addresses.Clear();
        }
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
