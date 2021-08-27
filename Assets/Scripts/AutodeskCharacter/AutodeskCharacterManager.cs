using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEditor;


public class AutodeskCharacterManager : MonoBehaviour
{
    #region variables

    public string characterName;

    //Fbx of the character 
    [Space(10)]
    [Header("Character Model")]
    public GameObject FBX;

    //Material List
    //[Space(10)]
    //[Header("Shared Material list")]


    private Material bodySharedMaterial;
    private Material rightEyeSharedMaterial;
    private Material leftEyeSharedMaterial;

    private Material bodyMatOriginal;
    private Material pantsMatOriginal;
    private Material shirtMatOriginal;
    private Material rightEyeMatOriginal;
    private Material leftEyeMatOriginal;
    private Material TransEyesSharedMaterial;


    // Maps
    [Space(10)]
    [Header("Maps")]
    [Tooltip("Drag the texture relating to the color map and drag them here ")]
    public Texture _colorMap;
    public Texture _normalMap;
    private Texture _colorBlendMale;
    private Texture _colorBlendFemale;

    // Change 
    [Space(10)]
    [Header("Model Properties")]
    public Color pantsColor;
    public Color shirtColor;


    public enum hair { showExistingWig, hideExistingWig }
    public hair modelHair;
   


    public enum eyesColor { Brown, Blue, Gray, Green, Hazel };
    public eyesColor EyesColor;
    public enum gender { Female, Male };
    public gender modelGender;
    public enum resolution { High, Medium };
    public resolution modelPolygonResolution;



    // Eyes Textures 
    private Texture brownEyesTexture;
    private Texture blueEyesTexture;
    private Texture grayEyesTexture;
    private Texture greenEyesTexture;
    private Texture hazelEyesTexture;

    // list Material for the body 
    private Material[] bodyMaterials;
    private Material[] cloneFemaleHairMaterials;

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
    private GameObject headBone;

    // rotation vectors
    private Vector3 rotationThumb1 = new Vector3(9.963f, -53.728f, 31f);
    private Vector3 rotationThumb2 = new Vector3(9.376f, 22.026f, -4.172f);
    private Vector3 rotationThumb3 = new Vector3(-3.239f, -10.175f, 7.636f);

    // scale vectors 
    private Vector3 ArmScale = new Vector3(0.9f, 0.9f, 0.9f);
    private Vector3 HandScale = new Vector3(0.85f, 0.85f, 0.85f);
    private Vector3 originalScale = new Vector3(1.0f, 1.0f, 1.0f);

    [Space(10)]
    [Header("Shaders")]
    public Shader skiNShader;
    private Shader skinShader;
    public Shader clothesShader;
    public Shader hdrpLit;

    [Space(10)]
    public GameObject generatedCharacter;
    public GameObject hairPrefab;
    public enum hairColor { lightRed, dark, blonde, brown }
    public hairColor hairColorTone;

    // hair model 

    private GameObject originalFemaleHairObj;
    private GameObject maleHairModel;

    private Material femaleHairScalpMat;
    private Material originalFemaleHairMat;
    private Material femaleHair_dark;
    private Material femaleHair_blond;
    private Material femaleHair_brown;
    private Material femaleHair_red;

    private GameObject existingHair;
    private GameObject cloneFemaleHairObj;

    private Vector3 hairScale = new Vector3(1f, 1f, 1f);
    private Vector3 hairOriginalPosition = new Vector3(1f, 1f, 1f);
    private Vector3 hairUpdatedPosition;



    // folder

    private string materialsFolderPath;

    #endregion

    #region functions
    
    public GameObject InstantiateNewCharacter()
    {
        // Assign the new materials to the fbx 
        createMaterialsFolder();
        CreateEyesMaterials();
        SetBodyMaterial();
        DisableEmbedded<Camera>();
        DisableEmbedded<Light>();
        

        // Clone the fbx and place it
        generatedCharacter = Instantiate(FBX, transform.position, Quaternion.identity);
        generatedCharacter.name = characterName;
        UpdateHair(generatedCharacter);


        return generatedCharacter;
    }

    protected void UpdateHair(GameObject go)
    {
        // handle hairs
        if (go.transform.Find("h_wig"))
        {
            existingHair = go.transform.Find("h_wig").gameObject;
            existingHair.GetComponent<Renderer>().enabled = modelHair == hair.showExistingWig;
        }
    }

    protected void DisableEmbedded<T>() where T : Component
    {
        if (FBX != null)
        {
            T embeddedCam = null;
            do
            {
                embeddedCam = FBX.GetComponentInChildren<T>(false);
                if (embeddedCam != null)
                    embeddedCam.gameObject.SetActive(false);
                else
                    break;
            } while (true);
        }        
    }

    public void createMaterialsFolder()
    {
        materialsFolderPath = "Assets / Models / Characters / " + characterName + "Materials";

        if (!AssetDatabase.IsValidFolder(materialsFolderPath))
        {
            AssetDatabase.CreateFolder(("Assets / Models / Characters / " + characterName), "Materials");
        }
    }


    public void CreateEyesMaterials()
    {
        // using the transparent material coat as a shared material 

        TransEyesSharedMaterial = Resources.Load<Material>("Materials/M_TransEyes");

        // New copies of the eyes material 
        Material leftEyeMat = new Material(hdrpLit);
        Material rightEyeMat = new Material(hdrpLit);

        if (modelGender == gender.Female)
        {
            leftEyeMatOriginal = Resources.Load<Material>("Materials/M_LeftEye");
            rightEyeMatOriginal = Resources.Load<Material>("Materials/M_RightEye");
        }
        else
        {
            leftEyeMatOriginal = Resources.Load<Material>("Materials/M_Male_LeftEye");
            rightEyeMatOriginal = Resources.Load<Material>("Materials/M_Male_RightEye");
        }

        leftEyeMat.CopyPropertiesFromMaterial(leftEyeMatOriginal);
        rightEyeMat.CopyPropertiesFromMaterial(rightEyeMatOriginal);

        //  find children of the gameobject 
        leftEye = FBX.transform.Find("h_L_eye").gameObject;
        rightEye = FBX.transform.Find("h_R_eye").gameObject;
        transLeftEye = FBX.transform.Find("h_L_trans").gameObject;
        transRightEye = FBX.transform.Find("h_R_trans").gameObject;

        // Change texture for the eyecolor 
        if (EyesColor == eyesColor.Brown)
        {
            brownEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_BrownEyes");
            leftEyeMat.SetTexture("_BaseColorMap", brownEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", brownEyesTexture);
        }

        else if (EyesColor == eyesColor.Blue)
        {
            // Load the texture and assign it to the material  
            blueEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_BlueEyes");
            leftEyeMat.SetTexture("_BaseColorMap", blueEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", blueEyesTexture);
        }

        else if (EyesColor == eyesColor.Green)
        {
            // Load the texture and assign it to the material  
            greenEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_GreenEyes");
            leftEyeMat.SetTexture("_BaseColorMap", greenEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", greenEyesTexture);
        }



        else if (EyesColor == eyesColor.Gray)
        {

            // Load the texture and assign it to the material  
            grayEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_GrayEyes");
            leftEyeMat.SetTexture("_BaseColorMap", grayEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", grayEyesTexture);
        }

        else if (EyesColor == eyesColor.Hazel)
        {

            // Load the texture and assign it to the material  
            hazelEyesTexture = Resources.Load<Texture2D>("Textures/Eyes/T_AT_HazelEyes");
            leftEyeMat.SetTexture("_BaseColorMap", hazelEyesTexture);
            rightEyeMat.SetTexture("_BaseColorMap", hazelEyesTexture);
        }


        // add the material to the folder assets 
        if (!AssetDatabase.IsValidFolder("Assets/Models/Characters/" + characterName))
        {
            AssetDatabase.CreateFolder("Assets/Models/Characters", characterName);
            AssetDatabase.CreateFolder("Assets/Models/Characters/" + characterName, "Materials");
        }
        AssetDatabase.CreateAsset(leftEyeMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_leftEye.mat");
        AssetDatabase.CreateAsset(rightEyeMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_rightEye.mat");


        // assign new eyes material to the new character 
        if (leftEye != null && rightEye != null && transLeftEye != null && transRightEye != null)
        {
            leftEye.GetComponent<Renderer>().material = leftEyeMat;
            rightEye.GetComponent<Renderer>().material = rightEyeMat;
            transLeftEye.GetComponent<Renderer>().material = TransEyesSharedMaterial;
            transRightEye.GetComponent<Renderer>().material = TransEyesSharedMaterial;
        }
        else Debug.Log("No child with name h_L_eye ");
    }

    public void SetBodyMaterial()
    {
        Material bodyMat = new Material(skiNShader);
        Material pantsMat = new Material(clothesShader);
        Material shirtMat = new Material(clothesShader);

        bodyMatOriginal = Resources.Load<Material>("Materials/M_Body");
        pantsMatOriginal = Resources.Load<Material>("Materials/M_Pants");
        shirtMatOriginal = Resources.Load<Material>("Materials/M_Shirt");


        bodyMat.CopyPropertiesFromMaterial(bodyMatOriginal);
        pantsMat.CopyPropertiesFromMaterial(pantsMatOriginal);
        shirtMat.CopyPropertiesFromMaterial(shirtMatOriginal);


        // find the children in the gameobject 
        leftGland = FBX.transform.Find("h_L_gland").gameObject;
        rightGland = FBX.transform.Find("h_R_gland").gameObject;
        teethDown = FBX.transform.Find("h_TeethDown").gameObject;
        teethUp = FBX.transform.Find("h_TeethUp").gameObject;
        body = FBX.transform.Find("H_DDS_HighRes").gameObject;

        // assign each material slot 
        bodyMaterials = body.GetComponent<Renderer>().sharedMaterials;
        bodyMaterials[0] = bodyMat;
        bodyMaterials[1] = pantsMat;
        bodyMaterials[2] = shirtMat;

        // color Tones Blending 

        _colorBlendMale = Resources.Load<Texture2D>("Textures/BodySkin/T_ColorTones");
        _colorBlendFemale = Resources.Load<Texture2D>("Textures/BodySkin/T_ColorTones_F");


        bodyMat.SetTexture("Texture2D_15c73632403f4139bd1dcc997fa84fdb", _colorMap);
        bodyMat.SetTexture("Texture2D_9b4a0c0d495a4e92b2a4640d0016d0bc", _normalMap);


        if (modelGender == gender.Female)
        {
            bodyMat.SetTexture("Texture2D_4970df62983c47a4923af03f232ef946", _colorBlendFemale);
        }
        else if (modelGender == gender.Male)
        {
            bodyMat.SetTexture("Texture2D_4970df62983c47a4923af03f232ef946", _colorBlendMale);
        }


        // change pants and shirt colors
        pantsMat.SetColor("Color_9f3dd916891141c1945eade78a3c804e", pantsColor);
        shirtMat.SetColor("Color_9f3dd916891141c1945eade78a3c804e", shirtColor);


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
        AssetDatabase.CreateAsset(shirtMat, "Assets/Models/Characters/" + characterName + "/Materials/M_" + characterName + "_shirt.mat");

    }


    public void rescaleHands()
    {
        GameObject obj = FBX;
        if (generatedCharacter != null)
            obj = generatedCharacter;

        // find hands and arms children objects of the fbx
        leftHand = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/LeftShoulder/LeftArm/LeftArmRoll/LeftForeArm/LeftForeArmRoll/LeftHand").gameObject;
        rightHand = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand").gameObject;
        leftArm = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/LeftShoulder/LeftArm").gameObject;
        rightArm = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm").gameObject;

        // scale the hands and arms if the character is female 
        if (modelGender == gender.Female)
        {
            leftHand.transform.localScale = HandScale;
            rightHand.transform.localScale = HandScale;
            leftArm.transform.localScale = ArmScale;
            rightArm.transform.localScale = ArmScale;
        }
        // scale the hands and arms if the character is male
        if (modelGender == gender.Male)
        {
            leftHand.transform.localScale = originalScale;
            rightHand.transform.localScale = originalScale;
            leftArm.transform.localScale = originalScale;
            rightArm.transform.localScale = originalScale;
        }
    }

    public void fixRighHandsBones()
    {
        GameObject obj = FBX;
        if (generatedCharacter != null)
            obj = generatedCharacter;

        // find bones 
        rightHandThumb1 = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1").gameObject;
        rightHandThumb2 = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1/RightHandThumb2").gameObject;
        rightHandThumb3 = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/RightShoulder/RightArm/RightArmRoll/RightForeArm/RightForeArmRoll/RightHand/RightFingerBase/RightHandThumb1/RightHandThumb2/RightHandThumb3").gameObject;

        // rotate 
        rightHandThumb1.transform.localRotation = Quaternion.Euler(rotationThumb1);
        rightHandThumb2.transform.localRotation = Quaternion.Euler(rotationThumb2);
        rightHandThumb3.transform.localRotation = Quaternion.Euler(rotationThumb3);
    }


    public GameObject addWig()
    {
        GameObject obj = FBX;

        if (generatedCharacter != null)
            obj = generatedCharacter;
                    
        // load new hair with materials
        if (hairPrefab != null)
            originalFemaleHairObj = hairPrefab;
        else
            originalFemaleHairObj = Resources.Load<GameObject>("Models/Hair/Female_Hair");
        originalFemaleHairMat = Resources.Load<Material>("Materials/M_FemaleHair_Bun");

        // hair textures 
        femaleHair_blond = Resources.Load<Material>("Materials/M_FemaleHair_Bun_Blond");
        femaleHair_red = Resources.Load<Material>("Materials/M_FemaleHair_Bun_Red");
        femaleHair_dark = Resources.Load<Material>("Materials/M_FemaleHair_Bun_Dark");
        femaleHair_brown = Resources.Load<Material>("Materials/M_FemaleHair_Bun_Brown");

        cloneFemaleHairObj = Instantiate(originalFemaleHairObj, generatedCharacter.transform, true);

        Renderer renderer = cloneFemaleHairObj.GetComponent<Renderer>();
        if (renderer != null)
        {
            cloneFemaleHairMaterials = renderer.sharedMaterials;

            // hair color tone 
            int colorIndex = -1;
            if (cloneFemaleHairMaterials.Length >= 2)
                colorIndex = 1;
            if (colorIndex > -1)
            {
                if (hairColorTone == hairColor.lightRed) { cloneFemaleHairMaterials[colorIndex] = femaleHair_red; }
                else if (hairColorTone == hairColor.dark) { cloneFemaleHairMaterials[colorIndex] = femaleHair_dark; }
                else if (hairColorTone == hairColor.blonde) { cloneFemaleHairMaterials[colorIndex] = femaleHair_blond; }
                else if (hairColorTone == hairColor.brown) { cloneFemaleHairMaterials[colorIndex] = femaleHair_brown; }

                // assign material to the new cloned object 
                cloneFemaleHairObj.GetComponent<Renderer>().materials = cloneFemaleHairMaterials;

                //hair position and scale 

                cloneFemaleHairObj.transform.position = new Vector3(0.001f, -0.139f, 0.096f);
                cloneFemaleHairObj.transform.localScale = new Vector3(0.43f, 0.43f, 0.43f);
            }

        }
        //find bone head and make the hair child for the bone 
        headBone = obj.transform.Find("master/Reference/Hips/Spine/Spine1/Spine2/Spine3/Spine4/Neck/Neck1/Head").gameObject;
        cloneFemaleHairObj.transform.SetParent(headBone.transform);

        //Instantiate(FBX, SpawnHere, Quaternion.identity);


        //if (existingHair != null)
        //{
        //    Instantiate(CloneFemaleHairObj, hairOriginalPosition, Quaternion.identity);
        //}

        return cloneFemaleHairObj;
       
    }

    public void createNewPrefab()
    {
        if (generatedCharacter != null)
            PrefabUtility.SaveAsPrefabAssetAndConnect(generatedCharacter, "Assets/Prefabs/Characters/" + characterName + ".prefab", InteractionMode.UserAction);
        else
            Debug.LogError("Create charater first");
    }



    #endregion

    #region CustomEditor

    [CustomEditor(typeof(AutodeskCharacterManager))]
    public class CustomInspector : Editor
    {
        public override void OnInspectorGUI()
        {

            DrawDefaultInspector();

            AutodeskCharacterManager characterGenerator = (AutodeskCharacterManager)target;

            // Generate new character 
            EditorGUILayout.HelpBox("First generate a charecter, then you can apply modifications",MessageType.Info);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Generate a new character with the new materials");
            if (GUILayout.Button("Generate New Character"))
            {
                GameObject go = characterGenerator.InstantiateNewCharacter();               
                if (go != null)
                {
                    go.SetActive(true);
                    Selection.activeGameObject = go;
                }
            }

            // Rescale Hands
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Rescale hands and arms : check the appropriate gender ");
            if (GUILayout.Button("Rescale hands"))
            {
                characterGenerator.rescaleHands();
            }

            // Fix bones of the hand 
            EditorGUILayout.LabelField("If the right hand's bones are rotated incorrectly");
            if (GUILayout.Button("Fix the right hand's bones"))
            {
                characterGenerator.fixRighHandsBones();
            }

            // Add wig if it's a female 
            EditorGUILayout.LabelField("Add the selected hair prefab as a wig");
            if (GUILayout.Button("Add wig"))
            {
                GameObject go = characterGenerator.addWig();
                if(go != null)
                    Selection.activeGameObject = go;
            }

            // create the prefab  
            EditorGUILayout.LabelField("Create a prefab for the new model ");
            if (GUILayout.Button("Create Prefab"))
            {
                characterGenerator.createNewPrefab();
            }


        }
    }

    #endregion
}
