using System;

namespace FHICORC.Enums
{
    [Flags]
    public enum FlashlightState
    {
        Default = 0,
        On = 1,
        Off = 2,
        Disabled = 4,
        DisabledAndOn = 5,
        DisabledAndOff = 6,
        Enabled = 8,
        EnabledAndOn = 9,
        EnabledAndOff = 10
    }
}
