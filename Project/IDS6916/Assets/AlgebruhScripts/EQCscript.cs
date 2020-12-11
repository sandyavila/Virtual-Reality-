using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EQCscript : MonoBehaviour
{
    public static int NumberOfBlocksEliminated;
    public bool isSolutionBlock;

    // Start is called before the first frame update
    void Start()
    {
        NumberOfBlocksEliminated = 0;
    }

    // Update is called once per frame
    private void OnCollisionEnter(Collision collision)
    {

        if(NumberOfBlocksEliminated >= 2 && isSolutionBlock)
        {
            int ret = 0;
            string eqcmathtext = this.gameObject.transform.Find("Canvas").Find("Title").GetComponent<Text>().text;
            string antimathtext = collision.transform.Find("Plane").Find("Canvas").Find("Text (TMP)").GetComponent<TextMeshProUGUI>().text;
            switch (antimathtext[0])
            {
                case '+':
                    ret = int.Parse(antimathtext[1].ToString()) + int.Parse(eqcmathtext);
                    break;
                case '-':
                    ret = int.Parse(eqcmathtext) - int.Parse(antimathtext[1].ToString());
                    break;
                case '*':
                    ret = int.Parse(eqcmathtext) * int.Parse(antimathtext[1].ToString());
                    break;
                case '/':
                    ret = int.Parse(eqcmathtext) / int.Parse(antimathtext[1].ToString());
                    break;
                default:
                    ret = 0;
                    break;
            }
            this.gameObject.transform.Find("Canvas").Find("Title").GetComponent<Text>().text = ret.ToString();
            isSolutionBlock = false;
            Equation.IsQuestionFinished = true;
        }
    }
}
