using UnityEngine;
using System.Collections.Generic;
using thriftImpl;

/// <summary>
/// Simple behaviour script to synchronize a character in GRETA.
/// </summary>
public class GretaCharacterSynchronizer : MonoBehaviour
{
    /// <summary>The character which position, orientation and scale have to be synchronized and reproduced in the GRETA environment.</summary>
	public GameObject character;
    /// <summary>The character's head.</summary>
	public GameObject characterHead;
    /// <summary>The animation script linked to the GRETA agent we want to add behaviours to.</summary>
    public GretaCharacterAnimator CharacterAnimScript;

    /// <summary>The Thrift command sender linked to our GRETA instance.</summary>
    private CommandSender _commandSender;
    /// <summary>
    /// Indicates whether we've done the initialization of the character synchronized in GRETA or not yet.<br/>
    /// This way, we give the character's initial position once, and then just synchronize it when it change.
    /// </summary>
    private bool _instantiated;

    void Start()
    {
        _commandSender = CharacterAnimScript.commandSender;
        character.transform.hasChanged = false;
    }

    void LateUpdate()
    {
        // Using late update so that the position values we send are taken after all possible calculations (physics, etc)

        if (!_instantiated)
        {
            if (!_commandSender.isConnected()) { return; }

            // Initialise the GRETA environment if it hasn't been done before
            _commandSender.NotifyCharacter(character, characterHead);
            character.transform.hasChanged = false;

            _instantiated = true;
        }
        else
        {
            if (character.transform.hasChanged)
            {
                _commandSender.NotifyCharacter(character, characterHead);
                character.transform.hasChanged = false;
            }
        }
    }
}
