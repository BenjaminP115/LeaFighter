using System;
using System.ComponentModel;
using System.Reflection;
using UnityEngine;

public class AnimationState
{
    private Animator animator;
    private string currentState;

    public AnimationState(Animator animator)
    {
        this.animator = animator;
    }

    public void ChangeState<T>(T value) where T : struct
    {
        string state = "";

        Type type = value.GetType();
        string name = Enum.GetName(type, value);
        if (name != null)
        {
            FieldInfo field = type.GetField(name);
            if (field != null)
            {
                DescriptionAttribute attr = Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) as DescriptionAttribute;
                if (attr != null)
                {
                    state = attr.Description;
                }
            }
        }

        if (String.IsNullOrEmpty(state)) return;
        if (currentState == state) return;

        currentState = state;

        animator?.Play(state);
    }
}
