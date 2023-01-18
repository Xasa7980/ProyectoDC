using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Door : MonoBehaviour
{
    Animator anim;
    public bool locked { get; private set; }

    bool open;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Open()
    {
        if (locked || open) return;

        open = true;
        anim.SetTrigger("Open");
    }

    public void Close()
    {
        if(!open) return;

        open = false;
        anim.SetTrigger("Close");
    }

    public void Lock()
    {
        locked = true;
    }

    public void Unlock()
    {
        locked = false;
    }
}
