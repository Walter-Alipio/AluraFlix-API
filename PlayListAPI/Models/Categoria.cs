using System.Text.Json.Serialization;

namespace PlayListAPI.Models
{
	public class Categoria : IEntity
	{
		public int Id { get; set; }
		public string Title { get; set; } = string.Empty;
		public string Cor { get; set; } = string.Empty;
		[JsonIgnore]
		public virtual List<Video>? Videos { get; set; }
	}
}