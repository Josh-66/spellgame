
public enum GlyphCategory{
    Invalid,
    Element,
    Form,
    Strength,
    Style
}
[System.Serializable]
public enum GlyphType:byte{
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
    Purify,
    Sides,
    Up,
    Down,

    //Strength
    Low,
    Medium,
    High,
    Ludicrous,

    //Style
    Sparkles,
    Strobe,
    Glowing,


}
public enum GlyphTypeElement:byte{
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
public enum GlyphTypeForm:byte{
    Ball = GlyphType.Ball,
    Spray= GlyphType.Spray,
    Strike= GlyphType.Strike,
    Hands= GlyphType.Hands,
    Explosion= GlyphType.Explosion,
    Purify= GlyphType.Purify,
    Sides= GlyphType.Sides,
    Up = GlyphType.Up,
    Down = GlyphType.Down,
    Invalid = GlyphType.Invalid,
}
public enum GlyphTypeStrength:byte{
    Low= GlyphType.Low,
    Medium = GlyphType.Medium,
    High= GlyphType.High,
    Ludicrous= GlyphType.Ludicrous,
    Invalid = GlyphType.Invalid,
}
public enum GlyphTypeStyle:byte{
    Sparkles = GlyphType.Sparkles,
    Strobe = GlyphType.Strobe,
    Glowing = GlyphType.Glowing,
    Invalid = GlyphType.Invalid,
}