using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


public class CharacterManager : MonoBehaviour
{
    #region variables

    public string characterName;

    // Placement of the new fbx mesh
    public Vector3 SpawnHere;

    //Fbx of the character 
    [Space(10)]
    [Header("Character Manager")]
    public GameObject FBX;

    //Material List
    [Space(10)]
    [Header("Shared Material list")]

    public Material bodySharedMaterial;
    public Material pantsSharedMaterial;
    public Material shirtSharedMaterial;
    public Material rightEyeSharedMaterial;
    public Material leftEyeSharedMaterial;
    public Material TransEyesSharedMaterial;


    public int numberOfSlotMaterials;

    // Maps
    [Space(10)]
    [Header("Maps")]
    [Tooltip("Drag the texture relating to the color map and drag them here ")]
    public Texture _colorMap;
    public Texture _normalMap;

    // Pants and Shirt color
    [Space(10)]
    [Header("Pants and Shirt Colors")]
    public Color pantsColor;
    public Color shirtColor;

    // Eye colors
    [Space(10)]
    [Header("EyesColor")]
    public bool brownEyes;
    public bool blueEyes;
    public bool greenEyes;
    public bool grayEyes;
    public bool hazelEyes;

    // Gender type
    [Space(10)]
    [Header("Gender")]
    public bool Female;
    public bool Male;
    public bool mediumResolution;

    // Eyes Textures 
    private Texture brownEyesTexture;
    private Texture blueEyesTexture;
    private Texture grayEyesTexture;
    private Texture greenEyesTexture;
    private Texture hazelEyesTexture;

    // list Material for the body 
    private Material[] bodyMaterials;

    // mesh children objects 
    private GameObject leftEye;
    private GameObject rightEye;
    private GameObject transLeftEye;
    private GameObject transRightEye;
    private GameObject leftGland;
    private GameObject rightGland;
    private GameObject teethDown;
    private GameObject teethUp;
    private GameObject body;

    // bones 
    private GameObject rightHandThumb1;
    private GameObject rightHandThumb2;
    private GameObject rightHandThumb3;
    private GameObject leftHand;
    private GameObject rightHand;
    private GameObject leftArm;
    private GameObject rightArm;

    // rotation vectors
    private Vector3 rotationThumb1 = new Vector3(9.963f, -53.728f, 31f);
    private Vector3 rotationThumb2 = new Vector3(9.376f, 22.026f, -4.172f);
    private Vector3 rotationThumb3 = new Vector3(-3.239f, -10.175f, 7.636f);

    // scale vectors 
    private Vector3 ArmScale = new Vector3(0.85f, 0.85f, 0.85f);
    private Vector3 HandScale = new Vector3(0.85f, 0.85f, 0.85f);
    private Vector3 originalScale = new Vector3(1.0f, 1.0f, 1.0f);


    public Shader skinShader;
    public Shader hdrpLit;
    public Shader clothesShader;



    #endregion

    #region functions

    public void DebugEssai()
    {
        Debug.Log("Essai ");
    }

    public void InstantiateNewCharacter()
    {
        // Assign the new materials to the fbx 
        CreateEyesMaterials();
        setBodyMaterial();

        // Clone the fbx and place it
        Instantiate(FBX, SpawnHere, Quaternion.identity);
    }

    public void CreateEyesMaterials()
    {

        Material leftEyeMat = new Material(hdrpLit);
        Material rightEyeMat = new Material(hdrpLit);

        leftEyeMat.CopyPropertiesFromMaterial(leftEyeSharedMaterial);
        rightEyeMat.CopyPropertiesFromMaterial(rightEyeSharedMaterial);

        //  find children of the gameobject 
        leftEye = FBX.transform.Find("h_L_eye").gameObject;
        rightEye = FBX.transform.Find("h_R_eye").gameObject;
        transLeftEye = FBX.transform.Find("h_L_trans").gameObject;
        transRightEye = FBX.transform.Find("h_R_trans").gameObject;


        // Change texture for the eyecolor according to the bool choice 
        if (brownEyes == true)
        {
            blueEyes = false; greenEyes = false; grayEyes = false; hazelEyes = false;

            // Load the texture and assign it to the material  
            brownEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_BrownEyes");
            leftEyeMat.SetTexture("_BaseColorMap", brownEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", brownEyesTexture);
        }

        else if (blueEyes == true)
        {
            brownEyes = false; greenEyes = false; grayEyes = false; hazelEyes = false;

            // Load the texture and assign it to the material  
            blueEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_BlueEyes");
            leftEyeMat.SetTexture("_BaseColorMap", blueEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", blueEyesTexture);
        }

        else if (greenEyes == true)
        {
            brownEyes = false; blueEyes = false; grayEyes = false; hazelEyes = false;

            // Load the texture and assign it to the material  
            greenEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_GreenEyes");
            leftEyeMat.SetTexture("_BaseColorMap", greenEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", greenEyesTexture);
        }



        else if (grayEyes == true)
        {
            brownEyes = false; blueEyes = false; greenEyes = false; hazelEyes = false;

            // Load the texture and assign it to the material  
            grayEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_GrayEyes");
            leftEyeMat.SetTexture("_BaseColorMap", grayEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", grayEyesTexture);
        }

        else if (hazelEyes == true)
        {
            brownEyes = false; blueEyes = false; greenEyes = false; grayEyes = false;

            // Load the texture and assign it to the material  
            hazelEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_HazelEyes");
            leftEyeMat.SetTexture("_BaseColorMap", hazelEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", hazelEyesTexture);
        }


        // add the material to the folder assets 
        AssetDatabase.CreateAsset(leftEyeMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_leftEye.mat");
        AssetDatabase.CreateAsset(rightEyeMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_rightEye.mat");


        // assign new eyes material to the new character 
        if (leftEye != null && rightEye != null && transLeftEye !=null && transRightEye !=null)
        {
            leftEye.GetComponent<Renderer>().material = leftEyeMat;
            rightEye.GetComponent<Renderer>().material = rightEyeMat;
            transLeftEye.GetComponent<Renderer>().material = TransEyesSharedMaterial;
            transRightEye.GetComponent<Renderer>().material = TransEyesSharedMaterial;
        }
        else Debug.Log("No child with name h_L_eye ");




    }
 
    public void setBodyMaterial()
    {

        Material bodyMat = new Material(skinShader);
        Material pantsMat = new Material(clothesShader);
        Material shirtMat = new Material(clothesShader);

        bodyMat.CopyPropertiesFromMaterial(bodySharedMaterial);
        pantsMat.CopyPropertiesFromMaterial(pantsSharedMaterial);
        shirtMat.CopyPropertiesFromMaterial(shirtSharedMaterial);


        // find the children in the gameobject 
        leftGland = FBX.transform.Find("h_L_gland").gameObject;
        rightGland = FBX.transform.Find("h_R_gland").gameObject;
        teethDown = FBX.transform.Find("h_TeethDown").gameObject;
        teethUp = FBX.transform.Find("h_TeethUp").gameObject;
        body = FBX.transform.Find("H_DDS_HighRes").gameObject;

        // assign each material slot 
        bodyMaterials = body.GetComponent<Renderer>().materials;
        bodyMaterials[0] = bodyMat;
        bodyMaterials[1] = pantsMat;
        bodyMaterials[2] = shirtMat;

        // change body textures 
        bodyMat.SetTexture("Texture2D_36645CC3", _colorMap);
        bodyMat.SetTexture("Texture2D_311772cd227f4f18a9b82a6f15179cc8", _normalMap);

        // change pants and shirt colors
        pantsSharedMaterial.SetColor("Color_9f3dd916891141c1945eade78a3c804e", pantsColor);
        shirtSharedMaterial.SetColor("Color_9f3dd916891141c1945eade78a3c804e", shirtColor);


        // Assign the new material to the new character
        if (leftGland != null && rightGland != null && teethDown != null && teethUp != null)
        {
            leftGland.GetComponent<Renderer>().material = bodyMat;
            rightGland.GetComponent<Renderer>().material = bodyMat;
            teethDown.GetComponent<Renderer>().material = bodyMat;
            teethUp.GetComponent<Renderer>().material = bodyMat;
            body.GetComponent<Renderer>().materials = bodyMaterials;

        }
        else Debug.Log("No gland or teeth children ");


        // add the material to the folder assets 
        AssetDatabase.CreateAsset(bodyMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_body.mat");
        AssetDatabase.CreateAsset(pantsMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_pants.mat");
        AssetDatabase.CreateAsset(shirtMat, "Assets/Models/Characters/" + characterName + "/Materials/M_"+ characterName + "_shirt.mat");

    }


    public void rescaleHands()
    {
        // find hands and arms children objects of the fbx
        leftHand = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/LeftShoulder/LeftArm/LeftArmRoll/LeftForeArm/LeftForeArmRoll/LeftHand").gameObject;
        rightHand = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand").gameObject;
        leftArm = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/LeftShoulder/LeftArm").gameObject;
        rightArm = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm").gameObject;

        // scale the hands and arms if the character is female 
        if ( Female == true)
        {
            leftHand.transform.localScale = HandScale;
            rightHand.transform.localScale = HandScale;
            leftArm.transform.localScale = ArmScale;
            rightArm.transform.localScale = ArmScale;

            Male = false;
        }
        // scale the hands and arms if the character is male
        if ( Male == true )
        {

            leftHand.transform.localScale = originalScale;
            rightHand.transform.localScale = originalScale;
            leftArm.transform.localScale = originalScale;
            rightArm.transform.localScale = originalScale;
            Female = false;
        }
    }

    public void fixRighHandsBones()
    {
        // find bones 
        rightHandThumb1 = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1").gameObject;
        rightHandThumb2 = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1/RightHandThumb2").gameObject;
        rightHandThumb3 = FBX.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1/RightHandThumb2/RightHandThumb3").gameObject;

        // rotate 
        rightHandThumb1.transform.localRotation = Quaternion.Euler(rotationThumb1);
        rightHandThumb2.transform.localRotation = Quaternion.Euler(rotationThumb2);
        rightHandThumb3.transform.localRotation = Quaternion.Euler(rotationThumb3);
    }


    public void deleteOldMaterials()
    {
    }

    public void createNewPrefab()
    {

        PrefabUtility.CreatePrefab("Assets/Prefabs/Characters/" + characterName + ".prefab", FBX);

    }



    #endregion

    #region CustomEditor

    [CustomEditor(typeof(CharacterManager))]
    public class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            CharacterManager characterGenerator = (CharacterManager)target;

            // trial 
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Trial");
            if (GUILayout.Button("Essai debug"))
            {
                characterGenerator.DebugEssai();
            }

           // Generate new character 
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Generate a new character with the new materials");
            if (GUILayout.Button("Generate New Character"))
            {
                characterGenerator.InstantiateNewCharacter();
            }

            // Rescale Hands
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rescale hands and arms : check the appropriate gender ");
            if (GUILayout.Button("Rescale hands"))
            {
                characterGenerator.rescaleHands();
            }

            // Fix bones of the hand 
            EditorGUILayout.LabelField("Click if only the bones are rotated visually");
            if (GUILayout.Button("Fix the right hand's bones"))
            {
                characterGenerator.fixRighHandsBones();
            }

            // Add wig if it's a female 
            EditorGUILayout.LabelField("Choose the color of the wig ");
            if (GUILayout.Button("Add Random wig"))
            {
               
            }

            // Add wig if it's a female 
            EditorGUILayout.LabelField("delete old materials ");
            if (GUILayout.Button("Delete old materials "))
            {
                characterGenerator.deleteOldMaterials();
            }

            // Add wig if it's a female 
            EditorGUILayout.LabelField("Create a prefab for the new model ");
            if (GUILayout.Button("Create Prefab"))
            {
                characterGenerator.createNewPrefab();
            }


        }
    }

    #endregion
}
