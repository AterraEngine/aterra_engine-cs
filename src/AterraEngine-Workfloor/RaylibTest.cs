using System.Numerics;
using Raylib_cs;
using static Raylib_cs.Raylib;

namespace AterraEngine_Workfloor;

public class SpriteExplosion
{
    const int _numFramesPerLine = 5;
    const int _numLines = 5;

    public void Main()
    {
        // Initialization
        //--------------------------------------------------------------------------------------
        const int screenWidth = 800;
        const int screenHeight = 450;

        InitWindow(screenWidth, screenHeight, "raylib [textures] example - sprite explosion");
        InitAudioDevice();

        // Load explosion sound
        Sound fxBoom = LoadSound("resources/audio/boom.wav");

        // Load explosion texture
        Texture2D explosion = LoadTexture("resources/explosion.png");

        // Init variables for animation

        // Sprite one frame rectangle width
        int frameWidth = explosion.Width / _numFramesPerLine;

        // Sprite one frame rectangle height
        int frameHeight = explosion.Height / _numLines;

        int currentFrame = 0;
        int currentLine = 0;

        Rectangle frameRec = new(0, 0, frameWidth, frameHeight);
        Vector2 position = new(0.0f, 0.0f);

        bool active = false;
        int framesCounter = 0;

        SetTargetFPS(240);
        //--------------------------------------------------------------------------------------

        // Main game loop
        while (!WindowShouldClose())
        {
            // Update
            //----------------------------------------------------------------------------------

            // Check for mouse button pressed and activate explosion (if not active)
            if (IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && !active)
            {
                position = GetMousePosition();
                active = true;

                position.X -= frameWidth / 2f;
                position.Y -= frameHeight / 2f;

                PlaySound(fxBoom);
            }

            // Compute explosion animation frames
            if (active)
            {
                framesCounter++;

                if (framesCounter > 2)
                {
                    currentFrame++;

                    if (currentFrame >= _numFramesPerLine)
                    {
                        currentFrame = 0;
                        currentLine++;

                        if (currentLine >= _numLines)
                        {
                            currentLine = 0;
                            active = false;
                        }
                    }

                    framesCounter = 0;
                }
            }

            frameRec.X = frameWidth * currentFrame;
            frameRec.Y = frameHeight * currentLine;
            //----------------------------------------------------------------------------------

            // Draw
            //----------------------------------------------------------------------------------
            BeginDrawing();
            ClearBackground(Color.RAYWHITE);

            // Draw explosion required frame rectangle
            if (active)
            {
                DrawTextureRec(explosion, frameRec, position, Color.WHITE);
            }

            EndDrawing();
            //----------------------------------------------------------------------------------
        }

        // De-Initialization
        //--------------------------------------------------------------------------------------
        UnloadTexture(explosion);
        UnloadSound(fxBoom);

        CloseAudioDevice();

        CloseWindow();
        //--------------------------------------------------------------------------------------
    }
}