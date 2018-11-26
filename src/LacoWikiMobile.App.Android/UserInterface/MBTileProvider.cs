namespace LacoWikiMobile.App.Droid.UserInterface
{
	using System.Data.SQLite;
	using Android.Gms.Maps.Model;

	public class MBTileProvider : Java.Lang.Object, ITileProvider
	{
		private string connectionString;

		public MBTileProvider(string paramConnectionString)
		{
			connectionString = paramConnectionString;
		}

		public Tile GetTile(int level, int col, int row)
		{
			var connection = new SQLiteConnection(connectionString);
			connection.Open();
			Tile tile = null;
			using (SQLiteCommand command = new SQLiteCommand(connection))
			{
				command.CommandText = "SELECT [tile_data] FROM [tiles] WHERE zoom_level = @zoom AND tile_column = @col AND tile_row = @row";
				command.Parameters.Add(new SQLiteParameter("zoom", level));
				command.Parameters.Add(new SQLiteParameter("col", col));
				command.Parameters.Add(new SQLiteParameter("row", row));
				var tileObj = command.ExecuteScalar();
				if (tileObj != null)
				{
					// make 256 not coded in hard way
					tile = new Tile(256, 256, (byte[])tileObj);
				}
			}

			connection.Close();
			connection.Dispose();

			return tile;
		}
	}
}