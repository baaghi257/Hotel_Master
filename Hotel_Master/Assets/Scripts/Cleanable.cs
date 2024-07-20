using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Cleanable : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] Image fillImg;
    [SerializeField] GameObject canvas;
    [Header("Settings")]
    [SerializeField]private bool isClean;

    private Animator anim;

    public RoomObjects roomObjects;

    public static Cleanable instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        anim = GetComponent<Animator>();
        MessUp();
    }

    void Update()
    {
        CanvasFaceCamera();

    }

    private void CanvasFaceCamera()
    {
        Vector3 direction = (Camera.main.transform.position - canvas.transform.position).normalized;
        canvas.transform.forward = direction;
    }
    public void Clean(float value)
    {
        fillImg.fillAmount += value;
        if(fillImg.fillAmount >= 1)
        {
            SetAsClean();
        }

    }
    public void MessUp()
    {
        canvas.SetActive(true);

        fillImg.fillAmount = 0;
        isClean = false;
        switch (roomObjects)
        {
            case RoomObjects.Bed:
                anim.Play("BedMessUp");
                break;
            case RoomObjects.Drawer:
                anim.Play("DrawerMessUp");
                break;
            case RoomObjects.Table:
                anim.Play("TableMessUp");
                break;
        }
        
        
    }
    private void SetAsClean()
    {
        canvas.SetActive(false);
        isClean = true;
        switch (roomObjects)
        {
            case RoomObjects.Bed:
                anim.Play("BedCleanUp");
                break;
            case RoomObjects.Drawer:
                anim.Play("DrawerCleanUp");
                break;
            case RoomObjects.Table:
                anim.Play("TableCleanUp");
                break;
        }
    }

    public bool IsClean()
    {
        return isClean;
    }
}

public enum RoomObjects
{
    Bed,
    Drawer,
    Table
}
