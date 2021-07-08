using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]

public class DimensionFix : MonoBehaviour
{
    // Gameobject arm
    public string boneArm = "Arm";
    
    public GameObject _leftArm;
    public GameObject _rightArm;

    public Vector3 scaleArm = new Vector3(1.0f, 1.0f, 1.0f);

    public bool ScaleArm = false;
    
    public bool automaticScale = false;

    // GameObject hand
    public string BoneHand = "Hand";

    public GameObject _leftHand;
    public GameObject _rightHand;

    public Vector3 scaleHand = new Vector3(1.0f, 1.0f, 1.0f);
    public bool automaticScaleH = false;

    public bool ResetToOriginal = false;

    //Vectors to Scale

    private Vector3 automaticArmScale = new Vector3(0.85f, 0.85f, 0.85f);
    private Vector3 originalScale = new Vector3(1.0f, 1.0f, 1.0f);
    private Vector3 automaticHandScale = new Vector3(0.85f, 0.85f, 0.85f);



    void Awake()
    {
       
    }
    

    void Start()
    {
      
    }
    

    void Update()

    {
        //automatic scale for arms
        if ( automaticScale == true)
        {
            _leftArm.transform.localScale = automaticArmScale;
            _rightArm.transform.localScale = automaticArmScale;
            
            scaleArm = automaticArmScale;

            ScaleArm = false;
            ResetToOriginal = false;
        }

        //manual scale for arms
        else if (ScaleArm == true)
        {
            _leftArm.transform.localScale = scaleArm;

            ResetToOriginal = false;
            automaticScale = false;
        }

        //reset the scale  
        else if ( ResetToOriginal == true)
        {

            _leftArm.transform.localScale = originalScale;
            _rightArm.transform.localScale = originalScale;

            scaleArm = originalScale;
            scaleHand = originalScale;

            ScaleArm = false;
            automaticScale = false;
        }


        //automatic scale for hands
        if (automaticScaleH == true)
        {
            _leftHand.transform.localScale = automaticHandScale;
            _rightHand.transform.localScale = automaticHandScale;

            scaleHand = automaticHandScale;
        }


        _leftHand.transform.localScale = scaleHand;
        _rightHand.transform.localScale = scaleHand;

        // Debug.Log("Running in edit mode");
    }
}
