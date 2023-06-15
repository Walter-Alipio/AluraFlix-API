using PlayListAPI.DTOs.VideosDTOs;
using dotenv.net;

namespace PlayListAPI.ViewModels;
internal class VideosPaginated
{
  private int Total { get; }
  private int Page { get; }
  private int PageSize { get; }
  private List<ReadVideoDTO> VideosPage { get; }
  private IConfiguration configuration;
  public VideosPaginated(int total, int page, int pageSize, List<ReadVideoDTO> videosPage)
  {
    Total = total;
    Page = page;
    PageSize = pageSize;
    VideosPage = videosPage;

    DotEnv.Load();
    var builder = new ConfigurationBuilder();
    builder.AddEnvironmentVariables();
    this.configuration = builder.Build();
  }

  public VideosPaginatedViewModel CreatePage()
  {
    if (!VideosPage.Any()) return null;

    var url = configuration.GetValue<string>("URL_SERVIDOR").Trim();
    int? previews = Page - 1;

    var previewsLink = previews <= 0 ? null : $"{url}/Videos/bypage?page={previews}&pageSize={PageSize}";

    var nextLink = $"{url}/Videos/bypage?page={Page + 1}&pageSize={PageSize}";

    var calcTotal = Total - Page * PageSize;

    if (calcTotal <= 0)
    {
      nextLink = null;
      calcTotal = 0;
    }

    return new VideosPaginatedViewModel()
    {
      Total = calcTotal + " itens restantes",
      Next = nextLink,
      Previews = previewsLink,
      Videos = VideosPage
    };
  }

}