using System;
using Godot;

public partial class SceneManager : Node {
	[Export] public ProgressBar SceneChangeProgressBar;
	public static SceneManager Instance { get; private set; }

	private bool _isLoading = false;

	public override void _EnterTree()
	{
		Instance = this;
	}

	public override void _Ready()
	{
		SceneChangeProgressBar.Hide();

	}

	public async void ChangeScene(string path)
	{
		if (_isLoading) return;
		_isLoading = true;

		SceneChangeProgressBar.Value = 0;
		SceneChangeProgressBar.Show();

		Error err = ResourceLoader.LoadThreadedRequest(path);
		if (err != Error.Ok)
		{
			GD.PrintErr($"Couldn't start loading {path}: {err}");
			SceneChangeProgressBar.Hide();
			_isLoading = false;
			return;
		}

		var progress = new Godot.Collections.Array();
		while (true)
		{
			var status = ResourceLoader.LoadThreadedGetStatus(path, progress);

			if (status == ResourceLoader.ThreadLoadStatus.InProgress)
			{
				if (progress.Count > 0)
					SceneChangeProgressBar.Value = (double)progress[0] * 100;
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
				continue;
			}

			if (status == ResourceLoader.ThreadLoadStatus.Loaded)
			{
				SceneChangeProgressBar.Value = 100;
				var scene = (PackedScene)ResourceLoader.LoadThreadedGet(path);
				GetTree().ChangeSceneToPacked(scene);
			}
			else
			{
				GD.PrintErr($"Failed to load {path}: {status}");
			}
			break;
		}

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
		SceneChangeProgressBar.Hide();
		_isLoading = false;
	}
}
