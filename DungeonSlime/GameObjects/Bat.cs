using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Shapes;

namespace DungeonSlime.GameObjects;

public class Bat
{
    #region Constants

    private const float MovementSpeed = 5.0f;

    #endregion

    #region Fields

    private Vector2 velocity;
    private AnimatedSprite sprite;
    private SoundEffect bounceSoundEffect;

    #endregion

    #region Properties

    public Vector2 Position { get; set; }

    #endregion

    #region Constructor

    public Bat(AnimatedSprite sprite, SoundEffect bounceSoundEffect)
    {
        this.sprite = sprite;
        this.bounceSoundEffect = bounceSoundEffect;
    }

    #endregion

    #region Public Methods

    public void RandomizeVelocity()
    {
        float angle = (float)(Random.Shared.NextDouble() * MathHelper.TwoPi);
        // Convert the angle into a direction vector
        float x = (float)(Math.Cos(angle));
        float y = (float)(Math.Sin(angle));
        Vector2 directionVector = new Vector2(x, y);
        //Multiply the directionVector by the movement speed to a velocity
        velocity = directionVector * MovementSpeed;
    }

    public void Bounce(Vector2 normalVector)
    {
        Vector2 newPosition = Position;
        // Adjust the position based on the normal to prevent sticking to walls
        if (normalVector.X != 0)
        {
            // Move slightly away from the wall direction of the normal
            newPosition.X += normalVector.X * (sprite.Width * 0.1f);
        }

        if (normalVector.Y != 0)
        {
            // Move slightly away from the wall direction of the normal
            newPosition.Y = normalVector.Y * (sprite.Height * .1f);
        }

        Position = newPosition;
        normalVector.Normalize();
        velocity = Vector2.Reflect(velocity, normalVector);
        Core.Audio.PlaySoundEffect(bounceSoundEffect);
    }

    public Circle GetBounds()
    {
        int x = (int)(Position.X + sprite.Width * .5f);
        int y = (int)(Position.Y + sprite.Height * .5f);
        int radius = (int)(sprite.Width * .25f);
        return new Circle(x, y, radius);
    }

    // The continuous movement of the bat contrasts with the grid-based interval movement of the slime, creating different gameplay dynamics for the player to consider.  This makes catching the bat challenging without requiring any complex behaviors.
    public void Update(GameTime gameTime)
    {
        sprite.Update(gameTime);
        Position += velocity;
    }

    public void Draw()
    {
        sprite.Draw(Core.SpriteBatch, Position);
    }

    #endregion
}
