namespace Assets.Scripts.GameLogic.Weapons
{
	public enum WeaponsType
	{
		Bullet,
		Missile
	}

	public class Weapons
	{
		#region Variables

		private WeaponsType _weaponsType;

		public WeaponsType Type
		{
			get { return _weaponsType; }
		}



		#endregion
	}
}
