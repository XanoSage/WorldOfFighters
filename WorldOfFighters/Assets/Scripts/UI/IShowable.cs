namespace Assets.Scripts.UI
{
	public interface IShowable
	{

		void Show();
		void Hide();
		bool Visible { get; set; }
	}
}
