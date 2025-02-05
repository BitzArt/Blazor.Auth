namespace BitzArt.Blazor.Auth.SampleApp;

public class SignInPayload
{
    public string? MyData { get; set; }

    public SignInPayload() { }

    public SignInPayload(string myData)
    {
        MyData = myData;
    }
}
