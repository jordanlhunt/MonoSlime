using System;
using System.Collections.Generic;

namespace MonoGameLibrary.Graphics;

public class Animation
{
    #region Properties

    public List<TextureRegion> Frames { get; set; }
    public TimeSpan Delay { get; set; }

    #endregion

    #region Constructors

    public Animation()
    {
        Frames = new List<TextureRegion>();
        Delay = TimeSpan.FromMilliseconds(100);
    }

    public Animation(List<TextureRegion> frames, TimeSpan delay)
    {
        this.Frames = frames;
        this.Delay = delay;
    }

    #endregion
}
