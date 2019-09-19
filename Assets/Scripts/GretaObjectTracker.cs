using UnityEngine;
using System.Collections.Generic;
using thriftImpl;

/// <summary>
/// Simple behaviour script to track objects in GRETA and to follow an object with an agent's gaze.
/// </summary>
public class GretaObjectTracker : MonoBehaviour
{
	/// <summary>The object to follow with the agent's gaze.</summary>
	public GameObject ObjectToFollowWithGaze;
	/// <summary>The objects which's positions, orientations and scales have to be tracked and reproduced in the GRETA environment.</summary>
	public List<GameObject> TrackedObjects = new List<GameObject>();
	/// <summary>Simple enum for the influence of the agent's gaze, as in the body parts that should move to look at something.</summary>
    public enum Influence {EYES, HEAD, SHOULDER, TORSO, WHOLE};
	/// <summary>The influence of the agent's gaze, as in the body parts that should move to look at something.</summary>
	public Influence GazeInfluence = Influence.EYES;
	/// <summary>The animation script linked to the GRETA agent we want to add behaviours to.</summary>
	public CharacterAnimation_Single_Autodesk CharacterAnimScript;

	/// <summary>The Thrift command sender linked to our GRETA instance.</summary>
    private CommandSender _commandSender;
	/// <summary>
	/// Indicates whether we've done the initialization of the objects tracked in GRETA or not yet.<br/>
	/// This way, we give the object's initial position once, and then just track the ones who changed.
	/// </summary>
    private bool _instantiated;
	/// <summary>
	/// Indicates whether we're following an object, so if ObjectToFollowWithGaze is not null at Start.
	/// That way, we consider it cannot be set to null in the middle of the program, and don't check if it is
	/// null or not every frame. Basically, it's optimisation.
	/// </summary>
    private bool _isFollowingWithGaze;

	void Start()
	{
		_commandSender = CharacterAnimScript.commandSender;
		foreach (GameObject trackedObject in TrackedObjects)
		{
			trackedObject.transform.hasChanged = false;
		}
		if (ObjectToFollowWithGaze != null)
		{
			_isFollowingWithGaze = true;
			ObjectToFollowWithGaze.transform.hasChanged = false;
		}
	}

	void LateUpdate () {
		// Using late update so that the position values we send are taken after all possible calculations (physics, etc)

		if (!_instantiated)
		{
			if (!_commandSender.isConnected()) { return; }

			// Initialise the GRETA environment if it hasn't been done before
			foreach (GameObject trackedObject in TrackedObjects)
			{
				_commandSender.NotifyObject(trackedObject);
				trackedObject.transform.hasChanged = false;
			}

			if (_isFollowingWithGaze)
			{
				CharacterAnimScript.FollowObjectWithGaze(ObjectToFollowWithGaze);
				ObjectToFollowWithGaze.transform.hasChanged = false;
			}

			_instantiated = true;
		}
		else
		{
			foreach (GameObject trackedObject in TrackedObjects)
			{
				// If the trackedObject has changed since the last frame, update the GRETA Environment.
				if (trackedObject.transform.hasChanged)
				{
					_commandSender.NotifyObject(trackedObject);
					trackedObject.transform.hasChanged = false;
				}
			}

			if (_isFollowingWithGaze && ObjectToFollowWithGaze.transform.hasChanged)
			{
				// If the trackedObject has changed since the last frame, update the GRETA Environment.
				CharacterAnimScript.FollowObjectWithGaze(ObjectToFollowWithGaze, GazeInfluence);
				ObjectToFollowWithGaze.transform.hasChanged = false;
			}
		}
	}
}
