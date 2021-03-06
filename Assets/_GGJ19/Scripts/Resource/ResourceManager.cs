﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Flags]
public enum ResourceColor {
    NONE    = 0,
    RED     = 1,
    GREEN   = 2,
    BLUE    = 4,
    PORTAL  = 8
        
}
public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    public ResourceColor generationState = ResourceColor.NONE;
    // Red Resource 
    private float s_redResource;
    public float redResource{
    	get{
    		return s_redResource;
    	}
    	set{
    		s_redResource = value;
    		s_redResource = Mathf.Clamp01(s_redResource);
    	}
    }
    // Blue Resource
    private float s_blueResource;
    public float blueResource{
    	get{
    		return s_blueResource;
    	}
    	set{
    		s_blueResource = value;
    		s_blueResource = Mathf.Clamp01(s_blueResource);
    	}
    }
    // Oxygen
    private float s_greenResource;
    public float greenResource{
    	get{
    		return s_greenResource;
    	}
    	set{
    		s_greenResource = value;
    		s_greenResource = Mathf.Clamp01(s_greenResource);
    	}
    }
    // Power
    public float timeTillTeleport = 90;
    private float s_yellowResource;
    private bool isInit = false;

    public float yellowResource{
    	get{
            return s_yellowResource;
        }
    	set{
    		s_yellowResource = value;
            s_yellowResource = Mathf.Clamp(s_yellowResource, 0, timeTillTeleport);
    	}
    }
    public void Initialize() {
        if (isInit) return;
        generationState = ResourceColor.NONE;
        redResource     = Values.Resources.REDBASE;
        blueResource    = Values.Resources.BLUEBASE;
        greenResource   = Values.Resources.GREENBASE;
        yellowResource  = Values.Resources.YELLOWBASE;
        isInit = true;
    }
    public bool isOverLoaded {
        get {
            if (
                (int)generationState != 0 ||
                (int)generationState != 1 ||
                (int)generationState != 2 ||
                (int)generationState != 4 ||
                (int)generationState != 8)
                return false;
            else
                return true;
        }
    }
    private void Update() {
        if (!isInit) return;
        if (isOverLoaded) return;
        float speed = 0.05f;
        if (generationState == ResourceColor.RED) {
            redResource += Values.Resources.REDBASERECHARGERATE * Time.deltaTime;
        } 
        if (generationState == ResourceColor.BLUE) {
            blueResource += Values.Resources.BLUEBASERECARGERATE * Time.deltaTime;
        } else {
            blueResource -= Values.Resources.BLUEBASEDECAYRATE * Time.deltaTime;
        }
        if (generationState == ResourceColor.GREEN) {
            greenResource += Values.Resources.GREENBASERECHARGERATE * Time.deltaTime;
        } else {
            if (PlayerController.Instance != null)
            {
                RoomNode room = PlayerController.Instance.currentRoom;
                float leak = 0;
                if (room != null && room.hasNeededRepairs)
                {
                    leak = 0.02f;
                }
                greenResource -= (Values.Resources.GREENBASEDECAYRATE+leak) * Time.deltaTime;

                if (greenResource == 0) {
                    GameManager.Instance.state = GameState.END;
                }
            }
        }
        if ( generationState == ResourceColor.PORTAL) {
            yellowResource += Values.Resources.YELLOWBASERECHARGERATE * Time.deltaTime;
            if (ResourceManager.Instance.yellowResource == ResourceManager.Instance.timeTillTeleport
                && DockedShip.instance != null) {
                CutSceneManager.Instance.ChangeState(CutSceneManager.CutSceneState.ShipLeaving);
                yellowResource = 0;
            }
        }
    }
    public void Cleanup() {
        generationState = ResourceColor.NONE;
        redResource     = Values.Resources.REDBASE;
        blueResource    = Values.Resources.BLUEBASE;
        greenResource   = Values.Resources.GREENBASE;
        yellowResource  = Values.Resources.YELLOWBASE;
    }
}