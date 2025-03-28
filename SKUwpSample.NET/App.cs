// <copyright file="App.cs" company="Nakamir, Inc.">
// Copyright (c) Nakamir, Inc. All rights reserved.
// </copyright>
using System;
using System.Threading.Tasks;
using HoloLens2ResearchMode;
using SKUwpSample.NET;
using StereoKit;
using Windows.ApplicationModel.Core;

SK.Initialize();

// Requests Research Mode camera access
_ = CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
{
	ResearchModeSensorDevice sensorDevice = new();
	ResearchModeSensorConsent requestCameraAccessTask = await sensorDevice.RequestCameraAccessAsync().AsTask();
});

// Adds QR tracking to the scene
ISpatialAnchor spatialAnchor = new QRCodeAnchor();
_ = Task.Run(async () =>
{
	// Note that you may want to request camera permissions on the UI thread
	// like above instead of the thread pool here
	await spatialAnchor.InitializeAsync();
	SK.AddStepper(new QRCodeRenderer(spatialAnchor));
});

Model model = Model.FromFile("Watermelon.glb");
SK.Run(() =>
{
	model.Draw(Matrix.TR(V.XYZ(0, 0, -0.5f), Quat.LookDir(0, 0, 1)));
});