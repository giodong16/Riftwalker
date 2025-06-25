/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    public WeaponAnimationData[] weaponAnimationDataList;
    private Dictionary<WeaponType, Dictionary<AnimationState, AnimationClip>> animationDictionary;

    [SerializeField] private Animator animator;
    private WeaponType currentWeapon;
    private AnimationState currentState = AnimationState.Idle;

    private bool isAnimating = false;
    private float animationEndTime = 0f;

    void Start()
    {
     //   animator = GetComponent<Animator>();
        InitializeAnimationDictionary();
       // SetWeapon(WeaponType.Handgun);
    }

    void InitializeAnimationDictionary()
    {
        animationDictionary = new Dictionary<WeaponType, Dictionary<AnimationState, AnimationClip>>();

        foreach (var weaponData in weaponAnimationDataList)
        {
            var animDict = new Dictionary<AnimationState, AnimationClip>();

            foreach (var mapping in weaponData.animations)
            {
                animDict[mapping.state] = mapping.clip;
            }

            animationDictionary[weaponData.weaponType] = animDict;
        }
    }

    public void SetWeapon(WeaponType newWeapon)
    {
        if (currentWeapon != newWeapon)
        {
            currentWeapon = newWeapon;
           // ChangeState(AnimationState.Idle); // Reset trạng thái khi đổi vũ khí
        }
    }

    public void ChangeState(AnimationState newState)
    {
        if (currentState == newState) return; // Không đổi nếu trạng thái giống nhau

        if (CanTransition(currentState, newState))
        {
            currentState = newState;
            PlayAnimation(newState);
        }
    }

    private bool CanTransition(AnimationState currentState, AnimationState newState)
    {
        // Nếu đang trong animation quan trọng, không thể ngắt
        if (isAnimating && Time.time < animationEndTime)
        {
            //Firing,Firing_In_The_Air,Kicking,Run_Firing,Run_Slashing,Run_Throwing,Slashing,Slashing_In_The_Air,Throwing, Throwing_In_The_Air,
            if (currentState == AnimationState.Firing || currentState == AnimationState.Firing_In_The_Air || currentState == AnimationState.Kicking
                || currentState == AnimationState.Run_Firing || currentState == AnimationState.Run_Slashing || currentState == AnimationState.Run_Throwing
                || currentState == AnimationState.Slashing || currentState == AnimationState.Slashing_In_The_Air || currentState == AnimationState.Throwing
                || currentState == AnimationState.Throwing_In_The_Air )
            {
                return false;
            }
        }

        // Quy tắc chuyển đổi
        switch (currentState)
        {
            case AnimationState.Firing:
            case AnimationState.Slashing:
            case AnimationState.Throwing:
                return newState == AnimationState.Idle; // Chỉ quay lại Idle sau khi hoàn tất
            case AnimationState.Jump_Start:
                return newState == AnimationState.Jump_Loop || newState == AnimationState.Falling || newState == AnimationState.Idle;
            case AnimationState.Running:
                return newState != AnimationState.Firing && newState != AnimationState.Slashing;
            default:
                return true;
        }
    }

    public void PlayAnimation(AnimationState state)
    {
        if (animationDictionary.ContainsKey(currentWeapon) && animationDictionary[currentWeapon].ContainsKey(state))
        {
            AnimationClip animClip = animationDictionary[currentWeapon][state];

            if (animClip != null)
            {
                animator.Play(animClip.name);
                isAnimating = true;
                animationEndTime = Time.time + animClip.length;
                StartCoroutine(ResetAnimationState(animClip.length));
            }
        }
    }

    IEnumerator ResetAnimationState(float duration)
    {
        yield return new WaitForSeconds(duration);
        isAnimating = false;
        ChangeState(AnimationState.Idle);
    }
}

public enum AnimationState
{
    Dying,
    Falling,
    Hurt,
    Idle,
    Idle_Blinking,
    Jump_Start,
    Jump_Loop,
    Running,
    Sliding,
    Laughing,

    Firing,
    Firing_In_The_Air,
    Kicking,
    Run_Firing,
    Run_Slashing,
    Run_Throwing,
    Slashing,
    Slashing_In_The_Air,
    Throwing,
    Throwing_In_The_Air,
}
*/