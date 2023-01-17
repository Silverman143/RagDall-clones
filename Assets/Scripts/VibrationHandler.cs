using MoreMountains.NiceVibrations;

public static class VibrationHandler 
{
    public static void Hit()
    {
        MMVibrationManager.Haptic(HapticTypes.SoftImpact);
    }

    public static void Spikes()
    {
        MMVibrationManager.TransientHaptic(0.56f, 0.4f);
    }
}
