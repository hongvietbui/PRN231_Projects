namespace Assignment01.DTO;

public class FilmResponseDTO
{
    public int FilmID { get; set; }
    public string Genre { get; set; }
    public string Title { get; set; } = null!;
    public int Year { get; set; }
    public string CountryName { get; set; } = null!;
    public string FilmUrl { get; set; } = null!;
}