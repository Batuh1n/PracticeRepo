namespace ConsoleApp1;

public record Sword2(Material SwordMaterial, Gemstone SwordGemstone, int Length, int Width);
public enum Material { Wood, Bronze, Iron, Steel, Binarium }
public enum Gemstone { None, Emerald, Amber, Sapphire, Diamond, Bitstone }

// Almost the same with author's solution
// https://csharpplayersguide.com/solutions/5th-edition/war-preparations
