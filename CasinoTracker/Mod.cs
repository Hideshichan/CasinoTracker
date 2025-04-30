using MelonLoader;

[assembly: MelonInfo(typeof(CasinoTracker.MyMod), "CasinoTracker", "1.0.0", "Hideshi", null)]
[assembly: MelonGame("TVGS", "Schedule I")]



namespace CasinoTracker
{
    public class MyMod : MelonMod
    {
        public override void OnInitializeMelon()
        {
            MelonLogger.Msg("Initialized CasinoTracker.");
            // Load the stats from the JSON files
            MelonLogger.Msg("Attempting to load stats from JSON files");
            RideTheBusStats.LoadToFields();
            BlackjackStats.LoadToFields();
            SlotStats.LoadToFields();
        }

        public override void OnDeinitializeMelon()
        {
            MelonLogger.Msg("Deinitialized CasinoTracker.");
        }
    }
}