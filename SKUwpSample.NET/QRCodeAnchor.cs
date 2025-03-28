// <copyright file="QRCodeAnchor.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
namespace SKUwpSample.NET;

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.MixedReality.QR;
using StereoKit;

/// <summary>
/// Used for detecting pose and text of QR marker.
/// </summary>
public class QRCodeAnchor : ISpatialAnchor
{
	private readonly ConcurrentDictionary<string, IAnchorData> _anchorData = [];

	/// <summary>
	/// Access status of the built-in QR module.
	/// </summary>
	private QRCodeWatcherAccessStatus _accessStatus;

	/// <summary>
	/// True, if the QR scanning is running.
	/// </summary>
	private long _isRunning;

	/// <summary>
	/// The built-in QR module.
	/// </summary>
	private QRCodeWatcher _qrTracker;

	/// <summary>
	/// Start time of QR watcher.
	/// QR codes detected before this time are filtered out.
	/// </summary>
	private DateTime _watcherStartTime;

	/// <inheritdoc/>
	public bool IsRunning => Interlocked.Read(ref _isRunning) == 1;

	/// <inheritdoc/>
	public IEnumerable<IAnchorData> AnchorData => _anchorData.Values;

	/// <inheritdoc/>
	public async Task<bool> IsAccessPermittedAsync()
	{
		if (!QRCodeWatcher.IsSupported())
		{
			Log.Err("QR Code Tracking is not supported.");
			return false;
		}

		_accessStatus = await QRCodeWatcher.RequestAccessAsync();
		return _accessStatus == QRCodeWatcherAccessStatus.Allowed;
	}

	/// <inheritdoc/>
	public void ClearAnchors() => _anchorData.Clear();

	/// <summary>
	/// Sets up the event system for the <see cref="QRCodeWatcher"/>.
	/// </summary>
	/// <returns>true, if setup was successful.</returns>
	protected void SetupTracking()
	{
		if (_qrTracker is not null)
		{
			return;
		}

		_qrTracker = new QRCodeWatcher();

		bool AddOrUpdateQR(QRCode qrCode, out IAnchorData qrData)
		{
			qrData = null;
			// QRCodeWatcher will provide QR codes from before session start,
			// so we often want to filter those out.
			if (qrCode.LastDetectedTime <= _watcherStartTime)
			{
				return false;
			}

			if (!World.FromSpatialNode(qrCode.SpatialGraphNodeId, out Pose qrPose))
			{
				Log.Err("Could not get QR pose from spatial node.");
				return false;
			}

			var anchor = Anchor.FromPose(qrPose);
			if (anchor is null)
			{
				Log.Err("Anchor is null!");
				return false;
			}

			qrData = new QRData()
			{
				Anchor = anchor,
				Data = qrCode.Data,
				Size = V.XY0(qrCode.PhysicalSideLength, qrCode.PhysicalSideLength),
				LastDetectedTime = qrCode.LastDetectedTime,
			};
			return true;
		}

		_qrTracker.Added += (_, qr) =>
		{
			if (!AddOrUpdateQR(qr.Code, out IAnchorData qrData))
			{
				return;
			}
			_anchorData[qrData.Data] = qrData;
			Log.Info("Added new QR marker. " + qrData.Data);
		};

		_qrTracker.Updated += (_, qr) =>
		{
			if (!AddOrUpdateQR(qr.Code, out IAnchorData qrData))
			{
				return;
			}
			_anchorData[qrData.Data] = qrData;
			Log.Info("Updated QR marker. " + qrData.Data);
		};

		_qrTracker.Removed += (_, qr) =>
		{
			// QRCodeWatcher will provide QR codes from before session start,
			// so we often want to filter those out.
			if (qr.Code.LastDetectedTime <= _watcherStartTime)
			{
				return;
			}

			_anchorData.TryRemove(qr.Code.Data, out IAnchorData _);
			Log.Info("Removed QR marker. " + qr.Code.Data);
		};
	}

	/// <summary>
	/// Initializes and runs QR Code tracking.
	/// </summary>
	public async Task InitializeAsync()
	{
		bool isAccessPermitted = await IsAccessPermittedAsync();
		if (!isAccessPermitted)
		{
			Log.Err("Anchor Tracking failed to start! No permission.");
			throw new InvalidOperationException("Anchor Tracking failed to start! No permission.");
		}

		SetupTracking();

		bool startTracking = StartTracking();
		if (!startTracking)
		{
			Log.Err("Unable to start tracking!");
			throw new InvalidOperationException("Unable to start tracking!");
		}
	}

	/// <inheritdoc/>
	public void Shutdown()
	{
		StopTracking();
		ClearAnchors();
	}

	/// <inheritdoc/>
	public bool StartTracking()
	{
		if (Interlocked.CompareExchange(ref _isRunning, 1, 0) == 1)
		{
			// We've already started playing
			Log.Err("We've already started the QR tracker!");
			return false;
		}

		Log.Info("AnchorFinder starting QRCodeWatcher");
		_watcherStartTime = DateTime.Now;

		try
		{
			// Start the QR code watcher
			_qrTracker.Start();
		}
		catch (Exception)
		{
			Log.Err("QR Code Watcher failed to start!");
			return false;
		}
		return true;
	}

	/// <inheritdoc/>
	public void StopTracking()
	{
		_qrTracker.Stop();
		Interlocked.Exchange(ref _isRunning, 0);
	}
}
