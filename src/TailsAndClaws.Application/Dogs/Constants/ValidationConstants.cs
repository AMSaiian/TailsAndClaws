namespace TailsAndClaws.Application.Dogs.Constants;

public static class ValidationConstants
{
    public static readonly int NameLength = 255;
    public static readonly int ColorLength = 400;

    public static readonly (int Precision, int Scale) WeightPrecision = (8, 3);
    public static readonly (int Precision, int Scale) LengthPrecision = (6, 3);

    public static readonly string NameRegex ;
}
