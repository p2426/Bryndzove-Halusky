﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class W_Weapon : Photon.MonoBehaviour {

    private C_Character Character;
    public GameObject Paintball;

    [Header("Attributes")]
    public int clipSize;
    public int ammoCount;
    public float reloadDelay;
    public float shotDelay;
    public AudioSource shotSound;
    public float shotSpeed;
    public Transform Muzzle;

    [Header("Networking")]
    public int paintballID = 0;

    private float shotTime = 0f;
    private float reloadTime = 0f;
    private bool isFiring = false;
    private bool isReloading = false;

    // Use this for initialization
    void Start ()
    {
        Character = transform.root.gameObject.GetComponent<C_Character>();
    }
	
	// Update is called once per frame
	void Update ()
    {

    }

    public virtual bool Fire()
    {
        if (ammoCount <= 0 || isFiring == true || isReloading == true) return false;

        ammoCount = ammoCount - 1;
        StartCoroutine(RunShotDelay());
        Debug.Log("Shooting.. Delay of " + shotDelay + " seconds - Paintball colour is " + Paintball.GetComponent<Renderer>().sharedMaterial);
        CreatePaintball();
        return true;
    }

    public virtual bool Reload()
    {
        if (ammoCount == clipSize) return false;

        Debug.Log("Reloading.. Delay of " + reloadDelay + " seconds");
        StartCoroutine(RunReloadDelay());
        return true;
    }

    public virtual void CreatePaintball()
    {

    }

    public void SetPaintballColour()
    {
        Character = transform.root.gameObject.GetComponent<C_Character>();
        if (Character.Team == "Red") Paintball.GetComponent<Renderer>().material = Paintball.GetComponent<P_Paintball>().redColour;
        else Paintball.GetComponent<Renderer>().material = Paintball.GetComponent<P_Paintball>().blueColour;
    }

    // weapon coroutines
    IEnumerator RunShotDelay()
    {
        while (shotTime < shotDelay)
        {
            isFiring = true;
            shotTime += Time.deltaTime;
            yield return null;
        }

        shotTime = 0f;
        isFiring = false;
        yield break;      
    }

    IEnumerator RunReloadDelay()
    {
        while (reloadTime < reloadDelay)
        {
            isReloading = true;
            reloadTime += Time.deltaTime;
            yield return null;
        }

        reloadTime = 0f;
        isReloading = false;
        ammoCount = clipSize;
        Debug.Log("Ammo now " + ammoCount);
        yield break;
    }

}
