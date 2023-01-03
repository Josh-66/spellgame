
public enum GlyphCategory{
    Invalid,
    Element,
    Form,
    Strength,
    Style
}
public enum GlyphType{
    Invalid,

    //Elements
    Fire,
    Air,
    Water,
    Electricity,
    Earth,
    Nature,
    Blood,
    Gag,

    //Form
    Ball,
    Spray,
    Strike,
    Hands,
    Explosion,
    Cleanse,
    Sides,
    Up,
    Down,

    //Strength
    Low,
    High,
    Ludicrous,

    //Style
    Sparkles,
    Strobe,
    Glowing,


}
public enum GlyphTypeElement{
    Fire = GlyphType.Fire,
    Air = GlyphType.Air,
    Water = GlyphType.Water,
    Electricity = GlyphType.Electricity,
    Earth = GlyphType.Earth,
    Nature = GlyphType.Nature,
    Blood = GlyphType.Blood,
    Gag = GlyphType.Gag,
    Invalid = GlyphType.Invalid,
}
public enum GlyphTypeForm{
    Ball = GlyphType.Ball,
    Spray= GlyphType.Spray,
    Strike= GlyphType.Strike,
    Hands= GlyphType.Hands,
    Explosion= GlyphType.Explosion,
    Cleanse= GlyphType.Cleanse,
    Sides= GlyphType.Sides,
    Up = GlyphType.Up,
    Down = GlyphType.Down,
    Invalid = GlyphType.Invalid,
}
public enum GlyphTypeStrength{
    Low= GlyphType.Low,
    High= GlyphType.High,
    Ludicrous= GlyphType.Ludicrous,
    Invalid = GlyphType.Invalid,
}
public enum GlyphTypeStyle{
    Sparkles = GlyphType.Sparkles,
    Strobe = GlyphType.Strobe,
    Glowing = GlyphType.Glowing,
    Invalid = GlyphType.Invalid,
}