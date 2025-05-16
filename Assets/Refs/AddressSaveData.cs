using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class AddressSaveData
{
    public string p_n;
    public Color e_c = Color.cyan;
    public List<AddressLayoutVariation> vs = new List<AddressLayoutVariation>();
}