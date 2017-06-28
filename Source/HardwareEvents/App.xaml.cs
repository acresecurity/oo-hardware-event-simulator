
namespace HardwareEvents
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            var container = TinyIoC.TinyIoCContainer.Current;
            container.AutoRegister(new []{ GetType().Assembly } );
        }
    }
}
