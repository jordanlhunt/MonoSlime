using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using MonoGameLibrary;
using MonoGameLibrary.Graphics;
using MonoGameLibrary.Shapes;

namespace DungeonSlime.GameObjects;

public class Slime
{
    #region Constants

    // A constant value that represents the amount of time to way between movement updates
    private static readonly TimeSpan MovementTime = TimeSpan.FromMilliseconds(200);
    private const int MAX_BUFFER_SIZE = 2;
    #endregion

    #region Fields

    // The Amount of time that has elapsed since the last movement update
    private TimeSpan movementTicks;

    // Normalized value (0-1) representing the progress between ticks for visual interpolation
    private float movementProgress;

    // The next direction to apply the head of the slime chain during the next movement update
    private float movementDistance;

    // The number of pixels to move the head during the movement cycle
    private Vector2 nextMovmementDirection;

    // Tracks the segments of the slime chain
    private List<SlimeSegment> slimeSegments;

    // The animatedSprite used when drawing each slime segment
    private AnimatedSprite slimeSprite;

    private Queue<Vector2> _inputBuffer;

    #endregion

    #region Events

    /// <summary>
    /// Event that is raised if it is detected if the head segment collides with the slimeSegemnts chain
    /// </summary>
    public event EventHandler BodyCollision;

    #endregion

    #region Constructor

    /// <summary>
    /// Creates a new slime using the correct
    /// </summary>
    /// <param name="animatedSprite"> The slime from the spriteSheet to draw</param>
    public Slime(AnimatedSprite animatedSprite)
    {
        slimeSprite = animatedSprite;
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Initialize the slime, can be used to reset to its initial state
    /// </summary>
    /// <param name="startingPosition"> The position the slime will start</param>
    /// <param name="movementDistance"> The number of pixels to move the head segment during each movement tick</param>
    public void Initialize(Vector2 startingPosition, float movementDistance)
    {
        // Initialize the segment list
        slimeSegments = new List<SlimeSegment>();
        this.movementDistance = movementDistance;
        SlimeSegment head = new SlimeSegment();
        head.CurrentPosition = startingPosition;
        head.TargetPosition = startingPosition + new Vector2(movementDistance, 0);
        head.Direction = Vector2.UnitX;
        // Add the head to the slimeSegments
        slimeSegments.Add(head);
        // Set the initial next direction as the same direction the head is moving
        nextMovmementDirection = head.Direction;
        // Zero out the movement time
        movementTicks = TimeSpan.Zero;
    }

    public void HandleInput()
    {
        Vector2 newDirection = this.nextMovmementDirection;
        if (GameController.MoveDown())
        {
            newDirection = Vector2.UnitY;
        }
        else if (GameController.MoveUp())
        {
            newDirection = -Vector2.UnitY;
        }
        else if (GameController.MoveLeft())
        {
            newDirection = -Vector2.UnitX;
        }
        else if (GameController.MoveRight())
        {
            newDirection = Vector2.UnitX;
        }

        // Only Direction change IF and ONLY IF it is not reversing the current direction. This prevents the slime from backing into itself.
        float dotProduct = Vector2.Dot(newDirection, slimeSegments[0].Direction);
        if (dotProduct >= 0)
        {
            nextMovmementDirection = newDirection;
        }
    }

    /// <summary>
    /// Grows the slime chain by one slimeSegement
    /// </summary>
    public void Grow()
    {
        SlimeSegment tail = slimeSegments[slimeSegments.Count - 1];
        // Create a new tail segment that is positioned a grid cell in the reverse direction to the tail
        SlimeSegment newAdditionToTail = new SlimeSegment();
        newAdditionToTail.CurrentPosition =
            tail.TargetPosition + tail.ReverseDirection * movementDistance;
        newAdditionToTail.TargetPosition = tail.CurrentPosition;
        newAdditionToTail.Direction = Vector2.Normalize(
            tail.CurrentPosition - newAdditionToTail.CurrentPosition
        );
        slimeSegments.Add(newAdditionToTail);
    }

    public void Update(GameTime gameTime)
    {
        slimeSprite.Update(gameTime);
        HandleInput();
        movementTicks += gameTime.ElapsedGameTime;
        // If the movement timeSinceLastMovementUpdate has accumulated enough time to be greater than the threshold, perform a full movement
        if (movementTicks >= MovementTime)
        {
            movementTicks -= MovementTime;
            Move();
        }

        movementProgress = (float)(movementTicks.TotalSeconds / MovementTime.TotalSeconds);
    }

    public void Draw()
    {
        foreach (SlimeSegment slimeSegment in slimeSegments)
        {
            // Calculate the visual position of the segment at the moment by lerping between its "currentPosition" and "targetPosition" by the movement offset lerp amount
            Vector2 position = Vector2.Lerp(
                slimeSegment.CurrentPosition,
                slimeSegment.TargetPosition,
                movementProgress
            );
            slimeSprite.Draw(Core.SpriteBatch, position);
        }
    }

    /// <summary>
    /// Returns a Circle value that represents collision bounds of the slime.
    /// </summary>
    /// <returns>A Circle value.</returns>
    public Circle GetBounds()
    {
        SlimeSegment head = slimeSegments[0];

        // Calculate the visual position of the segment at the moment by lerping between its "currentPosition" and "targetPosition" by the movement offset lerp amount
        Vector2 position = Vector2.Lerp(
            head.CurrentPosition,
            head.TargetPosition,
            movementProgress
        );
        // Create the bounds using the calculated visual position of the head
        Circle bounds = new Circle(
            (int)(position.X + (slimeSprite.Width * 0.5f)),
            (int)(position.Y + (slimeSprite.Height * 0.5f)),
            (int)(slimeSprite.Width * 0.5f)
        );
        return bounds;
    }

    #endregion

    #region Private Methods

    private void Move()
    {
        SlimeSegment head = slimeSegments[0];
        // Update the direction the head is supposed to move in to the next direction caches
        head.Direction = nextMovmementDirection;
        // Update the head's current position to its targetPosition
        head.CurrentPosition = head.TargetPosition;
        // Update the head's target position to the next tile in the direction it's moving
        head.TargetPosition = head.TargetPosition + head.Direction * movementDistance;
        // Inset the new adjust value for the head at the front of the line of Segment and remove the tail segments. This effectively moves the entire chain forward without needing to loop through every segment and update its "CurrentPosition" and "TargetPosition"
        slimeSegments.Insert(0, head);
        slimeSegments.RemoveAt(slimeSegments.Count - 1);
        // Iterate through all the segments except the head and check and check if they are at the same position as the head. if they are then the head is colliding and a body collision has occurred.
        for (int i = 1; i < slimeSegments.Count; i++)
        {
            SlimeSegment someSlimeSegment = slimeSegments[i];
            if (someSlimeSegment.CurrentPosition == head.CurrentPosition)
            {
                if (BodyCollision != null)
                {
                    BodyCollision.Invoke(this, EventArgs.Empty);
                }
            }

            return;
        }
    }

    #endregion
}
