using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : SingletonBehaviour<ResourceManager>
{
    // Red Resource 
    private float s_redResource;
    public float redResource{
    	get{
    		return s_redResource;
    	}
    	private set{
    		Mathf.Clamp01(value);
    		s_redResource = value;
    	}
    }
    // Blue Resource
    private float s_blueResource;
    public float blueResource{
    	get{
    		return s_blueResource;
    	}
    	private set{
    		Mathf.Clamp01(value);
    		s_blueResource = value;
    	}
    }
    // Oxygen
    private float s_greenResource;
    public float greenResource{
    	get{
    		return s_greenResource;
    	}
    	private set{
    		Mathf.Clamp01(value);
    		s_greenResource = value;
    	}
    }
    // Power
    private float s_yellowResource;
    public float yellowResource{
    	get{
    		return s_yellowResource;
    	}
    	private set{
    		Mathf.Clamp01(value);
    		s_yellowResource = value;
    	}
    }

    private void Cleanup() {
        redResource     = Values.Resources.RedBase;
        blueResource    = Values.Resources.BlueBase;
        greenResource   = Values.Resources.GreenBase;
        yellowResource  = Values.Resources.YellowBase;
    }
}