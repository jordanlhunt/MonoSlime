using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class Animation
{
    #region Properties
    public List<TextureRegion> Frames { get; set; }
    public TimeSpan TimeBetweenFrames { get; set; }
    #endregion
    #region Constructors
    public Animation()
    {
        Frames = new List<TextureRegion>();
        TimeBetweenFrames = TimeSpan.FromMilliseconds(100);
    }

    public Animation(List<TextureRegion> frames, TimeSpan timeBetweenFrames)
    {
        this.Frames = frames;
        this.TimeBetweenFrames = timeBetweenFrames;
    }
    #endregion
}
