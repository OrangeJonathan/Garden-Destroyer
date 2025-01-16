using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Colour", menuName = "ScriptableObjects/Colour Swatch Object")]
public class ColourSwatch : ScriptableObject
{
    [Header("Colour")]
    [SerializeField]
    private Color swatchColour;

    public Color SwatchColour => swatchColour;
}
