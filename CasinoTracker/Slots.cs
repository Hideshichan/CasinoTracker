using MelonLoader;
using Il2CppScheduleOne.Casino;
using HarmonyLib;
using MelonLoader.Utils;
using Newtonsoft.Json;


namespace CasinoTracker
{
    public class SlotStats
    {
        public int total_wins;
        public int total_spins;
        public int total_losses;
        public int total_spent;
        public int total_profit;
        public float win_rate;
        public float profit_per_spin;
        public int total_mini_wins;
        public int total_small_wins;
        public int total_big_wins;
        public int total_jackpots;

        private static string SavePath => Path.Combine(MelonEnvironment.ModsDirectory, "CasinoTracker", "SlotStats.json");

        public static void SaveFromFields()
        {
            var stats = new SlotStats
            {
                total_wins = Slots.total_wins,
                total_spins = Slots.total_spins,
                total_losses = Slots.total_losses,
                total_spent = Slots.total_spent,
                total_profit = Slots.total_profit,
                win_rate = Slots.win_rate,
                profit_per_spin = Slots.profit_per_spin,
                total_mini_wins = Slots.total_mini_wins,
                total_small_wins = Slots.total_small_wins,
                total_big_wins = Slots.total_big_wins,
                total_jackpots = Slots.total_jackpots

            };

            MelonLogger.Msg($"Attempting to save SlotStats to {SavePath}.");
            //MelonLogger.Msg($"Total spins: {stats.total_spins}");
            var json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            //MelonLogger.Msg($"Serialized JSON: {json}");
            File.WriteAllText(SavePath, json);
        }

        public static void LoadToFields()
        {
            if (!File.Exists(SavePath)) return;
            var json = File.ReadAllText(SavePath);
            var stats = JsonConvert.DeserializeObject<SlotStats>(json);
            if (stats == null) return;

            Slots.total_spins = stats.total_spins;
            Slots.total_spent = stats.total_spent;
            Slots.total_profit = stats.total_profit;
            Slots.total_wins = stats.total_wins;
            Slots.profit_per_spin = stats.profit_per_spin;
        }
    }

    public class Slots : MelonMod
    {

        public static int total_wins = 0;
        public static int total_spins = 0;
        public static int total_losses = 0;
        public static int total_spent = 0;
        public static int total_profit = 0;
        public static float win_rate = 0f;
        public static float profit_per_spin = 0f;
        public static int total_mini_wins = 0;
        public static int total_small_wins = 0;
        public static int total_big_wins = 0;
        public static int total_jackpots = 0;

        [HarmonyPatch(typeof(SlotMachine), "HandleInteracted")]
        public static class SlotMachine_HandleInteracted_Patch
        {
            public static void Postfix(SlotMachine __instance)
            {
                //MelonLogger.Msg("Slot machine was interacted with.");
                total_spins++;
            }
        }

        [HarmonyPatch(typeof(SlotMachine), "EvaluateOutcome")]
        public static class Slotmachine_EvaluateOutcome_Patch
        {
            public static void Postfix(SlotMachine.EOutcome __result, SlotMachine __instance)
            {

                int currentBetAmount = __instance.currentBetAmount;
                string result = __result.ToString();
                // MelonLogger.Msg($"Current bet amount: {currentBetAmount}.");
                // MelonLogger.Msg($"Slot machine outcome evaluated: {__result}.");
                // outcomes = ["NoWin", "MiniWin", "SmallWin", "BigWin", "Jackpot"];
                UpdateTracker(result, currentBetAmount);
                GamblerApp.UpdateSlotsStatsText();
            }

            public static void UpdateTracker(string outcome, int BetAmount)
            {
                switch (outcome)
                {
                    case "NoWin":
                        total_losses++;
                        total_profit -= BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "MiniWin":
                        total_wins++;
                        total_mini_wins++;
                        total_profit += BetAmount * 2 - BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "SmallWin":
                        total_wins++;
                        total_small_wins++;
                        total_profit += BetAmount * 10 - BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "BigWin":
                        total_wins++;
                        total_big_wins++;
                        total_profit += BetAmount * 25 - BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "Jackpot":
                        total_wins++;
                        total_jackpots++;
                        total_profit += BetAmount * 100 - BetAmount;
                        total_spent += BetAmount;
                        break;
                    default:
                        MelonLogger.Warning($"Unknown outcome: {outcome}");
                        break;
                }
                win_rate = (total_spins > 0) ? (float)total_wins / total_spins * 100 : 0f;
                win_rate = (float)Math.Round(win_rate, 2);
                profit_per_spin = (total_spins > 0) ? (float)total_profit / total_spins : 0f;
                profit_per_spin = (float)Math.Round(profit_per_spin, 2);
                // Save stats to JSON file
                SlotStats.SaveFromFields();
            }
        }

    }
}