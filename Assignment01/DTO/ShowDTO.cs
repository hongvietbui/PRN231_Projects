namespace Assignment01.DTO;

public class ShowDTO
{
    public int ShowID{ get; set; }
    public int RoomID { get; set; }
    public int FilmID { get; set; }
    public DateTime ShowDate { get; set; }
    public decimal Price { get; set; }
    public bool? Status { get; set; }
    public int Slot { get; set; }
}