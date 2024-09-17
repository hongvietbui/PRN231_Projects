namespace Assignment01.DTO;

public class FilmDTO
{
    public int FilmID { get; set; }
    public string Genre { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string CountryCode { get; set; } = null!;
    public string FilmUrl { get; set; } = null!;
}