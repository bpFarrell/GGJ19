using System.Collections;
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
    	private set{
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
    	private set{
    		s_greenResource = value;
    		s_greenResource = Mathf.Clamp01(s_greenResource);
    	}
    }
    // Power
    private float s_yellowResource;
    public float yellowResource{
    	get{
            return DockedShip.instance == null ? 1 : s_yellowResource/DockedShip.instance.teleportTime;

        }
    	private set{
    		s_yellowResource = value;
            s_yellowResource = Mathf.Clamp(0, DockedShip.instance == null ? 0 : DockedShip.instance.teleportTime, s_yellowResource);
    	}
    }
    public void Initialize() {
        generationState = ResourceColor.NONE;
        redResource     = Values.Resources.REDBASE;
        blueResource    = Values.Resources.BLUEBASE;
        greenResource   = Values.Resources.GREENBASE;
        yellowResource  = Values.Resources.YELLOWBASE;
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
            RoomNode room = PlayerController.instance.currentRoom;
            float leak = 0;
            if (room != null&&room.hasNeededRepairs) {
                leak = 0.02f;
            }
            greenResource -= (Values.Resources.GREENBASEDECAYRATE+leak) * Time.deltaTime;

            if (greenResource == 0) {
                GameManager.Instance.state = GameState.END;
            }
        }
        if ( generationState == ResourceColor.PORTAL) {
            yellowResource += Values.Resources.YELLOWBASERECHARGERATE * Time.deltaTime;
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