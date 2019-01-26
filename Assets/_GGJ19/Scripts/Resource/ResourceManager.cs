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
    	private set{
    		s_redResource = value;
    		Mathf.Clamp01(s_redResource);
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
    		Mathf.Clamp01(s_blueResource);
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
    		Mathf.Clamp01(s_greenResource);
    	}
    }
    // Power
    private float s_yellowResource;
    public float yellowResource{
    	get{
    		return s_yellowResource;
    	}
    	private set{
    		s_yellowResource = value;
    		Mathf.Clamp01(s_yellowResource);
    	}
    }
    public void Initialize() {
        generationState = ResourceColor.NONE;
        redResource     = Values.Resources.REDBASE;
        blueResource    = Values.Resources.BLUEBASE;
        greenResource   = Values.Resources.GREENBASE;
        yellowResource  = Values.Resources.YELLOWBASE;
    }
    private void Update() {
        float speed = 0.05f;
        if (generationState == ResourceColor.RED) {
            redResource += speed * Time.deltaTime;
        }
        if (generationState == ResourceColor.BLUE) {
            blueResource += speed * Time.deltaTime;
        }
        if (generationState == ResourceColor.GREEN) {
            greenResource += speed * Time.deltaTime;
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