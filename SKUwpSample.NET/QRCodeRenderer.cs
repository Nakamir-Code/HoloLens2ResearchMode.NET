// <copyright file="QRCodeRenderer.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
namespace SKUwpSample.NET;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StereoKit;
using StereoKit.Framework;

/// <summary>
/// Renders QR code anchors detected by the spatial anchor implementation.
/// </summary>
/// <param name="anchorFinder">
/// Provides access to anchor data from scanned QR codes.
/// </param>
public partial class QRCodeRenderer(ISpatialAnchor anchorFinder) : IStepper
{
	private readonly Sprite _playSprite = Sprite.FromFile("play.png", SpriteType.Single);

	/// <inheritdoc/>
	public bool Enabled => true;

	/// <inheritdoc/>
	public bool Initialize() => anchorFinder.IsRunning;

	/// <inheritdoc/>
	public void Shutdown() => anchorFinder.Shutdown();

	/// <summary>
	/// Guides the user to look at the QR marker and renders the anchor confirmation button.
	/// </summary>
	public void Step()
	{
		foreach (IAnchorData qrData in anchorFinder.AnchorData)
		{
			if ((qrData.Anchor.Tracked & BtnState.Inactive) > 0)
			{
				continue;
			}

			// We don't want to show the QR if it's not detected anymore, so let's specify a time limit!
			if (qrData.LastDetectedTime.AddSeconds(5) > DateTimeOffset.Now)
			{
				Pose qrPose = qrData.Anchor.Pose;
				Pose slightlyAboveQRPose = new(qrPose.ToMatrix().Transform(Vec3.Forward * 0.04f), qrPose.orientation * Quat.FromAngles(0, 0, 180));
				UI.PushSurface(slightlyAboveQRPose, Vec3.Zero, qrData.Size.XY);
				if (UI.ButtonRoundAt("Confirm_" + qrData.Data, _playSprite, Vec3.Zero, qrData.Size.x))
				{
					UI.PopSurface();
					Log.Info("Button clicked!");
					break;
				}
				UI.PopSurface();
			}
		}
	}
}
