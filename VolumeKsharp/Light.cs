﻿// <copyright file="Light.cs" company="LeonardoTassinari">
// Copyright (c) LeonardoTassinari. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace VolumeKsharp;

// ReSharper disable NonReadonlyMemberInGetHashCode
using System;
using System.Collections.Generic;

/// <summary>
/// Class to represent a rgbw light.
/// </summary>
public class Light : IEquatable<Light>
{
    /// <summary>
    /// The max value of the colors.
    /// </summary>
    private readonly int maxValue = 255;

    private readonly Controller controller;

    /// <summary>
    /// Initializes a new instance of the <see cref="Light"/> class.
    /// </summary>
    /// <param name="controller">The calling controller.</param>
    public Light(Controller controller)
        : this(0, 0, 0, 0, controller)
    {
        this.W = this.maxValue;
        this.Brightness = this.maxValue;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Light"/> class.
    /// </summary>
    /// <param name="parentLight">The light to copy.</param>
    /// <param name="controller">The calling controller.</param>
    public Light(Light parentLight, Controller controller)
        : this(parentLight.R, parentLight.G, parentLight.B, parentLight.W, controller)
    {
        this.State = parentLight.State;
        this.Brightness = parentLight.Brightness;
    }

    private Light(int r, int g, int b, int w, Controller controller)
    {
        this.R = r;
        this.G = g;
        this.B = b;
        this.W = w;
        this.controller = controller;
        this.State = false;
        this.Brightness = this.maxValue;
    }

    /// <summary>
    /// Gets the list of effects.
    /// </summary>
    public ISet<string> EffectsSet { get; } = new HashSet<string>(
        new[] { "Solid", "Rainbow", "Breath", "colorfade_slow", "colorfade_fast", "flash" });

    /// <summary>
    /// Gets or sets the active effect.
    /// </summary>
    public string? ActiveEffect { get; set; }

    /// <summary>
    /// Gets or sets red value.
    /// </summary>
    public int R { get; set; }

    /// <summary>
    /// Gets or sets the green value.
    /// </summary>
    public int G { get; set; }

    /// <summary>
    /// Gets or sets the blue value.
    /// </summary>
    public int B { get; set; }

    /// <summary>
    /// Gets or sets the white value.
    /// </summary>
    public int W { get; set; }

    /// <summary>
    /// Gets or sets the brightness value.
    /// </summary>
    public int Brightness { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the on off state.
    /// </summary>
    public bool State { get; set; }

    /// <summary>
    /// Equals operator override.
    /// </summary>
    /// <param name="left">The left light.</param>
    /// <param name="right">The right light.</param>
    /// <returns>If they are equal.</returns>
    public static bool operator ==(Light? left, Light? right)
    {
        return Equals(left, right);
    }

    /// <summary>
    /// Different operator override.
    /// </summary>
    /// <param name="left">The left light.</param>
    /// <param name="right">The right light.</param>
    /// <returns>If they are different.</returns>
    public static bool operator !=(Light? left, Light? right)
    {
        return !Equals(left, right);
    }

    /// <summary>
    /// Method to update the physical light with the current parameters.
    /// </summary>
    public void UpdateLight()
    {
        if (this.State)
        {
            this.controller.Serialcom.AddCommand(new SolidAppearanceCommand(
                this.R * this.Brightness / this.maxValue,
                this.G * this.Brightness / this.maxValue,
                this.B * this.Brightness / this.maxValue,
                this.W * this.Brightness / this.maxValue));
        }
        else
        {
            this.controller.Serialcom.AddCommand(new SolidAppearanceCommand(0, 0, 0, 0));
        }
    }

    /// <inheritdoc/>
    public bool Equals(Light? other)
    {
        if (ReferenceEquals(null, other))
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        return this.maxValue == other.maxValue && this.EffectsSet.SetEquals(other.EffectsSet) && this.ActiveEffect == other.ActiveEffect && this.R == other.R && this.G == other.G && this.B == other.B && this.W == other.W && this.Brightness == other.Brightness && this.State == other.State;
    }

    /// <inheritdoc />
    public override bool Equals(object? obj)
    {
        return ReferenceEquals(this, obj) || (obj is Light other && this.Equals(other));
    }

    /// <inheritdoc />
    public override int GetHashCode()
    {
        var hashCode = default(HashCode);
        hashCode.Add(this.EffectsSet);
        hashCode.Add(this.ActiveEffect);
        hashCode.Add(this.maxValue);
        hashCode.Add(this.G);
        hashCode.Add(this.R);
        hashCode.Add(this.B);
        hashCode.Add(this.W);
        hashCode.Add(this.Brightness);
        hashCode.Add(this.State);
        return hashCode.ToHashCode();
    }
}