using Rage;
using Rage.Native;

namespace Locked_Cells.Utils
{
    internal static class Common
    {
        public static Ped Player => Game.LocalPlayer.Character;
        
        public static void AddLog(this string s)
        {
            Game.LogTrivial(s);
        }

        public static void SetStateOfClosestDoorOfType(uint hash, Vector3 position, bool locked, float heading)
        {
            NativeFunction.Natives.SetStateOfClosestDoorOfType(hash, position.X, position.Y, position.Z, locked,
                heading, 0);
        }

        public static void GetStateOfClosestDoorOfType(uint hash, Vector3 position, out bool locked, out float heading)
        {
            NativeFunction.Natives.GetStateOfClosestDoorOfType(hash, position.X, position.Y, position.Z, out locked,
                out heading);
        }
    }
}