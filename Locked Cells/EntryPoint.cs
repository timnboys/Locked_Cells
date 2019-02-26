using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using Locked_Cells.Utils;
using Rage;
using Rage.Attributes;

[assembly:Plugin("Locked Cells", Author = "NoNameSet", Description = "Keep the cells locked at the downtown pd", PrefersSingleInstance = true)]
namespace Locked_Cells
{
    public static class EntryPoint
    {
        private static Vector5 _firstCellPosition = new Vector5(new Vector3(461.80f, -1001.30f, 25.06f), 0f);
        private static Vector5 _secondCellPosition = new Vector5(new Vector3(461.80f, -997.65f, 25.06f), 0f);
        private static Vector5 _thirdCellPosition = new Vector5(new Vector3(461.80f, -994.40f, 25.06f), 0f);
        
        private static Vector5 _firstCellUnLockPosition = new Vector5(new Vector3(462.31f, -1002.31f, 24.91f), 95.04f);
        private static Vector5 _secondCellUnLockPosition = new Vector5(new Vector3(462.31f, -998.72f, 24.91f), 99.10f);
        private static Vector5 _thirdCellUnLockPosition = new Vector5(new Vector3(462.31f, -993.28f, 24.91f), 100.75f);

        private static readonly List<Vector5> cellUnlockPositions = new List<Vector5>
        {
             _firstCellUnLockPosition,
             _secondCellUnLockPosition,
             _thirdCellUnLockPosition
        };
        
        internal static void Main()
        {
            GameFiber.StartNew(delegate
            {
                Game.DisplayNotification(
                    $"~b~Locked Cells~w~ (v~g~{Assembly.GetExecutingAssembly().GetName().Version}~w~) by NoNameSet has been loaded successfully");
                $"Locked Cells (v{Assembly.GetExecutingAssembly().GetName().Version}) by NoNameSet has been loaded successfully"
                    .AddLog();
                
                while (true)
                {
                    if (cellUnlockPositions.Any(x => x.position.DistanceTo(Common.Player) <= 1f))
                    {
                        var closest = cellUnlockPositions.First(x => x.position.DistanceTo(Common.Player) <= 1f);
                        
                        Common.GetStateOfClosestDoorOfType(631614199, closest.position, out var islocked, out _);
                        Game.DisplayHelp($"Press ~INPUT_CONTEXT~ to {(islocked ? "unlock" : "lock")} the jail cell");

                        if (closest.position.DistanceTo(Common.Player) <= 1f)
                        {
                            if (Game.IsKeyDown(Keys.E))
                            {
                                Common.Player.Tasks.Clear();
                                Common.Player.Tasks.GoStraightToPosition(closest.position, 1f, closest.heading, 0f,
                                    3000).WaitForCompletion();
                                Common.Player.Heading = closest.heading;
                                Common.Player.Tasks.PlayAnimation(new AnimationDictionary("missheistfbisetup1"),
                                    "unlock_exit_janitor", 1400, 3f, 1f, 0f,
                                    AnimationFlags.None);

                                Common.GetStateOfClosestDoorOfType(631614199, closest.position, out var locked, out _);
                                if (locked) Common.SetStateOfClosestDoorOfType(631614199, closest.position, false, 90f);
                                else Common.SetStateOfClosestDoorOfType(631614199, closest.position, true, 0f);
                            }
                        }
                    }
                    
                    GameFiber.Yield();
                }   
            });    
        }
    }
}