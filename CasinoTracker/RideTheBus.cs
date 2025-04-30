using MelonLoader;
using Il2CppScheduleOne.Casino;
using HarmonyLib;
using MelonLoader.Utils;
using Newtonsoft.Json;

namespace CasinoTracker
{

    // Json stuff
    public class RideTheBusStats
    {
        public int total_plays;
        public int total_spent;
        public int total_profit;
        public float profit_per_play;
        public int times_finished_at_r1;
        public int times_finished_at_r2;
        public int times_finished_at_r3;
        public int times_finished_at_r4;

        private static string SavePath => Path.Combine(MelonEnvironment.ModsDirectory, "CasinoTracker", "RideTheBusStats.json");

        public static void SaveFromFields()
        {
            var stats = new RideTheBusStats
            {
                total_plays = RideTheBus.total_plays,
                total_spent = RideTheBus.total_spent,
                total_profit = RideTheBus.total_profit,
                profit_per_play = RideTheBus.profit_per_play,
                times_finished_at_r1 = RideTheBus.times_finished_at_r1,
                times_finished_at_r2 = RideTheBus.times_finished_at_r2,
                times_finished_at_r3 = RideTheBus.times_finished_at_r3,
                times_finished_at_r4 = RideTheBus.times_finished_at_r4
            };

            var json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        public static void LoadToFields()
        {
            if (!File.Exists(SavePath)) return;
            var json = File.ReadAllText(SavePath);
            var stats = JsonConvert.DeserializeObject<RideTheBusStats>(json);
            if (stats == null) return;

            RideTheBus.total_plays = stats.total_plays;
            RideTheBus.total_spent = stats.total_spent;
            RideTheBus.total_profit = stats.total_profit;
            RideTheBus.profit_per_play = stats.profit_per_play;
            RideTheBus.times_finished_at_r1 = stats.times_finished_at_r1;
            RideTheBus.times_finished_at_r2 = stats.times_finished_at_r2;
            RideTheBus.times_finished_at_r3 = stats.times_finished_at_r3;
            RideTheBus.times_finished_at_r4 = stats.times_finished_at_r4;
        }
    }



    public class RideTheBus : MelonMod
    {
        public static int total_plays = 0;
        public static int total_spent = 0;
        public static int total_profit = 0;
        public static float win_rate = 0f;
        public static float profit_per_play = 0f;
        public static int times_finished_at_r1 = 0;
        public static int times_finished_at_r2 = 0;
        public static int times_finished_at_r3 = 0;
        public static int times_finished_at_r4 = 0;
        private static bool? lastRoundWasWin = null; // true = win, false = loss, null = unknown/exit
        private static int currentStage = 0; // Start at 0


        [HarmonyPatch(typeof(RTBGameController), "Awake")]
        public static class RTBGameController_Awake_Patch
        {
            public static void Postfix(RTBGameController __instance)
            {
                __instance.onLocalPlayerCorrect += new Action(() =>
                {
                    //MelonLogger.Msg("Player got it correct!");
                    lastRoundWasWin = true;
                    currentStage++; // Advance stage only on correct answer
                });


                __instance.onLocalPlayerIncorrect += new Action(() =>
                {
                    //MelonLogger.Msg("Player got it incorrect.");
                    lastRoundWasWin = false;
                });

                __instance.onLocalPlayerExitRound += new Action(() =>
                {
                    //MelonLogger.Msg("Player exited the round.");
                    lastRoundWasWin = null;
                });

            }
        }

        [HarmonyPatch(typeof(RTBGameController), "EndGame")]
        public static class RTBGameController_EndGame_Patch
        {
            public static void Postfix(RTBGameController __instance)
            {
                var stage_multipliers = new List<int> { 2, 3, 4, 20 };
                var bet = (int)__instance.LocalPlayerBet;
                var multiplier = (int)__instance.LocalPlayerBetMultiplier;

                int payout = 0;

                if (lastRoundWasWin == true)
                {
                    // Game was won at the current stage
                    payout = bet * multiplier;
                }
                else if (lastRoundWasWin == null)
                {
                    // Player exited early: use previous stage
                    if (currentStage > 0)
                    {
                        int finalStageIndex = currentStage - 1;
                        payout = bet * stage_multipliers[finalStageIndex];
                        multiplier = stage_multipliers[finalStageIndex]; // fix for stats display
                    }
                }
                else
                {
                    // Loss: no payout
                    payout = 0;
                }

                int stage_display = stage_multipliers.IndexOf(multiplier) + 1;
                UpdateTracker(stage_display.ToString(), bet, payout);
                GamblerApp.UpdateRTBStatsText();

                // Reset
                currentStage = 0;
                lastRoundWasWin = null;
            }

            public static void UpdateTracker(string outcome, int BetAmount, int Payout)
            {
                total_plays++;
                total_spent += BetAmount;
                total_profit += Payout - BetAmount;

                switch (outcome)
                {
                    case "1": times_finished_at_r1++; break;
                    case "2": times_finished_at_r2++; break;
                    case "3": times_finished_at_r3++; break;
                    case "4": times_finished_at_r4++; break;
                    default:
                        MelonLogger.Warning($"Unknown outcome/round: {outcome}");
                        break;
                }

                profit_per_play = (total_plays > 0) ? (float)total_profit / total_plays : 0f;
                profit_per_play = (float)Math.Round(profit_per_play, 2);
                // Save stats to JSON file
                RideTheBusStats.SaveFromFields();
            }
        }
    }
    }
