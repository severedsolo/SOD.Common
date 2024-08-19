using System.Collections.Generic;
using SOD.Common.Extensions;

namespace SOD.Common.Helpers;

public sealed class SocialCredit
{
    public void AddSocialCredit(int amount, bool showMessage = true, string reason = "")
    {
        GameplayController.Instance.AddSocialCredit(amount, showMessage, reason);
        if (amount >= 0) return;
        AdjustPerksToLevel();
    }
    
    private void AdjustPerksToLevel()
    {
        //Remove additional perks the player has accrued
        //We start at Lv1 and 0 perks, so minus 1 when considering amount of perks to remove.
        int buffsPlayerShouldHave = GameplayController.Instance.GetCurrentSocialCreditLevel() - 1;
        List<SocialControls.SocialCreditBuff> perks = GameplayController.Instance.socialCreditPerks.ToList();
        for (int i = GameplayController.Instance.socialCreditPerks.Count - 1; i >= buffsPlayerShouldHave; i--)
        {
            SocialControls.SocialCreditBuff buff = perks[i];
            if (buff == null) continue;
            GameplayController.Instance.socialCreditPerks.Remove(buff);
            //Broadcast removal to mimic base game behaviour when a perk is added
            Lib.GameMessage.Broadcast("Removed: "+buff.description, InterfaceController.GameMessageType.notification, InterfaceControls.Icon.star);
        }
    }
}