using System;
using Microsoft.Xna.Framework;

namespace MonoGameLibrary.Graphics;

public class AnimatedSprite : Sprite
{
    #region Member Variables
    private int currentFrame;
    private TimeSpan elapsedTime;
    private Animation currentAnimation;
    #endregion
    #region Properties
    public Animation Animation
    {
        get { return currentAnimation; }
        set
        {
            currentAnimation = value;
            TextureRegion = currentAnimation.Frames[0];
        }
    }
    #endregion
    #region Constructors
    public AnimatedSprite() { }

    public AnimatedSprite(Animation animation)
    {
        Animation = animation;
    }
    #endregion

    #region Public Methods
    public void Update(GameTime gameTime)
    {
        elapsedTime += gameTime.ElapsedGameTime;
        if (elapsedTime >= currentAnimation.TimeBetweenFrames)
        {
            elapsedTime -= currentAnimation.TimeBetweenFrames;
            currentFrame++;
            if (currentFrame >= currentAnimation.Frames.Count)
            {
                currentFrame = 0;
            }
            TextureRegion = currentAnimation.Frames[currentFrame];
        }
    }
    #endregion
}
