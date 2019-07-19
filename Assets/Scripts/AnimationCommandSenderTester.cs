using UnityEngine;

public class AnimationCommandSenderTester : MonoBehaviour {

    public GameObject character;
    public string animationID;

    private CharacterAnimation_Single_Autodesk _charAnimScript;

    void Start()
    {
        _charAnimScript = character.GetComponent<CharacterAnimation_Single_Autodesk>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.T) && animationID != null && animationID.Trim().Length > 0)
        {
            _charAnimScript.PlayAgentAnimation(animationID);
        }
    }
}