using System;


namespace _mrstruijk.Components.SaveSystem
{
	public class GameEvents
	{
		private static GameEvents _current;

		public static GameEvents current
		{
			get => _current ??= new GameEvents();
			set => _current = value;
		}

		public Action onLoadEvent;

	}
}
