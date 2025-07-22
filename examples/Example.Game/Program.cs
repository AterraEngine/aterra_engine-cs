// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using Raylib_cs;
using AterraEngine;
using AterraEngine.Frameworks.Nexities;
using AterraEngine.Frameworks.Nexities.Variants;

namespace Example.Game;
// -----------------------------------------------------------------------------------------------------------------
// Methods
// -----------------------------------------------------------------------------------------------------------------
public static class Program {
    public static NexitiesEntity[] Entities { get; } = new NexitiesEntity[EntityCountI*EntityCountJ];
    
    public static RenderEntitySystem RenderSystem { get; } = new();
    public static MoveEntitySystem MoveSystem { get; } = new();
    
    public static Camera2D Camera { get; } = new() {
        Target = new Vector2(0, 0),
        Offset = new Vector2(400, 200), // Center of screen (half of 800x400)
        Rotation = 0.0f,
        Zoom = 7.5f
    };

    private const int EntityCountI = 20;
    private const int EntityCountJ = 50;
    
    // -----------------------------------------------------------------------------------------------------------------
    // Methods
    // -----------------------------------------------------------------------------------------------------------------
    public static async Task Main(string[] args) {
        using Engine engine =  Engine.Initialize();
        
        engine.SetupWindow();
        Raylib.SetWindowMonitor(2);
        Texture2D texture = Raylib.LoadTexture("Assets/RubberDuck-Chaos-256.png");
        RenderSystem.TextureLookup.Add("duck", texture);
        
        var index = 0;
        for (int i = -(EntityCountI/2); i < EntityCountI/2; i++) {
            for (int j = -(EntityCountJ/2); j < EntityCountJ/2; j++) {
                var entity = new BasicEntity {
                    Sprite = {
                        TextureId = "duck"
                    },
                    Transform = {
                        Position = new Vector2(i, j),
                        Rotation = Random.Shared.NextSingle() * 360f,
                    }
                };
                Entities[index++] = entity;   
            }
        }
        
        engine.Run(Update);
    }

    private static void Update() {
        
        ImGui.Begin("Main UI");
        ImGui.BeginChild("SmallBox");

        ImGui.Text($"FPS: {Raylib.GetFPS()}");
        ImGui.Text($"Entites: {EntityCountI * EntityCountJ}");
        ImGui.Text($"batched draw calls: {1 + EntityCountI * EntityCountJ/Rlgl.DEFAULT_BATCH_BUFFER_ELEMENTS}");

        ImGui.EndChild();
        ImGui.End();

        Raylib.BeginMode2D(Camera);
        
        var entitySpan = MemoryMarshal.CreateSpan(ref Unsafe.As<NexitiesEntity, BasicEntity>(ref Entities[0]), Entities.Length);
        
        foreach (BasicEntity entity in entitySpan) {
            MoveSystem.Update(entity);
            RenderSystem.Update(entity);
        }
        
        Raylib.EndMode2D();
    }
}