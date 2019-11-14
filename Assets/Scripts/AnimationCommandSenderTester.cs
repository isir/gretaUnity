using UnityEngine;

public class AnimationCommandSenderTester : MonoBehaviour {

    public GameObject character;
    public string animationID;

    private GretaCharacterAnimator _charAnimScript;

    void Start()
    {
        _charAnimScript = character.GetComponent<GretaCharacterAnimator>();
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