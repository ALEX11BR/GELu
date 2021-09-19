using System;

namespace Utils
{
    public static class Util
    {
        public const float ACCELERATION = 512;
        public const float MAXSPEED = 64;
        public const float ROTATIONBOOST = 5;
        public const float ROTATIONSPEED = 120;
        public const float FRICTION = 0.25F;
        public const float GRAVITY = 200;
        public const float JUMPFORCE = 128;

        public static float clamp0(float val, float lim)
        {
            if (val < -lim)
            {
                return -lim;
            }
            if (val > lim)
            {
                return lim;
            }
            return val;
        }
        public static float lerp0(float val, float by)
        {
            return val*(1-by);
        }
    }
}