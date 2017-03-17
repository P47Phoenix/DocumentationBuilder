namespace Core.Ifx.Documentation.Services.Formators
{
    public interface IFormatorFactory
    {
        IFormator CreateFormator(IFormatorRequest formatorRequest);
    }
}