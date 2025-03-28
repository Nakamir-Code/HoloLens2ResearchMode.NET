// <copyright file="QRData.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
namespace SKUwpSample.NET;

using System;
using StereoKit;

/// <summary>
/// Represents data scanned from a QR code.
/// </summary>
public class QRData : IAnchorData
{
	/// <inheritdoc/>
	public Anchor Anchor { get; init; }

	/// <inheritdoc/>
	public string Data { get; init; }

	/// <inheritdoc/>
	public Vec3 Size { get; init; }

	/// <inheritdoc/>
	public DateTimeOffset LastDetectedTime { get; init; }
}
