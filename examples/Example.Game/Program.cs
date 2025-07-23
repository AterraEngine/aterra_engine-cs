// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------

using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using ImGuiNET;
using Raylib_cs;
using AterraEngine;
using AterraEngine.Contracts;
using AterraEngine.Frameworks.Nexities;
using AterraEngine.Frameworks.Nexities.Library.Entities;
using AterraEngine.Frameworks.Nexities.Library.Systems;
using AterraEngine.Frameworks.Nexities.Pools;
using Microsoft.Extensions.DependencyInjection;

namespace Example.Game;
// -----------------------------------------------------------------------------------------------------------------
// Methods
// -----------------------------------------------------------------------------------------------------------------
public static class Program {
    private static INexitiesEntity[] Entities { get; } = new INexitiesEntity[EntityCountI*EntityCountJ];
    private static Lock Lock { get; } = new();

    private static RenderEntitySystem RenderSystem { get; set; } = null!;
    private static MoveEntitySystem MoveSystem { get; set; } = null!;

    private static Camera2D Camera { get; } = new() {
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
        Engine engine =  Engine.Initialize();
        RenderSystem = engine.ServiceProvider.GetRequiredService<RenderEntitySystem>();
        MoveSystem = engine.ServiceProvider.GetRequiredService<MoveEntitySystem>();
        
        engine.SetupWindow();
        Raylib.SetWindowMonitor(2);

        var textureProvider = engine.ServiceProvider.GetRequiredService<ITextureProvider>();
        textureProvider.TryAddTexture("duck", "Assets/RubberDuck-Chaos-256.png", out _);
        AddEntities();
        
        engine.Run(Update);
    }

    private static void AddEntities() {
        var textureProvider = Engine.Instance.ServiceProvider.GetRequiredService<ITextureProvider>();
        if (!textureProvider.TryGetId("duck", out uint textureId)) return;
        
        var index = 0;
        for (int i = -(EntityCountI/2); i < EntityCountI/2; i++) {
            for (int j = -(EntityCountJ/2); j < EntityCountJ/2; j++) {
                BasicEntity basicEntity = EntityPool<BasicEntity>.Shared.Get();
                basicEntity.Sprite.TextureId = textureId;
                basicEntity.Transform.Position = new Vector2(i, j);
                basicEntity.Transform.Rotation = Random.Shared.NextSingle() * 360f;
                Entities[index++] = basicEntity;   
            }
        }
    }

    private static void RemoveEntities() {
        var index = 0;
        foreach (INexitiesEntity entity in Entities) {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalseAccordingToNullableAPIContract
            if (entity == null) continue;
            entity.ReturnToPool();
            Entities[index++] = null!;
        }
    }

    private static void Update() {
        ImGui.Begin("Main UI");
        ImGui.BeginChild("SmallBox");

        ImGui.Text($"FPS: {Raylib.GetFPS()}");
        ImGui.Text($"Entites: {EntityCountI * EntityCountJ}");
        ImGui.Text($"batched draw calls: {1 + EntityCountI * EntityCountJ/Rlgl.DEFAULT_BATCH_BUFFER_ELEMENTS}");
        if (ImGui.Button("Reset Entities")){
            lock (Lock) {
                RemoveEntities();
                AddEntities();
            }
        }

        ImGui.EndChild();
        ImGui.End();

        Raylib.BeginMode2D(Camera);
        
        var entitySpan = MemoryMarshal.CreateSpan(ref Unsafe.As<INexitiesEntity, BasicEntity>(ref Entities[0]), Entities.Length);
        float delta = Raylib.GetFrameTime();
        
        foreach (BasicEntity entity in entitySpan) {
            MoveSystem.Update(entity, delta);
            RenderSystem.Update(entity, delta);
        }
        
        Raylib.EndMode2D();
    }
}