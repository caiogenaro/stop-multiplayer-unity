using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterCustomizer : MonoBehaviour
{
    [SerializeField] Color[] allColors;
    [SerializeField] Mesh[] allMaterials;

    public void SetColor(int colorIndex)
    {
        PlayerInfo.PI.SetColor(allColors[colorIndex]);
        Debug.Log("Trocou");
    }

    public void SetGender(int genderIndex)
    {
        PlayerInfo.PI.SetGender(allMaterials[genderIndex]);
    }
 
}
