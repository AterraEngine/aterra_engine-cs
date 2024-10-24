﻿// ---------------------------------------------------------------------------------------------------------------------
// Imports
// ---------------------------------------------------------------------------------------------------------------------
using AterraCore.Contracts.OmniVault.Textures;
using Raylib_cs;
using System.Numerics;

namespace AterraCore.AssetVault.Textures;
// ---------------------------------------------------------------------------------------------------------------------
// Code
// ---------------------------------------------------------------------------------------------------------------------
public abstract class AbstractTexture2DAsset : AssetInstance, ITexture2DAsset {
    private Texture2D? _texture;
    private bool _textureDefined;
    public abstract string ImagePath { get; set; }
    public abstract Vector2 Size { get; set; }

    public bool TryGetTexture(out Texture2D texture) {
        texture = _texture ?? new Texture2D();
        return _textureDefined;
    }

    public bool TrySetTexture(Texture2D texture) {
        _texture = texture;
        _textureDefined = true;

        return _textureDefined;
    }

    public bool TryUnSetTexture(out Texture2D texture) {
        texture = _texture ?? new Texture2D();
        _texture = new Texture2D();
        _textureDefined = false;

        return true;
    }

    public Texture2D GetTexture() => _texture ?? new Texture2D();
}
