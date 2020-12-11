using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Equation : MonoBehaviour
{
    private static System.Random rand;
    public GameObject EquationContainer;
    public GameObject EquationBlockPrefab;
    public GameObject SolveForBoard;
    public GameObject RightHandSphere;
    public GameObject AntiText;
    public GameObject AntiBlockPrefab;
    public GameObject AntiBlockSpawner;
    public static GameObject TimeValue;
    public static GameObject BlocksMadeValue;
    public static GameObject MistakesValue;
    public static GameObject CorrectValue;
    private static TextMeshProUGUI AntiBlockTextValue;
    float touchDistance;
    Vector3 SphereForward;
    public static List<GameObject> EquationBlocks;
    public DificultyEnum DifficultySetting;
    public static float XAxisValue;
    private bool IsQuestionEmpty;
    public static bool IsQuestionFinished;
    private bool IsAntiBlockWait;
    private bool CanBlockBeCreated;
    private bool IsDone;
    private float StartTime;
    private TextMeshProUGUI TimeValuePro;
    private TextMeshProUGUI BlocksMadeValuePro;
    private TextMeshProUGUI MistakesValuePro;
    private TextMeshProUGUI CorrectValuePro;


    // Start is called before the first frame update
    void Start()
    {
        rand = new System.Random();
        DifficultySetting = DificultyEnum.Low;
        XAxisValue = -0.41f;
        EquationBlocks = new List<GameObject>();
        SphereForward = RightHandSphere.transform.TransformDirection(Vector3.forward);
        touchDistance = 0.005f;
        IsQuestionEmpty = true;
        AntiBlockTextValue = AntiText.GetComponent<TextMeshProUGUI>();
        TimeValue = GameObject.Find("TimeValue");
        TimeValuePro = TimeValue.GetComponent<TextMeshProUGUI>();
        BlocksMadeValue = GameObject.Find("AntValue");
        BlocksMadeValuePro = BlocksMadeValue.GetComponent<TextMeshProUGUI>();
        MistakesValue = GameObject.Find("MistakesValue");
        MistakesValuePro = MistakesValue.GetComponent<TextMeshProUGUI>();
        CorrectValue = GameObject.Find("CorrectValue");
        CorrectValuePro = CorrectValue.GetComponent<TextMeshProUGUI>();

        //RandomProblem(DifficultySetting);
    }
    void Update()
    {
        SphereForward = RightHandSphere.transform.TransformDirection(Vector3.forward);//RightHandSphere.transform.TransformDirection(RightHandSphere.transform.localPosition.);
        Debug.DrawRay(RightHandSphere.transform.position, SphereForward, Color.green);
        if(Physics.Raycast(RightHandSphere.transform.position,SphereForward,out RaycastHit ray,touchDistance))
        {
            Collider rayCollider = ray.collider;
            if(rayCollider.gameObject.name.Equals("EasyButton") && IsQuestionEmpty)
            {
                RandomProblem(DificultyEnum.Low);
                StartTime = Time.time;
                IsQuestionEmpty = false;
                
            }
            if (rayCollider.gameObject.name.Equals("ClearButton") && !IsQuestionEmpty)
            {
                foreach(GameObject equationcube in EquationBlocks)
                {
                    Destroy(equationcube);
                    
                }
                XAxisValue = -0.41f;
                EquationBlocks.Clear();
                IsQuestionEmpty = true;
            }
            #region Anti Block Events
            if (rayCollider.gameObject.name.Contains("AntiBlock") && !IsQuestionEmpty && !IsAntiBlockWait)
            {
                StartCoroutine(AntiBlockDelay());
                var textobject = rayCollider.gameObject.transform.Find(@"Text (TMP)");
                var textish = textobject.GetComponent<TextMeshProUGUI>();
                string textstring = textish.text;
                AntiBlockTextValue.text = AntiBlockTextValue.text + textstring;
                CanBlockBeCreated = true;
            }
            if (rayCollider.gameObject.name.Equals("ABlockGenerate") && !IsQuestionEmpty && !IsAntiBlockWait && CanBlockBeCreated)
            {
                StartCoroutine(AntiBlockDelay());
                GenerateAntiBlock(AntiBlockPrefab, AntiBlockSpawner);
                CanBlockBeCreated = false;
                BlocksMadeValuePro.text = (Int32.Parse(BlocksMadeValuePro.text) + 1).ToString();
            }
        
            #endregion
        }
        if (!IsQuestionFinished && !IsQuestionEmpty)
        {
            float t = Time.time - StartTime;
            string minutes = ((int)t / 60).ToString();
            string seconds = (t % 60).ToString("f1");
            TimeValuePro.text = minutes + ":" + seconds;
        }
       if(IsQuestionFinished && !IsQuestionEmpty && !IsDone)
        {
            IsDone = true;
            float NewXFirstBlock = EquationBlocks[2].transform.position.x;
            Vector3 newvec = new Vector3(0.0f  , 0.0f, (NewXFirstBlock * -1) -0.1f);
            Destroy(EquationBlocks[1]);
            Destroy(EquationBlocks[2]);
            EquationBlocks[0].transform.Translate(newvec);
        }

    }
    private void RandomProblem(DificultyEnum dificulty)
    {

        float xaxit = 0;
        int mathsymbolcap = 0;
        int maxcoefficientcap = 0;
        int maxsolveforvariablecap = 0;

        if (dificulty == DificultyEnum.Low)
        {
            mathsymbolcap = 2;
            maxcoefficientcap = 9;
            maxsolveforvariablecap = 3;
            // Create the first block which will be just a variable x,y,z           
            
            //Set the first block as a symbol according to enum
            SolveForVariables sfv = new SolveForVariables();
            MathSymbolVarEnum sym = new MathSymbolVarEnum();
            EndOfEquation end = new EndOfEquation();
            GenerateBlock(sfv, maxsolveforvariablecap, 0,0.0f,EquationBlockPrefab,EquationContainer);
            //Create a second block and have a math symbol like + or -
            GenerateBlock(sym, mathsymbolcap, 0, 0.11f, EquationBlockPrefab, EquationContainer);
            //Create constant on its own
            GenerateBlock(null, mathsymbolcap, 0, 0.11f, EquationBlockPrefab, EquationContainer);
            //add equal sign for end of equation
            GenerateBlock(end,1, 0, 0.18f, EquationBlockPrefab, EquationContainer);
            GenerateBlock(null, mathsymbolcap, 0, 0.18f, EquationBlockPrefab, EquationContainer);
            UpdateGoalText(SolveForBoard);
            EquationBlocks[EquationBlocks.Count - 1].GetComponent<EQCscript>().isSolutionBlock = true; 
        }
        else if (dificulty == DificultyEnum.Medium)
        {

        }
        else if (dificulty == DificultyEnum.Hard)
        {

        }
        else
        {
            Debug.Log("Difficulty not set");
        }
    }

    private void UpdateGoalText(GameObject solveForBoard)
    {
        string solvethis = $"Goal: Isolate ";

        solveForBoard.transform.Find("Title").GetComponent<Text>().text = solvethis + EquationBlocks[0].transform.Find("Canvas").Find("Title").GetComponent<Text>().text;
    }
    private static void GenerateAntiBlock(GameObject antiBlockPrefab, GameObject spawnPoint)
    {
        GameObject antiblock = Instantiate(antiBlockPrefab, spawnPoint.transform.position, Quaternion.identity);
        var blocktextobj = antiblock.transform.Find("Plane").Find("Canvas").Find("Text (TMP)").gameObject.GetComponent<TextMeshProUGUI>();
        string texts = AntiBlockTextValue.text;
        blocktextobj.text = AntiBlockTextValue.text;
        AntiBlockTextValue.text = "";
    }
    private static void GenerateBlock(Enum enumer,int maxRange, int minRange,float xoffset,GameObject EquationBlockPrefab,GameObject EquationContainer)
    {
        
        Vector3 vpos = new Vector3(-0.41f, 0.0f, 0.0f);
       
        //GameObject block = Instantiate(EquationBlockPrefab, vpos, rot, EquationContainer.transform);
        GameObject block = Instantiate(EquationBlockPrefab, EquationContainer.transform, false);
        EquationBlocks.Add(block);
        block.transform.localPosition = new Vector3(XAxisValue + xoffset, 0.0f, 0.0f);
        XAxisValue += xoffset;
        if (enumer != null)
        {
            var enumValues = Enum.GetValues(enumer.GetType());
            if (enumValues.GetValue(0).ToString().Length > 2)
            {
                enumValues = SwapValues(enumValues);
            }
            
            var textBox = block.transform.Find("Canvas").Find("Title");
            string solvefor = enumValues.GetValue(rand.Next(minRange, maxRange)).ToString();

            textBox.GetComponent<Text>().text = solvefor;
        }
        else
        {
            //var rand = new System.Random();
            string solvefor = rand.Next(0, 10).ToString();
            var textBox = block.transform.Find("Canvas").Find("Title");
            textBox.GetComponent<Text>().text = solvefor;
        }
    }

    private static Array SwapValues(Array enumValues)
    {
        int index = 0;
        System.Object[] temp = new System.Object[enumValues.Length];
        foreach(var strin in enumValues)
        {
            switch(strin.ToString())
            {
                case "Plus":
                    temp[index] = "+";
                    index++;
                    break;
                case "Minus":
                    temp[index] = "-";
                    index++;
                    break;
                case "Times":
                    temp[index] = "*";
                    index++;
                    break;
                case "Divide":
                    temp[index] = "/";
                    index++;
                    break;
                case "Equals":
                    temp[index] = "=";
                    index++;
                    break;
                default:
                    temp[index] = "Error";
                    break;
            }
        }
        return temp;
    }

    public void GenerateEquation()
    {
        double xaxis = -0.31;


    }

    public enum DificultyEnum
    {
        Low,
        Medium,
        Hard
    }
    public enum MathSymbolVarEnum
    {
        Plus,
        Minus,
        Times,
        Divide
    }
    public enum SolveForVariables
    {
        X,
        Y,
        Z
    }
    public enum EndOfEquation
    {
        Equals
    }

    IEnumerator AntiBlockDelay()
    {
        IsAntiBlockWait = true;
        yield return new WaitForSeconds(1);
        IsAntiBlockWait = false;
    }
    // Update is called once per frame

}
