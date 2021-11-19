using OscJack;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;

public class Gaze_to_target_Test : MonoBehaviour
{
    public MultiAimConstraint _constraint;
    public MultiAimConstraint _constraint_chest;
    public MultiAimConstraint _constraint_spine2;
    public MultiAimConstraint _constraint_spine3;
    public MultiAimConstraint _constraint_leftshoulder;
    public MultiAimConstraint _constraint_rightshoulder;
    public MultiAimConstraint _constraint_spineV;
    public Transform HeadAim;
    public Transform ChestAim;
    public Transform Spine2Aim;
    public Transform Spine3Aim;
    public Transform SpineVAim;
    public Transform LeftShoulderAim;
    public Transform RightShoulderAim;
    public Transform MoveObject;
    public float ObjecSpeed;
    public bool contains_head = false;
    public bool second_mouve = false;
    public float limite = 0.25f;
    public bool condition = false;
    // Start is called before the first frame update

    void Start()
    {
        
        GameObject character = GameObject.Find("CharacterIngrid").gameObject;
        Debug.Log("Ingrid charged");
        GameObject g = character.gameObject.transform.Find("Rig1").gameObject;
        Debug.Log("FASE1");
        GameObject g1 = g.gameObject.transform.Find("HeadAim").gameObject;
        Debug.Log(g1.ToString());
        RecursiveBones(GameObject.Find("CharacterIngrid").gameObject);
        _constraint.data.offset = new Vector3(90, 0, 0);
        Debug.Log("Offset " + _constraint.data.offset.x);
        GameObject g2 = character.gameObject.transform.Find("MovingObject").gameObject;
        MoveObject = g2.transform;
        Debug.Log("VEDI "+MoveObject);
        MoveObject.position = new Vector3(0, 0, 10);
        GameObject target = g2;
        var wta = new WeightedTransformArray(0);
        wta.Add(new WeightedTransform(g2.transform, 1f));
        _constraint.data.constrainedObject = HeadAim;
        _constraint_chest.data.constrainedObject = ChestAim;
        _constraint_leftshoulder.data.constrainedObject = LeftShoulderAim;
        _constraint_rightshoulder.data.constrainedObject = RightShoulderAim;
        _constraint_spine2.data.constrainedObject = Spine2Aim;
        _constraint_spineV.data.constrainedObject = SpineVAim;
        _constraint_spineV.data.constrainedObject = Spine3Aim;
        _constraint.data.sourceObjects = wta;
        _constraint.data.limits = new Vector2(-90, 130);
        _constraint.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_chest.data.sourceObjects = wta;
        _constraint_chest.data.limits = new Vector2(-90, 90);
        _constraint_chest.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_chest.data.offset = new Vector3(90, 0, 0);
        _constraint_spine2.data.sourceObjects = wta;
        _constraint_spine2.data.limits = new Vector2(-110, 110);
        _constraint_spine2.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_spine2.data.offset = new Vector3(90, 0, 0);

        _constraint_spine3.data.sourceObjects = wta;
        _constraint_spine3.data.limits = new Vector2(-110, 110);
        _constraint_spine3.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_spine3.data.offset = new Vector3(90, 0, 0);

        _constraint_leftshoulder.data.sourceObjects = wta;
        _constraint_leftshoulder.data.limits = new Vector2(-15, 10);
        _constraint_leftshoulder.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_leftshoulder.data.offset = new Vector3(0, 0, 0);
        _constraint_rightshoulder.data.sourceObjects = wta;
        _constraint_rightshoulder.data.limits = new Vector2(-15,-30);
        _constraint_rightshoulder.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_rightshoulder.data.offset = new Vector3(0, 0, 0);

        _constraint_spineV.data.sourceObjects = wta;
        _constraint_spineV.data.limits = new Vector2(60, -60);
        _constraint_spineV.data.aimAxis = (MultiAimConstraintData.Axis)Axis.Y;
        _constraint_spineV.data.offset = new Vector3(90, 0, 0);
        RigBuilder rigs = GetComponent<RigBuilder>();
        rigs.Build();
    }

    // Update is called once per frame
    void Update()
    
    {
        if (MoveObject != null && contains_head==false && second_mouve==false && limite<2)
        {
            //Print the time of when the function is first called.
            
            MoveToTarget();
           // Debug.Log(MoveObject.position.x);
            if ((int)MoveObject.position.x == 10)
            {
                Debug.Log("Fase 1 riuscita "+limite);
                //After we have waited 5 seconds print the time again.
                while(System.Math.Round(MoveObject.position.y, 1) < limite - 0.05f)
                MoveUp(limite);
                Debug.Log(System.Math.Round(MoveObject.position.y, 2));
                if (System.Math.Round(MoveObject.position.y, 1) >= limite-0.10f)
                {
                    Debug.Log("Fase 2 riuscita");
                    contains_head = true;
                    condition = false;
                }
            }
        }
        if (contains_head == true)
        {
            
            if(!condition)
            MoveEst();
            //Debug.Log((int)MoveObject.position.x == -10);
            if ((int)MoveObject.position.x == -10)
            {
                
                second_mouve = true;
                Debug.Log("Fase 3 riuscita ");
                if (second_mouve)
                {
                    MoveUp(limite+0.25f);
                    Debug.Log("Altezza: " + MoveObject.position.y);
                    if (System.Math.Round(MoveObject.position.y, 1) >= limite+0.25f)
                    {
                        Debug.Log("Altezza " + System.Math.Round(MoveObject.position.y, 2));
                        second_mouve = false;
                        contains_head = false;
                        condition = true;
                        limite = limite+0.50f;
                        Debug.Log("CAMBIO LIMITE " + limite);
                    }
                }
            }
        }
    }

    public bool RecursiveBones(GameObject parent)
    {
        int i = 0;
        //Debug.Log("[INFO NUMBER]:" + parent.transform.childCount + "   " + parent.name);
        while (parent.transform.childCount != 0)
        {
           // Debug.Log("[INFO]:" + parent.transform.GetChild(i).gameObject.name);
            RecursiveBones(parent.transform.GetChild(i).gameObject);
            i++;
            if (parent.name == "Head")
            {
                Debug.Log("HEAD Found" + parent);
                HeadAim = parent.transform;

            }
            if (parent.name == "Spine")
            {
                ChestAim = parent.transform;
            }

            if (parent.name == "Spine2")
            {
                Debug.Log("SPINE Found" + parent);
                Spine2Aim = parent.transform;
            }

            if (parent.name == "SpineV")
            {
                Debug.Log("SPINE Found" + parent);
                SpineVAim = parent.transform;
            }

            if (parent.name == "Spine3")
            {
                Debug.Log("SPINE Found" + parent);
                Spine3Aim = parent.transform;
            }

            if (parent.name == "LeftShoulder")
            {
              Debug.Log("Left Shoulder Found" + parent);
              LeftShoulderAim = parent.transform;
            }

            if (parent.name == "RightShoulder")
            {
                Debug.Log("Right Shoulder Found" + parent);
                RightShoulderAim = parent.transform;
            }

            if (i == parent.transform.childCount)
            {
                return false;
            }

        }


        return contains_head;
    }

    public void MoveToTarget()
    {
        if (MoveObject.position.x <= 10) 
        {
            Vector3 vect = new Vector3(10, 0, 0);
           // Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + MoveObject.position + vect + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, MoveObject.position + vect, 1 * Time.fixedDeltaTime);
            //Debug.Log("Moving Object new pos:" + MoveObject.position);

            /*Vector3 vect3 = new Vector3(-10, 0, 0);
            Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + MoveObject.position + vect3 + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, MoveObject.position + vect3, 0.50f * Time.fixedDeltaTime);
            Debug.Log("Moving Object new pos:" + MoveObject.position);
            Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + MoveObject.position + vect2 + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, MoveObject.position + vect2, 0.50f * Time.fixedDeltaTime);
            Debug.Log("Moving Object new pos:" + MoveObject.position);
            */
        }
    }

    public void MoveUp(float lim)
    {
        
        if (MoveObject.position.y <= lim)
        {
            Vector3 vect2 = new Vector3(0, 0.01f, 0);
            //Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + MoveObject.position + vect2 + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, MoveObject.position + vect2, 0.50f * Time.fixedDeltaTime);
            //Debug.Log("Moving Object new pos:" + MoveObject.position);
        }
    }

    public void MoveUp2(float lim)
    {

        if (MoveObject.position.y <= 0.75)
        {
            Vector3 vect2 = new Vector3(0, 0.10f, 0);
            Vector3 g = MoveObject.position + vect2;
           // Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + g + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, g, 0.50f * Time.fixedDeltaTime);
            //Debug.Log("Moving Object new pos:" + MoveObject.position);
        }
    }

    public void MoveEst()
    {
        if (MoveObject.position.x >= -10)
        {
            Vector3 vect3 = new Vector3(-10, 0, 0);
            Vector3 g = MoveObject.position + vect3;
           // Debug.Log("Moving Object from position:" + MoveObject.position + "   to:" + g + "   " + ObjecSpeed * Time.deltaTime);
            MoveObject.position = Vector3.MoveTowards(MoveObject.position, g, 1 * Time.fixedDeltaTime);
           // Debug.Log("Moving Object new pos:" + MoveObject.position);
        }
    }


}


