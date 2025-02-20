using StereoKit;

SK.Initialize();

Model    model = Model.FromFile("Watermelon.glb");
Material floor = new Material("Floor.hlsl");
floor.Transparency = Transparency.Blend;
floor["color"] = new Color(1,1,1,1);

SK.Run(() => {
	Mesh.Cube.Draw(floor, Matrix.TS(0,-1.5f,0, V.XYZ(30,0.01f,30)));
	model.Draw(Matrix.TR(V.XYZ(0,0,-0.5f), Quat.LookDir(0,0,1)));
});