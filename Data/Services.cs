namespace SP2.Data
{
  public class Services
  {
    public Services(GoLogContext _context)
    {
      Context = _context;
    }

    GoLogContext Context { get; }
  }
}