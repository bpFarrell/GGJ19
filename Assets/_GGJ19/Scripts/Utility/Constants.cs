namespace Values
{
    public class Oxygen
    {
        public const float BASE          = -.01f;
        public const float BREACH        = -.02f;
        public const float POWERED       =  .01f;
        public const float POWEREDBREACH =  POWERED + BREACH;
    }
    public class Resources
    {
        public const float REDBASE         = 1;
        public const float BLUEBASE        = 1;
        public const float GREENBASE       = 1;
        public const float YELLOWBASE      = 1;
    }
    public class Hazards
    {
        public const int WEIGHTS = 10;
        public const float POWER_DOWN_SPEED = 3;
        public const float POWER_UP_SPEED = 0.5f;
    }
    public class Colors {
        public static readonly UnityEngine.Color NORMAL_LIGHTS = new UnityEngine.Color32(79, 71, 15, 136);
        public static readonly UnityEngine.Color EMERGENCY_LIGHTS = new UnityEngine.Color32(116, 20, 15, 136);
        public static readonly UnityEngine.Color NORMAL_ROOM = new UnityEngine.Color32(255, 255, 255, 255);
        public static readonly UnityEngine.Color EMERGENCY_ROOM = new UnityEngine.Color32(100, 80, 80, 255);
    }
}
