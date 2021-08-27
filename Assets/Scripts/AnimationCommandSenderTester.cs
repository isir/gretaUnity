using UnityEngine;

[RequireComponent(typeof(GretaCharacterAnimator))]
public class AnimationCommandSenderTester : MonoBehaviour {

    public string animationID;
    public KeyCode keyboardTrigger;

    private GretaCharacterAnimator _charAnimScript;

    void Start()
    {
        _charAnimScript = GetComponent<GretaCharacterAnimator>();
    }

    // Update is called once per frame
    void Update ()
    {
        
        if (Input.GetKeyUp(keyboardTrigger) && animationID != null && animationID.Trim().Length > 0)
        {
            _charAnimScript.PlayAgentAnimation(animationID);
        }
    }
}