using System.Collections.Generic;


namespace LalaFight
{
    public enum FireModeType
    {
        Semi,
        Burst,
        Auto
    }

    static public class FireModeTypes
    {
        static private Dictionary<FireModeType, string> _names;

        static FireModeTypes()
        {
            _names = new Dictionary<FireModeType, string>()
        {
            {FireModeType.Auto, "Auto" },
            {FireModeType.Burst, "Burst" },
            {FireModeType.Semi, "Semi" }
        };
        }

        static public Dictionary<FireModeType, string> names => _names;
    }
}