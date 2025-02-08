﻿namespace Assassins.Hub;

public interface IAssassinsClient
{
	public Task NotifyGameStateChanged();
	public Task NotifyKillHappened();
}