using System;

namespace Creek.UI.EFML.UI_Elements
{
    public class FlashElement : ObjectElement
    {
        public FlashElement()
        {
            Type = Type.GetTypeFromProgID("ShockwaveFlash.ShockwaveFlash");
        }
    }
}