namespace Assignment01.DTO.Request;

public class FilmDTO
{
    public int FilmID { get; set; }
    public int GenreID { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string CountryCode { get; set; } = null!;
    public string FilmUrl { get; set; } = null!;
}