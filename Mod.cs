using System;
using System.Collections.Generic;
using System.Linq;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;

namespace ThreeHeartDancePartner {
    public class Mod : StardewModdingAPI.Mod {
        private ModConfig Config;
        public override void Entry(IModHelper helper) {
            GameEvents.UpdateTick += onUpdate;
            this.Config = this.Helper.ReadConfig<ModConfig>();
        }

        private Dictionary<String, String> RejectDialog = new Dictionary<string, string>();

        private void onUpdate(object sender, EventArgs args) {
            if (Game1.currentLocation == null || Game1.currentLocation.Name != "Temp" || Game1.currentLocation.currentEvent == null)
                return;
            Event @event = Game1.currentLocation.currentEvent;

            if (!@event.FestivalName.Equals("Flower Dance"))
                return;

            foreach (NPC npc in @event.actors) {
                if (!npc.datable.Value || npc.HasPartnerForDance) continue;
                try {
                    if (npc.CurrentDialogue.Count() <= 0) continue;
                    String reject;
                    if (!RejectDialog.TryGetValue(npc.Name, out reject)) {
                        reject = new Dialogue(Game1.content.Load<Dictionary<string, string>>("Characters\\Dialogue\\" + npc.Name)["danceRejection"], npc).getCurrentDialogue();
                        Monitor.Log(reject);
                        RejectDialog[npc.Name] = reject;
                    }
                    String curr = npc.CurrentDialogue.Peek().getCurrentDialogue();

                    if (curr == reject) {
                        Monitor.Log("ASDF");
                        Monitor.Log(npc.Name + " " + npc.HasPartnerForDance + " " + (Game1.player.getFriendshipLevelForNPC(npc.Name) >= Config.NumberOfHeartsRequiredForAcceptance * 250));

                        // The original stuff, only the relationship point check is modified. (1000 -> 750)
                        if (!npc.HasPartnerForDance && Game1.player.getFriendshipLevelForNPC(npc.Name) >= Config.NumberOfHeartsRequiredForAcceptance * 250) {
                            Monitor.Log("AEEF");
                            string s = "";
                            switch (npc.Gender) {
                            case 0:
                                s = "You want to be my partner for the flower dance?#$b#Okay. I look forward to it.$h";
                                break;
                            case 1:
                                s = "You want to be my partner for the flower dance?#$b#Okay! I'd love to.$h";
                                break;
                            }

                            try {
                                Game1.player.changeFriendship(250, npc);
                            }
                            catch (Exception) {
                            }
                            Game1.player.dancePartner.Value = npc;
                            npc.setNewDialogue(s, false, false);

                            // Okay, looks like I need to fix the current dialog box
                            Game1.activeClickableMenu = new DialogueBox(new Dialogue(s, npc) { removeOnNextMove = false });
                        }
                    }
                }
                catch (Exception e) {
                    this.Monitor.Log("Exception: " + e, LogLevel.Error);
                    continue;
                }
            }
        }
    }
}
