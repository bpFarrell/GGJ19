namespace Values {
    public class Oxygen {
        public const float BASE             = -.01f;
        public const float BREACH           = -.02f;
        public const float POWERED          = .01f;
        public const float POWEREDBREACH    = POWERED + BREACH;
    }
    public class Resources {
        public const float REDBASE          = 1;
        public const float BLUEBASE         = 1;
        public const float GREENBASE        = 1;
        public const float YELLOWBASE       = 0;

        public const float REDBASEDECAYRATE         = 0f;
        public const float BLUEBASEDECAYRATE        = 0.03f;
        public const float GREENBASEDECAYRATE       = 0.01f;
        public const float YELLOWBASEDECAYRATE      = 0f;

        public const float REDBASERECHARGERATE      = 0.1f;
        public const float BLUEBASERECARGERATE      = 0.1f;
        public const float GREENBASERECHARGERATE    = 0.1f;
        public const float YELLOWBASERECHARGERATE   = 0;

        public static float REDMODRATE{
            get {
                return DockedShip.instance == null ? 0 : DockedShip.instance.redTicSec;
            }
        }
        public static float BLUEMODRATE{
            get {
                return DockedShip.instance == null ? 0 : DockedShip.instance.blueTicSec;
            }
        }
        public static float GREENMODRATE {
            get {
                return DockedShip.instance == null ? 0 : DockedShip.instance.greenTicSec;
            }
        }
        public static float YELLOWMODRATE {
            get {
                return 0;
            }
        }

    }
    public class Hazards
    {
        public const int WEIGHTS                = 10;
        public const float POWER_DOWN_SPEED     = 3;
        public const float POWER_UP_SPEED       = 0.5f;
        public const float REPAIRCOST           = 0.1f;
    }
    public class Colors {
        public static readonly UnityEngine.Color NORMAL_LIGHTS      = new UnityEngine.Color32(79, 71, 15, 136);
        public static readonly UnityEngine.Color EMERGENCY_LIGHTS   = new UnityEngine.Color32(116, 20, 15, 136);
        public static readonly UnityEngine.Color NORMAL_ROOM        = new UnityEngine.Color32(255, 255, 255, 255);
        public static readonly UnityEngine.Color EMERGENCY_ROOM     = new UnityEngine.Color32(100, 80, 80, 255);
    }
}
