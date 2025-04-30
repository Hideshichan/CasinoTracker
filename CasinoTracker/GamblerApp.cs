using UnityEngine;
using S1API.PhoneApp;
using MelonLoader;
using S1API.UI;
using UnityEngine.UI;
using MelonLoader.Utils;

namespace CasinoTracker;
public class GamblerApp : PhoneApp
{
    string iconPath = Path.Combine(MelonEnvironment.ModsDirectory, "CasinoTracker/assets", "app_icon.png");
    protected override string AppName => "GamblerApp";
    protected override string AppTitle => "Casino Stats";
    protected override string IconLabel => "Casino Stats";
    protected override string IconFileName => iconPath;

    // Slots
    private static Text slots_total_wins_text, slots_total_spins_text, slots_total_losses_text, slots_total_spent_text, slots_total_profit_text, slots_win_rate_text, slots_profit_per_spin_text, slots_total_mini_wins_text, slots_total_small_wins_text, slots_total_big_wins_text, slots_total_jackpots_text;
    // Blackjack
    private static Text blackjack_total_wins_text, blackjack_total_pushes_text, blackjack_total_plays_text, blackjack_total_losses_text, blackjack_total_spent_text, blackjack_total_profit_text, blackjack_win_rate_text, blackjack_profit_per_play_text;
    // RTB
    private static Text rtb_total_plays_text, rtb_total_spent_text, rtb_total_profit_text, rtb_profit_per_play_text, rtb_times_at_r1_text, rtb_times_at_r2_text, rtb_times_at_r3_text, rtb_times_at_r4_text;

    protected override void OnCreated()
    {
        base.OnCreated();
        MelonLogger.Msg("[GamblerApp] OnCreated called.");
    }

    protected override void OnCreatedUI(GameObject container)
    {
        var bg = UIFactory.Panel("Background", container.transform, Color.gray, fullAnchor: true);
        var topbar = UIFactory.Panel("TopBar", bg.transform, Color.black, new Vector2(0f, 0.9f), new Vector2(1f, 1f));
        // add separators in the menu between the 3 games
        var far_left_separator = UIFactory.Panel("FarLeftSeparator", bg.transform, Color.black, new Vector2(0f, 0.0f), new Vector2(0.01f, 0.9f));
        var left_separator = UIFactory.Panel("LeftSeparator", bg.transform, Color.black, new Vector2(0.32f, 0.0f), new Vector2(0.34f, 0.9f));
        var right_separator = UIFactory.Panel("RightSeparator", bg.transform, Color.black, new Vector2(0.66f, 0.0f), new Vector2(0.68f, 0.9f));
        var far_right_separator = UIFactory.Panel("FarRightSeparator", bg.transform, Color.black, new Vector2(0.99f, 0.0f), new Vector2(1f, 0.9f));

        // ok so the phone has a border that is 0.04 on the left and 0.04 on the right, so i have 0.92 available space
        var slots_section =     UIFactory.Panel("SlotsSection", bg.transform, Color.clear, new Vector2(0.04f, 0f), new Vector2(0.29f, 1));
        var blackjack_section = UIFactory.Panel("BlackjackSection", bg.transform, Color.clear, new Vector2(0.37f, 0f), new Vector2(0.63f, 1));
        var rtb_section =       UIFactory.Panel("RTBSection", bg.transform, Color.clear, new Vector2(0.71f, 0f), new Vector2(0.96f, 1));

        // *************************************************** SLOTS ***************************************************

        // Title panel
        var slots_title_panel =                     UIFactory.Panel("SlotsTitlePanel", slots_section.transform, Color.clear, new Vector2(0.34f, 0.8f), new Vector2(0.66f, 0.85f));
        // Stats panels
        var slots_stats_total_wins_panel =          UIFactory.Panel("SlotsStatsTotalWinsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.70f), new Vector2(0.8f, 0.70f));
        var slots_stats_total_spins_panel =         UIFactory.Panel("SlotsStatsTotalSpinsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.65f), new Vector2(0.8f, 0.65f));
        var slots_stats_total_losses_panel =        UIFactory.Panel("SlotsStatsTotalLossesPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.60f), new Vector2(0.8f, 0.60f));
        var slots_stats_total_spent_panel =         UIFactory.Panel("SlotsStatsTotalSpentPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.55f), new Vector2(0.8f, 0.55f));
        var slots_stats_total_profit_panel =        UIFactory.Panel("SlotsStatsTotalProfitPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.50f), new Vector2(0.8f, 0.50f));
        var slots_stats_win_rate_panel =            UIFactory.Panel("SlotsStatsWinRatePanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.45f), new Vector2(0.8f, 0.45f));
        var slots_stats_profit_per_spin_panel =     UIFactory.Panel("SlotsStatsProfitPerSpinPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.40f), new Vector2(0.8f, 0.40f));
        var slots_stats_total_mini_wins_panel =     UIFactory.Panel("SlotsStatsTotalMiniWinsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.35f), new Vector2(0.8f, 0.35f));
        var slots_stats_total_small_wins_panel =    UIFactory.Panel("SlotsStatsTotalSmallWinsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.30f), new Vector2(0.8f, 0.30f));
        var slots_stats_total_big_wins_panel =      UIFactory.Panel("SlotsStatsTotalBigWinsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.25f), new Vector2(0.8f, 0.25f));
        var slots_stats_total_jackpots_panel =      UIFactory.Panel("SlotsStatsTotalJackpotsPanel", slots_section.transform, Color.clear, new Vector2(0.2f, 0.20f), new Vector2(0.8f, 0.20f));

        // Setting text for the stats panels
        slots_total_wins_text =         UIFactory.Text("SlotsTotalWinsText", $"Total Wins: {Slots.total_wins}", slots_stats_total_wins_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_spins_text =        UIFactory.Text("SlotsTotalSpinsText", $"Total Spins: {Slots.total_spins}", slots_stats_total_spins_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_losses_text =       UIFactory.Text("SlotsTotalLossesText", $"Total Losses: {Slots.total_losses}", slots_stats_total_losses_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_spent_text =        UIFactory.Text("SlotsTotalSpentText", $"Total Spent: {Slots.total_spent}", slots_stats_total_spent_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_profit_text =       UIFactory.Text("SlotsTotalProfitText", $"Total Profit: {Slots.total_profit}", slots_stats_total_profit_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_win_rate_text =           UIFactory.Text("SlotsWinRateText", $"Win Rate: {Slots.win_rate}%", slots_stats_win_rate_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_profit_per_spin_text =    UIFactory.Text("SlotsProfitPerSpinText", $"Average Profit/Spin: ${Slots.profit_per_spin}", slots_stats_profit_per_spin_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_mini_wins_text =    UIFactory.Text("SlotsTotalMiniWinsText", $"Total Mini Wins: {Slots.total_mini_wins}", slots_stats_total_mini_wins_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_small_wins_text =   UIFactory.Text("SlotsTotalSmallWinsText", $"Total Small Wins: {Slots.total_small_wins}", slots_stats_total_small_wins_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_big_wins_text =     UIFactory.Text("SlotsTotalBigWinsText", $"Total Big Wins: {Slots.total_big_wins}", slots_stats_total_big_wins_panel.transform, 20, TextAnchor.MiddleCenter);
        slots_total_jackpots_text =     UIFactory.Text("SlotsTotalJackpotsText", $"Total Jackpots: {Slots.total_jackpots}", slots_stats_total_jackpots_panel.transform, 20, TextAnchor.MiddleCenter);

        // Setting horizontal overflow mode to Overflow so that it doesn't go onto 2 lines
        slots_total_wins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_spins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_losses_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_spent_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_profit_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_win_rate_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_profit_per_spin_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_mini_wins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_small_wins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_big_wins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        slots_total_jackpots_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        
        // *************************************************** BLACKJACK ***************************************************

        // Title panel
        var blackjack_title_panel =                     UIFactory.Panel("BlackJackTitlePanel", blackjack_section.transform, Color.clear, new Vector2(0.34f, 0.8f), new Vector2(0.66f, 0.85f));
        // Stats panels
        var blackjack_stats_total_wins_panel =          UIFactory.Panel("BlackJackStatsTotalWinsPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.70f), new Vector2(0.8f, 0.70f));
        var blackjack_stats_total_pushes_panel =        UIFactory.Panel("BlackJackStatsTotalPushesPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.65f), new Vector2(0.8f, 0.65f));
        var blackjack_stats_total_plays_panel =         UIFactory.Panel("BlackJackStatsTotalPlaysPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.60f), new Vector2(0.8f, 0.60f));
        var blackjack_stats_total_losses_panel =        UIFactory.Panel("BlackJackStatsTotalLossesPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.55f), new Vector2(0.8f, 0.55f));
        var blackjack_stats_total_spent_panel =         UIFactory.Panel("BlackJackStatsTotalSpentPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.50f), new Vector2(0.8f, 0.50f));
        var blackjack_stats_total_profit_panel =        UIFactory.Panel("BlackJackStatsTotalProfitPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.45f), new Vector2(0.8f, 0.45f));
        var blackjack_stats_win_rate_panel =            UIFactory.Panel("BlackJackStatsWinRatePanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.40f), new Vector2(0.8f, 0.40f));
        var blackjack_stats_profit_per_play_panel =     UIFactory.Panel("BlackJackStatsProfitPerPlayPanel", blackjack_section.transform, Color.clear, new Vector2(0.2f, 0.35f), new Vector2(0.8f, 0.35f));

        // Setting text for the stats panels
        blackjack_total_wins_text =         UIFactory.Text("BlackJackTotalWinsText", $"Total Wins: {BlackJack.total_wins}", blackjack_stats_total_wins_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_total_pushes_text =       UIFactory.Text("BlackJackTotalPushesText", $"Total Pushes: {BlackJack.total_pushes}", blackjack_stats_total_pushes_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_total_plays_text =        UIFactory.Text("BlackJackTotalPlaysText", $"Total Plays: {BlackJack.total_plays}", blackjack_stats_total_plays_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_total_losses_text =       UIFactory.Text("BlackJackTotalLossesText", $"Total Losses: {BlackJack.total_losses}", blackjack_stats_total_losses_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_total_spent_text =        UIFactory.Text("BlackJackTotalSpentText", $"Total Spent: {BlackJack.total_spent}", blackjack_stats_total_spent_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_total_profit_text =       UIFactory.Text("BlackJackTotalProfitText", $"Total Profit: {BlackJack.total_profit}", blackjack_stats_total_profit_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_win_rate_text =           UIFactory.Text("BlackJackWinRateText", $"Win Rate: {BlackJack.win_rate}%", blackjack_stats_win_rate_panel.transform, 20, TextAnchor.MiddleCenter);
        blackjack_profit_per_play_text =    UIFactory.Text("BlackJackProfitPerPlayText", $"Average Profit/Play: ${BlackJack.profit_per_play}", blackjack_stats_profit_per_play_panel.transform, 20, TextAnchor.MiddleCenter);

        // Setting horizontal overflow mode to Overflow so that it doesn't go onto 2 lines
        blackjack_total_wins_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_total_pushes_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_total_plays_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_total_losses_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_total_spent_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_total_profit_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_win_rate_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        blackjack_profit_per_play_text.horizontalOverflow = HorizontalWrapMode.Overflow;

        // *************************************************** RTB ***************************************************

        // Title panel
        var rtb_title_panel =                   UIFactory.Panel("SlotsTitlePanel", rtb_section.transform, Color.clear, new Vector2(0.34f, 0.8f), new Vector2(0.66f, 0.85f));
        // Stats panels
        var rtb_stats_total_plays_panel =       UIFactory.Panel("SlotsStatsTotalSpinsPanel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.70f), new Vector2(0.8f, 0.70f));
        var rtb_stats_total_spent_panel =       UIFactory.Panel("SlotsStatsTotalSpentPanel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.65f), new Vector2(0.8f, 0.65f));
        var rtb_stats_total_profit_panel =      UIFactory.Panel("SlotsStatsTotalProfitPanel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.60f), new Vector2(0.8f, 0.60f));
        var rtb_stats_profit_per_play_panel =   UIFactory.Panel("SlotsStatsProfitPerSpinPanel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.55f), new Vector2(0.8f, 0.55f));
        var rtb_stats_times_at_r1_panel = UIFactory.Panel("SlotsStatsTimesAtR1Panel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.50f), new Vector2(0.8f, 0.50f));
        var rtb_stats_times_at_r2_panel =       UIFactory.Panel("SlotsStatsTimesAtR2Panel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.45f), new Vector2(0.8f, 0.45f));
        var rtb_stats_times_at_r3_panel =       UIFactory.Panel("SlotsStatsTimesAtR3Panel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.40f), new Vector2(0.8f, 0.40f));
        var rtb_stats_times_at_r4_panel =       UIFactory.Panel("SlotsStatsTimesAtR4Panel", rtb_section.transform, Color.clear, new Vector2(0.2f, 0.35f), new Vector2(0.8f, 0.35f));

        // Setting text for the stats panels
        rtb_total_plays_text =          UIFactory.Text("TotalSpinsText", $"Total Spins: {RideTheBus.total_plays}", rtb_stats_total_plays_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_total_spent_text =          UIFactory.Text("TotalSpentText", $"Total Spent: {RideTheBus.total_spent}", rtb_stats_total_spent_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_total_profit_text =         UIFactory.Text("TotalProfitText", $"Total Profit: {RideTheBus.total_profit}", rtb_stats_total_profit_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_profit_per_play_text =      UIFactory.Text("ProfitPerSpinText", $"Average Profit/Play: ${RideTheBus.profit_per_play}", rtb_stats_profit_per_play_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_times_at_r1_text =          UIFactory.Text("TimesAtR1Text", $"Times Finished at R1: {RideTheBus.times_finished_at_r1}", rtb_stats_times_at_r1_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_times_at_r2_text =          UIFactory.Text("TimesAtR2Text", $"Times Finished at R2: {RideTheBus.times_finished_at_r2}", rtb_stats_times_at_r2_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_times_at_r3_text =          UIFactory.Text("TimesAtR3Text", $"Times Finished at R3: {RideTheBus.times_finished_at_r3}", rtb_stats_times_at_r3_panel.transform, 20, TextAnchor.MiddleCenter);
        rtb_times_at_r4_text =          UIFactory.Text("TimesAtR4Text", $"Times Finished at R4: {RideTheBus.times_finished_at_r4}", rtb_stats_times_at_r4_panel.transform, 20, TextAnchor.MiddleCenter);

        // Setting horizontal overflow mode to Overflow so that it doesn't go onto 2 lines
        rtb_total_plays_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_total_spent_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_total_profit_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_profit_per_play_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_times_at_r1_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_times_at_r2_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_times_at_r3_text.horizontalOverflow = HorizontalWrapMode.Overflow;
        rtb_times_at_r4_text.horizontalOverflow = HorizontalWrapMode.Overflow;

        var slots_title =       UIFactory.Text("SlotsTitle", "Slots", slots_title_panel.transform, 26, TextAnchor.MiddleCenter);
        var blackjack_title =   UIFactory.Text("BlackjackTitle", "Blackjack", blackjack_title_panel.transform, 23, TextAnchor.MiddleCenter);
        var rtb_title =         UIFactory.Text("RTBTitle", "Ride The Bus", rtb_title_panel.transform, 26, TextAnchor.MiddleCenter);
    }

    public static void UpdateSlotsStatsText()
    {
        slots_total_wins_text.text = $"Total Wins: {Slots.total_wins}";
        slots_total_spins_text.text = $"Total Spins: {Slots.total_spins}";
        slots_total_losses_text.text = $"Total Losses: {Slots.total_losses}";
        slots_total_spent_text.text = $"Total Spent: {Slots.total_spent}";
        slots_total_profit_text.text = $"Total Profit: {Slots.total_profit}";
        slots_win_rate_text.text = $"Win Rate: {Slots.win_rate}%";
        slots_profit_per_spin_text.text = $"Average Profit/Spin: ${Slots.profit_per_spin}";
        slots_total_mini_wins_text.text = $"Total Mini Wins: {Slots.total_mini_wins}";
        slots_total_small_wins_text.text = $"Total Small Wins: {Slots.total_small_wins}";
        slots_total_big_wins_text.text = $"Total Big Wins: {Slots.total_big_wins}";
        slots_total_jackpots_text.text = $"Total Jackpots: {Slots.total_jackpots}";
    }

    public static void UpdateBlackJackStatsText()
    {
        blackjack_total_wins_text.text = $"Total Wins: {BlackJack.total_wins}";
        blackjack_total_plays_text.text = $"Total Plays: {BlackJack.total_plays}";
        blackjack_total_losses_text.text = $"Total Losses: {BlackJack.total_losses}";
        blackjack_total_spent_text.text = $"Total Spent: {BlackJack.total_spent}";
        blackjack_total_profit_text.text = $"Total Profit: {BlackJack.total_profit}";
        blackjack_win_rate_text.text = $"Win Rate: {BlackJack.win_rate}%";
        blackjack_profit_per_play_text.text = $"Average Profit/Play: ${BlackJack.profit_per_play}";
    }

    public static void UpdateRTBStatsText()
    {
        rtb_total_plays_text.text = $"Total Plays: {RideTheBus.total_plays}";
        rtb_total_spent_text.text = $"Total Spent: {RideTheBus.total_spent}";
        rtb_total_profit_text.text = $"Total Profit: {RideTheBus.total_profit}";
        rtb_profit_per_play_text.text = $"Average Profit/Play: ${RideTheBus.profit_per_play}";
        rtb_times_at_r1_text.text = $"Times Finished at R1: {RideTheBus.times_finished_at_r1}";
        rtb_times_at_r2_text.text = $"Times Finished at R2: {RideTheBus.times_finished_at_r2}";
        rtb_times_at_r3_text.text = $"Times Finished at R3: {RideTheBus.times_finished_at_r3}";
        rtb_times_at_r4_text.text = $"Times Finished at R4: {RideTheBus.times_finished_at_r4}";
    }
}