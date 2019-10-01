using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using thrift.gen_csharp;
using thrift.services;

namespace thriftImpl
{
    public class CommandSender : Sender
    {
        private int _cpt; // automatically initialized to 0

        public CommandSender() : this(DEFAULT_THRIFT_HOST, DEFAULT_THRIFT_PORT)
        {
        }

        public CommandSender(string host, int port) : base(host, port)
        {
        }

        public void playAnimation(string animationID, InterpersonalAttitude attitude = null)
        {
            if (isConnected())
            {
                Message message = new Message
                {
                    Type = "animID",
                    Time = 0,
                    Id = _cpt.ToString(),
                    String_content = animationID
                };

                // Add property about social attitude model
                if (attitude != null)
                {
                    message.Properties = new Dictionary<string, string>
                    {
                        {"Dominance", attitude.Dominance.ToString()},
                        {"Liking", attitude.Liking.ToString()}
                    };
                }

                _cpt++;
                ThreadPool.QueueUserWorkItem((stateInfo) => { send(message); });
            }
            else {
                Debug.Log("animationReceiver on host: " + getHost() + " and port: " + getPort() + " not connected");
            }
        }

        /// <summary>
        /// Notifies GRETA that the given object has changed its position.<br/>
        /// If GRETA does not know the object, it will be created in its environment.<br/>
        /// If GRETA knows the object, it will be moved in its environment.<br/>
        /// The object is always represented by a cube in GRETA's environment.
        /// </summary>
        /// <param name="gameObject">object to be notified</param>
        public void NotifyObject(GameObject gameObject)
        {
            SendObjectMessage(gameObject);
        }

        /// <summary>
        /// Notifies GRETA that the given object has changed its position.
        /// The GRETA agent will follow it with its gaze with the given gaze influence.<br/>
        /// If GRETA does not know the object, it will be created in its environment.<br/>
        /// If GRETA knows the object, it will be moved in its environment.<br/>
        /// The object is always represented by a cube in GRETA's environment.
        /// </summary>
        /// <param name="gameObject">object to be notified</param>
        /// <param name="gazeInfluence">gaze influence with which to gaze at the object</param>
        public void SendFollowObjectWithGaze (GameObject gameObject,
            GretaObjectTracker.Influence gazeInfluence = GretaObjectTracker.Influence.EYES)
        {
            SendObjectMessage(gameObject, true, gazeInfluence);
        }

        /// <summary>
        /// Notifies GRETA that the given object has changed its position.
        /// The GRETA agent will follow it with its gaze with the given gaze influence if gaze is set to true.<br/>
        /// If GRETA does not know the object, it will be created in its environment.<br/>
        /// If GRETA knows the object, it will be moved in its environment.<br/>
        /// The object is always represented by a cube in GRETA's environment.
        /// </summary>
        /// <param name="gameObject">object to be notified</param>
        /// <param name="gaze">whether or not to gaze at the object</param>
        /// <param name="gazeInfluence">gaze influence with which to gaze at the object</param>
        private void SendObjectMessage(GameObject gameObject,
            bool gaze = false, GretaObjectTracker.Influence gazeInfluence = GretaObjectTracker.Influence.EYES)
        {
            if (isConnected())
            {
                Vector3 position = gameObject.transform.position;
                Quaternion quaternion = gameObject.transform.rotation;
                Vector3 scale = gameObject.transform.localScale;
                Vector3 shift = quaternion
                                * new Vector3(0.5f * scale.x, -0.5f * scale.y, -0.5f * scale.z);
                Message message = new Message
                {
                    Type = "object",
                    Time = 0,
                    Id = _cpt.ToString(),
                    // Some coordinates have to be flipped because GRETA doesn't handle coordinates the same way as Unity
                    // The X axis for position is reversed in GRETA, as well as the Y and Z axis for rotation.
                    // Coordinates also have to be changed because objects in GRETA have their pivot at their bottom,
                    //     while objects in Unity have their pivot in their center
                    Properties = new Dictionary<string, string>
                    {
                        {"position.x", (-(position.x + shift.x)).ToString()},
                        {"position.y", (position.y + shift.y).ToString()},
                        {"position.z", (position.z + shift.z).ToString()},
                        {"quaternion.x", quaternion.x.ToString()},
                        {"quaternion.y", (-quaternion.y).ToString()},
                        {"quaternion.z", (-quaternion.z).ToString()},
                        {"quaternion.w", quaternion.w.ToString()},
                        {"scale.x", scale.x.ToString()},
                        {"scale.y", scale.y.ToString()},
                        {"scale.z", scale.z.ToString()},
                        {"id", gameObject.name + gameObject.GetInstanceID()}
                    }
                };
                if (gaze)
                {
                    message.Properties.Add("gaze", "true");
                    message.Properties.Add("influence", gazeInfluence.ToString());
                }

                _cpt++;
                ThreadPool.QueueUserWorkItem((stateInfo) => { send(message); });
            }
            else
            {
                Debug.Log("commandReceiver on host: " + getHost() + " and port: " + getPort() + " not connected");
            }
        }
    }
}