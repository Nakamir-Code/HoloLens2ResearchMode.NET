// <copyright file="ISpatialAnchor.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
namespace SKUwpSample.NET;

using System.Collections.Generic;
using System.Threading.Tasks;

/// <summary>
/// Represents an interface for managing spatial anchors.
/// </summary>
public interface ISpatialAnchor
{
	/// <summary>
	/// Gets a value indicating whether the spatial anchor system is running.
	/// </summary>
	bool IsRunning { get; }

	/// <summary>
	/// Initializes the spatial anchor system asynchronously.
	/// Should be called before using any anchor-related functionality.
	/// </summary>
	Task InitializeAsync();

	/// <summary>
	/// Shuts down the spatial anchor system and releases any associated resources.
	/// </summary>
	void Shutdown();

	/// <summary>
	/// Clears all currently tracked anchors and associated data.
	/// </summary>
	void ClearAnchors();

	/// <summary>
	/// Gets a collection of anchor data.
	/// </summary>
	IEnumerable<IAnchorData> AnchorData { get; }
}
