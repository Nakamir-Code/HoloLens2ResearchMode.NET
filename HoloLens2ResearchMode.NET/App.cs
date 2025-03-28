using HoloLens2ResearchMode;
using StereoKit;
using System;
using System.ComponentModel;
using Windows.Foundation;

SK.Initialize();

ResearchModeSensorDevice sensorDevice = new();
ResearchModeSensorConsent requestCameraAccessTask = await sensorDevice.RequestCameraAccessAsync().AsTask();

Model model = Model.FromFile("Watermelon.glb");
Material floor = new("Floor.hlsl");
floor.Transparency = Transparency.Blend;
floor["color"] = new Color(1, 1, 1, 1);

SK.Run(() =>
{
	Mesh.Cube.Draw(floor, Matrix.TS(0, -1.5f, 0, V.XYZ(30, 0.01f, 30)));
	model.Draw(Matrix.TR(V.XYZ(0, 0, -0.5f), Quat.LookDir(0, 0, 1)));
});