using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AntiBlocker : MonoBehaviour
{
    public Material OriginalMaterial;
    public Material ChangeToMaterial;
    // Start is called before the first frame update
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "EqBlock")
        {
            CompareTextInBlocks(this.gameObject, collision.gameObject);
        }
    }

    private void CompareTextInBlocks(GameObject antiBlock, GameObject EqBlock)
    {
        string antiblocktext = antiBlock.transform.Find("Plane").Find("Canvas").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
        string eqblocktext = EqBlock.transform.Find("Canvas").Find("Title").GetComponent<Text>().text;
        foreach(char symbol in eqblocktext)
        {
            if (antiblocktext.Contains(CompareInverse(symbol)))
            {
                EQCscript.NumberOfBlocksEliminated += 1;

                // I have to change the material on the equation block
                EqBlock.GetComponent<MeshRenderer>().material = ChangeToMaterial;
                //Then I have to enable the grabable script... ez pz
                EqBlock.GetComponent<OVRGrabbable>().enabled = true;
                //
                string text = (Int32.Parse(Equation.CorrectValue.GetComponent<TextMeshProUGUI>().text) + 1).ToString();
                Equation.CorrectValue.GetComponent<TextMeshProUGUI>().text = (Int32.Parse(Equation.CorrectValue.GetComponent<TextMeshProUGUI>().text) + 1).ToString();
            }
            else
            {
                Equation.MistakesValue.GetComponent<TextMeshProUGUI>().text = (Int32.Parse(Equation.MistakesValue.GetComponent<TextMeshProUGUI>().text) + 1).ToString();
            }
        }

    }

    public string CompareInverse(char symbol)
    {
        string ret;
        switch (symbol.ToString())
        {
            case "+":
                ret = "-";
                break;
            case "-":
                ret = "+";
                break;
            case "*":
                ret = "/";
                break;
            case "/":
                ret = "*";
                break;
            default:
                ret = symbol.ToString();
                break;
        }
        return ret;
    }
}
