// <copyright file="IAnchorData.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
namespace SKUwpSample.NET;

using System;
using StereoKit;

/// <summary>
/// Represents data associated with a detected anchor in the environment.
/// </summary>
public interface IAnchorData
{
    /// <summary>
    /// Gets the reference to the spatial anchor in the environment.
    /// </summary>
    Anchor Anchor { get; }

    /// <summary>
    /// Gets the metadata or custom data associated with the anchor.
    /// </summary>
    string Data { get; }

    /// <summary>
    /// Gets the physical size or dimensions of the anchor.
    /// </summary>
    Vec3 Size { get; }

    /// <summary>
    /// Gets the timestamp representing the last time this anchor was detected in the environment.
    /// </summary>
    DateTimeOffset LastDetectedTime { get; }
}
