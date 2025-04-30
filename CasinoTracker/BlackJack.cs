using MelonLoader;
using Il2CppScheduleOne.Casino;
using HarmonyLib;
using MelonLoader.Utils;
using Newtonsoft.Json;

namespace CasinoTracker
{
    // Json stuff
    public class BlackjackStats
    {
        public int total_plays;
        public int total_wins;
        public int total_losses;
        public int total_pushes;
        public int total_profit;
        public int total_spent;
        public float profit_per_play;

        private static string SavePath => Path.Combine(MelonEnvironment.ModsDirectory, "CasinoTracker", "BlackjackStats.json");

        public static void SaveFromFields()
        {
            var stats = new BlackjackStats
            {
                total_plays = BlackJack.total_plays,
                total_wins = BlackJack.total_wins,
                total_losses = BlackJack.total_losses,
                total_pushes = BlackJack.total_pushes,
                total_profit = BlackJack.total_profit,
                total_spent = BlackJack.total_spent,
                profit_per_play = BlackJack.profit_per_play
            };

            var json = JsonConvert.SerializeObject(stats, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        public static void LoadToFields()
        {
            if (!File.Exists(SavePath)) return;
            var json = File.ReadAllText(SavePath);
            var stats = JsonConvert.DeserializeObject<BlackjackStats>(json);
            if (stats == null) return;

            BlackJack.total_plays = stats.total_plays;
            BlackJack.total_wins = stats.total_wins;
            BlackJack.total_losses = stats.total_losses;
            BlackJack.total_pushes = stats.total_pushes;
            BlackJack.total_profit = stats.total_profit;
            BlackJack.total_spent = stats.total_spent;
            BlackJack.profit_per_play = stats.profit_per_play;
        }
    }

    public class BlackJack : MelonMod
    {
        public static int total_wins = 0;
        public static int total_plays = 0;
        public static int total_losses = 0;
        public static int total_pushes = 0;
        public static int total_spent = 0;
        public static int total_profit = 0;
        public static float win_rate = 0f;
        public static float profit_per_play = 0f;

        [HarmonyPatch(typeof(BlackjackGameController), "GetPayout")]
        public static class BlackjackGameController_GetPayout_Patch
        {
            public static void Postfix(float bet, BlackjackGameController.EPayoutType payout, float __result)
            {
                //MelonLogger.Msg($"Blackjack payout: {__result} on bet: {bet}, payout type: {payout}");
                // __result is how much you win (so bet * 2)
                // bet is how much you put in
                // payout is the type of payout ("Win" or "None")
                UpdateTracker(payout.ToString(), (int)bet);
                GamblerApp.UpdateBlackJackStatsText();
            }

            public static void UpdateTracker(string outcome, int BetAmount)
            {
                switch (outcome)
                {
                    case "Win":
                        total_plays++;
                        total_wins++;
                        total_profit += BetAmount * 2 - BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "Push":
                        total_plays++;
                        total_pushes++;
                        total_profit += BetAmount - BetAmount;
                        total_spent += BetAmount;
                        break;
                    case "None":
                        total_plays++;
                        total_losses++;
                        total_profit -= BetAmount;
                        total_spent += BetAmount;
                        break;
                    default:
                        MelonLogger.Warning($"Unknown outcome: {outcome}");
                        break;
                }
                win_rate = (total_plays > 0) ? (float)total_wins / total_plays * 100 : 0f;
                win_rate = (float)Math.Round(win_rate, 2);
                profit_per_play = (total_plays > 0) ? (float)total_profit / total_plays : 0f;
                profit_per_play = (float)Math.Round(profit_per_play, 2);
                // Save stats to JSON file
                BlackjackStats.SaveFromFields();
            }
        }

    }
}
