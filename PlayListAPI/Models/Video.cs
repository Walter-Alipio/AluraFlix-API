namespace PlayListAPI.Models

{
  public class Video : IEntity
  {
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public int CategoriaId { get; set; }
    public virtual Categoria? Categoria { get; set; }
    public string AuthorId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifyAt { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedAt { get; set; } = null;
  }
}