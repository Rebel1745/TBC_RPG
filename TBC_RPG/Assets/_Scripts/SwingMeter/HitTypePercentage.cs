[System.Serializable]
public class HitTypePercentage {
    public HIT_TYPE HitType;
    public float Percentage;

    public HitTypePercentage(HIT_TYPE hitType, float percentage)
    {
        HitType = hitType;
        Percentage = percentage;
    }
}
