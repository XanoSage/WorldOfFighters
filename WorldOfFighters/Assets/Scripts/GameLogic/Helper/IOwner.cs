using Assets.Scripts.GameLogic.Weapons;

namespace Assets.Scripts.GameLogic.Helper
{
	public interface IOwner
	{
		OwnerInfo Owner { get; set; }
	}
}