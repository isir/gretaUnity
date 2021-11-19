using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Animations;
using UnityEngine.Animations.Rigging;
using UnityEditor.Animations;

public class Blend_tree_dynamic : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        var animator = GetComponent<Animator>();
        var idleClip = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Mixamo/Idle.anim", typeof(AnimationClip));
        var jumpClip = (AnimationClip)AssetDatabase.LoadAssetAtPath("Assets/Mixamo/Jumping.anim", typeof(AnimationClip));
        var controller = (AnimatorController)animator.runtimeAnimatorController;
        controller.AddParameter("BlendEvent", AnimatorControllerParameterType.Float);
        UnityEditor.Animations.BlendTree blendTree;
        //controller.CreateBlendTreeInController("BlendState", out blendTree, 0);
        //blendTree.name = "Blend Tree";
        //blendTree.blendParameter = "BlendEvent";
        //blendTree.AddChild(idleClip);
        //blendTree.AddChild(jumpClip);
        //UnityEditor.Animations.AnimatorControllerLayer[] layers = controller.layers;
        if (controller.layers.Length == 1)
        {
            
            controller.AddLayer("Head");
            controller.AddLayer("Gesture");
            controller.AddLayer("Torso");
            controller.AddLayer("LowerBody");
        }
        Debug.Log(animator.runtimeAnimatorController.name);
        

    }


    // Update is called once per frame
    void Update()
    {
        //(string name, float value, float dampTime, deltaTime)
        var animator = GetComponent<Animator>();
        Debug.Log("Done");
        animator.SetFloat("BlendEvent", 0.5f);
            
        
    }
}
