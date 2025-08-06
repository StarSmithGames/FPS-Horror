namespace Moduls.Physics
{
    public static class LayersUtils
    {
        public static bool Contains( int mask, int layer )
        {
            return ( mask & ( 1 << layer ) ) != 0;
        }

        public static int Combine( int layerA, int layerB )
        {
            return ( 1 << layerA ) | ( 1 << layerB );
        }

        public static int Flip( int layer )
        {
            return ~layer;
        }
    }
}