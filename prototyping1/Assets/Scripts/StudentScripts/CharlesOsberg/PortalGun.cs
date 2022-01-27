using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [SerializeField] private Transform plyTransform = null;
    
    [SerializeField] private GameObject portalObject1 = null;
    [SerializeField] private GameObject portalObject2 = null;

    public static GameObject curPor1 { get; private set; } = null;
    public static GameObject curPor2 { get; private set; } = null;

    [SerializeField] private SpriteRenderer crosshair1 = null;
    [SerializeField] private Sprite crosshair1Empty = null;
    [SerializeField] private Sprite crosshair1Full = null;
    [SerializeField] private SpriteRenderer crosshair2 = null;
    [SerializeField] private Sprite crosshair2Empty = null;
    [SerializeField] private Sprite crosshair2Full = null;

    private static PortalGun instance = null;

    private PortalGun()
    {
        instance = this;
    }
    
    public static void NotifyPortalSuccess(bool isPortal2)
    {
        if (!isPortal2)
        {
            instance.crosshair1.sprite = instance.crosshair1Full;
        }
        else
        {
            instance.crosshair2.sprite = instance.crosshair2Full;
        }
    }
    
    private GameObject FirePortal(GameObject portalPrefab, GameObject portalInstance)
    {
        var wPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        var pPos = plyTransform.position;

        wPos.z = -0.1f;
        pPos.z = -0.1f;
        if (!portalInstance)
        {
            portalInstance = Instantiate(portalPrefab, pPos, Quaternion.identity);
        }
        else
        {
            portalInstance.transform.position = pPos;
        }

        var pDelta = wPos - pPos;
        portalInstance.GetComponent<PortalTeleport>().Throw(new Vector2(pDelta.x, pDelta.y));

        return portalInstance;
    }

    private void DestroyPortals()
    {
        Destroy(curPor1);
        Destroy(curPor2);

        crosshair1.sprite = crosshair1Empty;
        crosshair2.sprite = crosshair2Empty;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            curPor1 = FirePortal(portalObject1, curPor1);
        }
        if (Input.GetButtonDown("Fire2"))
        {
            curPor2 = FirePortal(portalObject2, curPor2);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            DestroyPortals();
        }
    }
}
